using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HelpPopUp : TurnPage {

    private int index;
    private Text text;
    private GameObject left, right;
    private string[] HelpText = {
        "根据菜谱来排列组合步骤，在尽可能短的时间内完成菜肴",
        "将左侧的步骤拖到右侧的时间轴上即可生效",
        "大部分情况下，你只能同时进行一个步骤，这也意味着一个时间点上只能有一个步骤正在进行",
        "然而对于有些步骤，例如蒸某样菜品，你可以同时完成其他的操作。这些特殊步骤在时间轴上会以绿色表明。",
        "如果时间轴上某一个步骤变红，那么就说明他的顺序不对或者是有其他步骤正在进行。"
    };

    void Start() {
        text = GetComponentsInChildren<Text>().Where(x => x.name == "Text").First();
        left = GetComponentsInChildren<RectTransform>().Where(x => x.name == "Left").First().gameObject;
        right = GetComponentsInChildren<RectTransform>().Where(x => x.name == "Right").First().gameObject;
        index = 1;  ClickHandle(false);
    }

    public override void ClickHandle(bool isRight) { // 处理左右翻页
        if (! isRight) {
            index--; if (index == 0) left.SetActive(false);
            right.SetActive(true); text.text = HelpText[index];
        } else {
            index++; if (index == HelpText.Length - 1) right.SetActive(false);
            left.SetActive(true); text.text = HelpText[index];
        }
    }

}
