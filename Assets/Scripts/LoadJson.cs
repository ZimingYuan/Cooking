// 弃用
using LitJson;
using System.IO;
using UnityEngine;

public static class LoadJson {

    // 读入Json文件转换为烹饪步骤集合类,filename为Json文件在Assets下的路径和文件名，如/Jsons/test.json
    public static CookingStepCollection Load(string filename) {
        StreamReader streamreader = new StreamReader(Application.dataPath + filename);
        JsonReader jr = new JsonReader(streamreader);
        JsonData jd = JsonMapper.ToObject(jr);
        CookingStepCollection csc = new CookingStepCollection();
        foreach (JsonData i in jd) {
            CookingStep cs = new CookingStep((string)i["名字"], (int)i["持续时间"], (bool)i["能否同时"],(string)i["图片"]);
            
            csc.CookingSteps.Add(cs);
        }
        for (int i = 0; i < jd.Count; i++) {
            CookingStep cs = csc.CookingSteps[i];
            JsonData depend = jd[i]["前置条件"];
            foreach (JsonData j in depend) cs.DirectDepend.Add(csc.FindByName((string)j));
        }
        csc.CalcDepend();
        return csc;
    }

}
