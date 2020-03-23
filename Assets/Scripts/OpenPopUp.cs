using UnityEngine;

public class OpenPopUp : MonoBehaviour { // 处理打开弹窗键和关闭弹窗键

    [SerializeField] GameObject Black = null, PopUp = null;
    private AudioSource OperateSound;

    private void Start() {
        OperateSound = GameObject.Find("OperateSound").GetComponent<AudioSource>();
    }

    public void Click() {
        if (name.StartsWith("Open")) {
            OperateSound.Play(); Black.SetActive(true); PopUp.SetActive(true);
        }
        if (name.StartsWith("Close")) {
            OperateSound.Play(); Black.SetActive(false); PopUp.SetActive(false);
        }
    }

}
