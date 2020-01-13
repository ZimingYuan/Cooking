using System.Collections.Generic;
using UnityEngine;

public class CookingStep: MonoBehaviour{

    public string Name;
    public int Time; // 持续时间
    public bool CanParallel; // 能否同时
    public bool canDrag = true;
    public int ID;
    public float LastRight = -1000; //记录前依赖点的最右
    public List<CookingStep> DirectDepend; // 直接依赖的步骤,在游戏开始后不要修改
    public List<CookingStep> Depend; // 直接或间接依赖的步骤,在游戏开始后不要修改
    public List<CookingStep> Control; // 直接或间接依赖本步骤的步骤,在游戏开始后不要修改
    public List<CookingStep> DependNotSatisfied; // 实际游戏中该步骤未满足的依赖
    public TimeHolder Belong;
    public string spritePath;

    public CookingStep(string name, int time, bool cp,string path) {
        Name = name; Time = time; CanParallel = cp;
        canDrag = true;
        DirectDepend = new List<CookingStep>();
        Depend = new List<CookingStep>();
        Control = new List<CookingStep>();
        DependNotSatisfied = new List<CookingStep>();
        spritePath = path;
    }

    public void Copy(CookingStep old)
    {
        Name = old.Name;
        Time = old.Time;
        CanParallel = old.CanParallel;
        canDrag = old.canDrag;
        DirectDepend = old.DirectDepend;
        Depend = old.Depend;
        Control = old.Control;
        DependNotSatisfied = old.DependNotSatisfied;
        spritePath = old.spritePath;
    }

}
