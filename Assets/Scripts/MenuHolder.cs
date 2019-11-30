using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using LitJson;

public class MenuHolder: Place {
    [SerializeField]
    private CookingStep stepPrefab;
    List<CookingStep> steps;
    GameController gameController;

    private Vector2 unitSize = new Vector2(200, 100);

    void Start() {
        //gameController.stepCollection = LoadJson.Load("/Jsons/test.json");
        //Debug.Log(gameController.stepCollection.CookingSteps[1].Depend);
        gameController = GameController.GetInstance();
        InitializedMenu("/Jsons/test.json");
    }

    void Update() {
        
    }

    public override void DragEffectBegin(Dragable d) {

    }

    public override bool DragEffectEnd(Dragable d) {

        CookingStep addStep = d.GetComponent<CookingStep>();
        //foreach(var step in gameController.stepCollection.CookingSteps)
        //{
        //    //var tmp = step.sholdDepend.Exists(t => t.ID == addStep.ID);
        //    if (step.sholdDepend.Exists(t => t.ID == addStep.ID))
        //    {
        //        step.DirectDepend.Add(addStep);
        //        step.transform.parent = this.transform;
        //    }
                
        //}

        foreach(var step in addStep.Control)
        {
            if(!step.DirectDepend.Exists(t => t.ID== addStep.ID))
            {
                step.DirectDepend.Add(addStep);
                step.transform.parent = this.transform;
                step.GetComponent<Dragable>().SetDragSize(unitSize);
                step.canDrag = false;
            }
        }

        d.transform.parent = this.transform;
        d.SetDragSize(unitSize);
        return false;
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
            CookingStep cs = new CookingStep((string)i["名字"], (int)i["ID"], (int)i["持续时间"], (bool)i["能否同时"]);
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
            JsonData depend = jd[i]["前置条件"];
            foreach (JsonData j in depend) cs.DirectDepend.Add(gameController.stepCollection.FindByName((string)j));
            foreach (var dep in cs.DirectDepend)
                cs.sholdDepend.Add(dep);
            //cs.sholdDepend = Clone<CookingStep>(cs.DirectDepend);
        }
        gameController.stepCollection.CalcDepend();        

        foreach(var step in gameController.stepCollection.CookingSteps)
        {
            if (step.DirectDepend.Count > 0)
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
