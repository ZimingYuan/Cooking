using UnityEngine;

public class ToggleInstruct : MonoBehaviour { // 处理说明面板的左右移动

    [SerializeField] private InstructMenu menu;

    public void Click() {
        if (name == "Left") menu.ClickHandle(false);
        if (name == "Right") menu.ClickHandle(true);
    }
}
