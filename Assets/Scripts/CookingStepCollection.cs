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

    // 检查当前时间条上所有步骤是否合法，不合法变红色
    public void CheckDepend() {
        var OnTimeHolder = (from x in CookingSteps where x.Belong != null select x).ToList();
        OnTimeHolder.ForEach((cookingStep) => {
            Dragable dragable = cookingStep.GetComponent<Dragable>();
            RectTransform dragRect = dragable.dragRect;
            bool f1 = (from x in cookingStep.Depend
                      where x.Belong != null && x != cookingStep
                      select x.GetComponent<RectTransform>())
                      .Any((x) => x.anchoredPosition.x + x.sizeDelta.x > dragRect.anchoredPosition.x);
            Rect t = new Rect(dragRect.anchoredPosition.x, 0, dragRect.sizeDelta.x, 1);
            bool f2 = CookingSteps
            .Where((x) => x.Belong != null && x != cookingStep && (!x.CanParallel && !cookingStep.CanParallel))
            .Select((x) => x.GetComponent<RectTransform>())
            .Select((x) => new Rect(x.anchoredPosition.x, 0, x.sizeDelta.x, 1))
            .Any((x) => x.Overlaps(t));
            if (f1 || f2) dragable.frameImage.GetComponent<Image>().color = Color.red;
            else {
                if (cookingStep.CanParallel) dragable.frameImage.GetComponent<Image>().color = Color.green;
                else dragable.frameImage.GetComponent<Image>().color = Color.white;
            }
        });
    }


}
