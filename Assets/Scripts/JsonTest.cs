using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour {

    void Start() {
        CookingStepCollection csc = LoadJson.Load("/Jsons/test.json");
        Debug.Log(csc.CookingSteps[2].Name);
        Debug.Log(csc.CookingSteps[2].Time);
        Debug.Log(csc.CookingSteps[2].CanParallel);
        csc.CookingSteps[2].Control.ForEach((x) => Debug.Log(x.Name));
    }

    void Update() {
        
    }
}
