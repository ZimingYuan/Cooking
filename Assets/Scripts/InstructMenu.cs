using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InstructMenu : MonoBehaviour {

    private string dishinfo, dishinst;
    private Text title, text;
    private GameObject left, right;

    void Start() {
        GameController gc = GameController.GetInstance();
        dishinfo = gc.dishinfo; dishinst = gc.dishinst;
        title = GetComponentsInChildren<Text>().Where(x => x.name == "Title").First();
        text = GetComponentsInChildren<Text>().Where(x => x.name == "Text").First();
        left = GetComponentsInChildren<RectTransform>().Where(x => x.name == "Left").First().gameObject;
        right = GetComponentsInChildren<RectTransform>().Where(x => x.name == "Right").First().gameObject;
        ClickHandle(false);
    }

    public void ClickHandle(bool isRight) { // 处理左右翻页
        if (! isRight) {
            left.SetActive(false);
            right.SetActive(true);
            title.text = "菜式";
            text.text = dishinfo;
        } else {
            left.SetActive(true);
            right.SetActive(false);
            title.text = "介绍";
            text.text = dishinst;
        }
    }

}
