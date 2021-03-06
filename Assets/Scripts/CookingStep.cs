﻿using System.Collections.Generic;
using UnityEngine;

public class CookingStep: MonoBehaviour{

    public string Name;
    public int Time; // 持续时间
    public bool CanParallel; // 能否同时
    // public bool canDrag = true;
    public int ID;
    public float LastRight = -1000; //记录前依赖点的最右
    public List<CookingStep> DirectDepend; // 直接依赖的步骤,在游戏开始后不要修改
    public List<CookingStep> Depend; // 直接或间接依赖的步骤,在游戏开始后不要修改
    public List<CookingStep> Control; // 直接或间接依赖本步骤的步骤,在游戏开始后不要修改
    public List<CookingStep> DependNotSatisfied; // 实际游戏中该步骤未满足的依赖
    public TimeHolder Belong;
    public string spritePath;
    public int workshop; // 对应的动画编号
    public int StartTime; // 开始时间

    public void Init(string name, int time, bool cp,string path, int ws) {
        Name = name; Time = time; CanParallel = cp;
        // canDrag = true;
        DirectDepend = new List<CookingStep>();
        Depend = new List<CookingStep>();
        Control = new List<CookingStep>();
        DependNotSatisfied = new List<CookingStep>();
        spritePath = path;
        workshop = ws;
    }

}
