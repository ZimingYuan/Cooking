using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class MenuHolder: Place {

    [SerializeField] private CookingStep stepPrefab;
    List<CookingStep> steps;
    GameController gameController;
    private Dragable drag;

    private Vector2 unitSize = new Vector2(200, 100); // 在菜单栏里的步骤的大小

    void Start() {
        gameController = GameController.GetInstance();
        InitializedMenu("/Jsons/test.json");
    }

    void Update() {
        
    }

    public override void DragEffectBegin(Dragable d) {
        drag = d;
    }

    public override void DragEffectEndIn() {

        CookingStep addStep = drag.GetComponent<CookingStep>(); // 被拖的步骤
        foreach(var step in addStep.Control) // 依赖addStep且在时间条上的自动弹回菜单栏
        {
            if(!step.DependNotSatisfied.Exists(t => t.name == addStep.name))
            {
                step.DependNotSatisfied.Add(addStep);
                step.transform.SetParent(transform);
                step.GetComponent<Dragable>().SetDragSize(unitSize);
                step.canDrag = false;
                step.Belong = null;
            }
        }
        drag.transform.SetParent(transform);
        drag.GetComponent<CookingStep>().Belong = null;
        drag.SetDragSize(unitSize);
        drag = null;
        gameController.stepCollection.CheckDepend();
    }

    public override void DragEffectEndOut() {
        drag = null;
    }

    private void InitializedMenu(string filename)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + filename);
        JsonReader jr = new JsonReader(streamreader);
        JsonData jd = JsonMapper.ToObject(jr);
        foreach (JsonData i in jd)
        {
            CookingStep cs = new CookingStep((string)i["名字"], (int)i["持续时间"], (bool)i["能否同时"],(string)i["图片"]);
            CookingStep tmp = Instantiate(stepPrefab, transform);
            tmp.Copy(cs);
            var drag = tmp.GetComponent<Dragable>();
            gameController.stepCollection.CookingSteps.Add(tmp);
        }
        for (int i = 0; i < jd.Count; i++)
        {
            CookingStep cs = gameController.stepCollection.CookingSteps[i];
            string path = "Images/" + cs.spritePath;
            Sprite sprite = Resources.Load<Sprite>(path);
            Image t = cs.GetComponentsInChildren<Image>()[1];
            t.sprite = sprite; t.preserveAspect = true;
            JsonData depend = jd[i]["前置条件"];
            foreach (JsonData j in depend) cs.DirectDepend.Add(gameController.stepCollection.FindByName((string)j));
        }
        gameController.stepCollection.CalcDepend();        
        gameController.stepCollection.CookingSteps.ForEach((x) =>
            x.Depend.ForEach((y) =>
                x.DependNotSatisfied.Add(y)
            )
        );

        foreach(var step in gameController.stepCollection.CookingSteps)
        {
            if (step.DependNotSatisfied.Count > 0)
            {
                step.canDrag = false;
            }
        }
    }


    public static List<T> Clone<T>(object List)
    {
        using (Stream objectStream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, List);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as List<T>;
        }
    }

}
