using System.Collections.Generic;

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

}
