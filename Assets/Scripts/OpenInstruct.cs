using UnityEngine;

public class OpenInstruct : MonoBehaviour { // 处理打开说明键（小菜谱）和关闭说明键（说明上的叉）

    [SerializeField] GameObject Menu;

    public void Click() {
        if (name == "OpenInstruct") Menu.SetActive(true); 
        if (name == "CloseInstruct") Menu.SetActive(false); 
    }

}
