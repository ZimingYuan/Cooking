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

    private Vector2 unitSize = new Vector2(200, 100);

    void Start() {
        gameController = GameController.GetInstance();
        InitializedMenu("/Jsons/test.json");

        //Texture2D sprite= Resources.Load("SteamEgg") as Texture2D;
        //GameObject game = Resources.Load("app") as GameObject;
        //Debug.Log(game);
        //Debug.Log(sprite);
    }

    void Update() {
        
    }

    public override void DragEffectBegin(Dragable d) {

    }

    public override void DragEffectEnd(Dragable d) {

        CookingStep addStep = d.GetComponent<CookingStep>(); // 被拖的步骤
        foreach(var step in addStep.Control) // 依赖addStep且在时间条上的自动弹回菜单栏
        {
            if(!step.DependNotSatisfied.Exists(t => t.ID== addStep.ID))
            {
                step.DependNotSatisfied.Add(addStep);
                step.transform.parent = this.transform;
                step.GetComponent<Dragable>().SetDragSize(unitSize);
                step.canDrag = false;
                step.Belong = null;
            }
        }

        d.transform.parent = this.transform;
        d.GetComponent<CookingStep>().Belong = null;
        d.SetDragSize(unitSize);
    }

    public override void DragAway() {

    }

    private void InitializedMenu(string filename)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + filename);
        JsonReader jr = new JsonReader(streamreader);
        JsonData jd = JsonMapper.ToObject(jr);
        foreach (JsonData i in jd)
        {
            CookingStep cs = new CookingStep((string)i["名字"], (int)i["ID"], (int)i["持续时间"], (bool)i["能否同时"],(string)i["图片"]);
            CookingStep tmp = Instantiate(stepPrefab, this.transform);
            tmp.Copy(cs);
            var drag = tmp.GetComponent<Dragable>();
            drag.fromPlace =transform.parent.GetComponent<Place>();
            //drag.toPlace = GameObject.FindWithTag("TimeHolder1").GetComponent<Place>();
            gameController.stepCollection.CookingSteps.Add(tmp);
        }
        for (int i = 0; i < jd.Count; i++)
        {
            CookingStep cs = gameController.stepCollection.CookingSteps[i];
            string path = "Images/" + cs.spritePath;
            Debug.Log(path);
            Texture2D tex = Resources.Load(path) as Texture2D;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            Debug.Log(sprite);
            cs.GetComponent<Image>().sprite = sprite;
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
        Debug.Log(gameController.stepCollection.CookingSteps.Count);
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
