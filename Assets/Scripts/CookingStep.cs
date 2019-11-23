using System.Collections.Generic;
using UnityEngine;

public class CookingStep: MonoBehaviour{

    public string Name;
    public int Time; // 持续时间
    public bool CanParallel; // 能否同时
    public Vector3 origin;
    public List<CookingStep> DirectDepend; // 直接依赖的步骤
    public List<CookingStep> Depend; // 直接或间接依赖的步骤
    public List<CookingStep> Control; // 直接或间接依赖本步骤的步骤

    public CookingStep(string name, int time, bool cp) {
        Name = name; Time = time; CanParallel = cp;
        DirectDepend = new List<CookingStep>();
        Depend = new List<CookingStep>();
        Control = new List<CookingStep>();
    }

    public void Copy(CookingStep old)
    {
        Name = old.Name;
        Time = old.Time;
        CanParallel = old.CanParallel;
        DirectDepend = old.DirectDepend;
        Depend = old.Depend;
        Control = old.Control;
    }
}
