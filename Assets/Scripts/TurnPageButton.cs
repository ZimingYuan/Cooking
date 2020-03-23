using UnityEngine;

public class TurnPageButton : MonoBehaviour { // 处理说明面板的左右移动

    [SerializeField] private TurnPage PopUp = null;
    private AudioSource OperateSound;

    private void Start() {
        OperateSound = GameObject.Find("OperateSound").GetComponent<AudioSource>();
    }

    public void Click() {
        if (name == "Left") {
            OperateSound.Play(); PopUp.ClickHandle(false);
        }
        if (name == "Right") {
            OperateSound.Play(); PopUp.ClickHandle(true);
        }
    }
}
