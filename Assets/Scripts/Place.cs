using UnityEngine;

// 容器抽象类
public abstract class Place: MonoBehaviour {

    // 当对应的东西开始拖动目标容器的反应
    public abstract void DragEffectBegin(Dragable d);
    // 当对应的东西拖动结束接受容器的反应
    public abstract void DragEffectEndIn();
    // 当对应的东西拖动结束未接受容器的反应
    public abstract void DragEffectEndOut();

}
