using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class MenuHolder: Place {
    CookingStepCollection stepCollection;
    [SerializeField]
    private CookingStep stepPrefab;
    List<CookingStep> steps;

    void Start() {
        //stepCollection = LoadJson.Load("/Jsons/test.json");
        //Debug.Log(stepCollection.CookingSteps[1].Depend);
        InitializedMenu("/Jsons/test.json");
        
    }

    void Update() {
        
    }

    public override void DragEffectBegin(Dragable d) {

    }

    public override bool DragEffectEnd() {
        return false;
    }

    public override void DragAway() {

    }

    private void InitializedMenu(string filename)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + filename);
        JsonReader jr = new JsonReader(streamreader);
        JsonData jd = JsonMapper.ToObject(jr);
        stepCollection = new CookingStepCollection();
        foreach (JsonData i in jd)
        {
            CookingStep cs = new CookingStep((string)i["名字"], (int)i["持续时间"], (bool)i["能否同时"]);
            CookingStep tmp = Instantiate(stepPrefab, transform);
            tmp.origin = tmp.transform.position;
            tmp.Copy(cs);
            var drag = tmp.GetComponent<Dragable>();
            drag.fromPlace =GameObject.FindWithTag("MenuHolder").GetComponent<Place>();
            drag.toPlace = GameObject.FindWithTag("TimeHolder").GetComponent<Place>();
            stepCollection.CookingSteps.Add(tmp);
        }
        for (int i = 0; i < jd.Count; i++)
        {
            CookingStep cs = stepCollection.CookingSteps[i];
            JsonData depend = jd[i]["前置条件"];
            foreach (JsonData j in depend) cs.DirectDepend.Add(stepCollection.FindByName((string)j));
        }
        stepCollection.CalcDepend();

    }

}
