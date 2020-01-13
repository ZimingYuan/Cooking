using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CookingStepCollection {

    public List<CookingStep> CookingSteps;

    public CookingStepCollection() {
        CookingSteps = new List<CookingStep>();
    }

    // 根据名字查找烹饪步骤对象
    public CookingStep FindByName(string name) {
        return CookingSteps.Find((x) => x.Name == name);
    }

    private void Dfs(CookingStep x, List<CookingStep> y) {
        y.Add(x); x.DirectDepend.ForEach((z) => Dfs(z, y));
    }

    // 计算烹饪步骤之间的依赖关系
    public void CalcDepend() {
        CookingSteps.ForEach((x) => x.DirectDepend.ForEach((y) => Dfs(y, x.Depend)));
        CookingSteps.ForEach((x) => x.Depend.ForEach((y) => y.Control.Add(x)));
    }

    public void CheckDepend() { // 把不满足依赖的变成红色，其他变成黄色
        foreach (var i in CookingSteps) {
            float pos = i.GetComponent<RectTransform>().anchoredPosition.x;
            if (i.Belong != null
            && (i.Depend.Any((x) => x.Belong == null)
            || i.Depend
                .Select((x) => x.GetComponent<RectTransform>())
                .Select((x) => x.anchoredPosition.x + x.sizeDelta.x)
                .Any((x) => x > pos))) {
                i.GetComponent<Image>().color = new Color(1, 0, 0);
                i.GetComponentsInChildren<Image>().Where((x) => x.name == "Image").First().color = new Color(1, 0, 0);
            } else {
                i.GetComponent<Image>().color = new Color(0.9058f, 0.8274f, 0.5647f);
                i.GetComponentsInChildren<Image>().Where((x) => x.name == "Image").First().color = new Color(1, 1, 1);
            }
        }
    }

}
