using System.Collections.Generic;

public class CookingStep {

    public string Name;
    public int Time; // 持续时间
    public bool CanParallel; // 能否同时
    public List<CookingStep> DirectDepend; // 直接依赖的步骤
    public List<CookingStep> Depend; // 直接或间接依赖的步骤
    public List<CookingStep> Control; // 直接或间接依赖本步骤的步骤

    public CookingStep(string name, int time, bool cp) {
        Name = name; Time = time; CanParallel = cp;
        DirectDepend = new List<CookingStep>();
        Depend = new List<CookingStep>();
        Control = new List<CookingStep>();
    }

}
