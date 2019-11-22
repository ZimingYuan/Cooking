using UnityEngine;

// 容器抽象类
public abstract class Place: MonoBehaviour {

    // 当对应的东西开始拖动目标容器的反应
    public abstract void DragEffectBegin(Dragable d);
    // 当对应的东西拖动结束目标容器的反应
    public abstract bool DragEffectEnd();
    // 对应的东西拖动前所在的容器在拖动结束后的收尾操作
    public abstract void DragAway();

}
