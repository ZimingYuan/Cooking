﻿using System.Collections.Generic;
using UnityEngine;

public class CookingStep: MonoBehaviour{

    public string Name;
    public int Time; // 持续时间
    public bool CanParallel; // 能否同时
    public bool canDrag = true;
    public int ID;
    public float LastRight; //记录前依赖点的最右
    public List<CookingStep> DirectDepend; // 直接依赖的步骤
    public List<CookingStep> Depend; // 直接或间接依赖的步骤
    public List<CookingStep> Control; // 直接或间接依赖本步骤的步骤
    public List<CookingStep> sholdDepend;

    public CookingStep(string name, int _ID, int time, bool cp) {
        Name = name; Time = time; CanParallel = cp;ID = _ID;
        canDrag = true;
        DirectDepend = new List<CookingStep>();
        Depend = new List<CookingStep>();
        Control = new List<CookingStep>();
        sholdDepend = new List<CookingStep>();
    }

    public void Copy(CookingStep old)
    {
        Name = old.Name;
        Time = old.Time;
        CanParallel = old.CanParallel;
        canDrag = old.canDrag;
        ID = old.ID;
        DirectDepend = old.DirectDepend;
        Depend = old.Depend;
        Control = old.Control;
        sholdDepend = old.sholdDepend;
    }
}
