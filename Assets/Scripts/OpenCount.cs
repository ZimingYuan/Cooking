using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OpenCount : MonoBehaviour {

    private CookingStepCollection csp;
    [SerializeField] GameObject Black = null, Count = null;

    void Start() {
        csp = FindObjectOfType<GameController>().stepCollection;
    }

    public void Click() {
        if (csp.CookingSteps
            .Where((x) => x.Belong != null)
            .Select((x) =>
                (from y in x.GetComponentsInChildren<Image>()
                 where y.name == "FrameImage" select y.color).First())
            .Where((x) => x != Color.red)
            .Count() == csp.CookingSteps.Count) {
            GameObject.Find("OperateSound").GetComponent<AudioSource>().Play();
            Black.SetActive(true);
            Count.SetActive(true);
            int MaxTime = csp.CookingSteps.Select((x) => x.StartTime + x.Time).Max();
            Count.GetComponentsInChildren<Text>().Where((x) => x.name == "Text").First()
                .text = string.Format("你的过关时间为：{0}分钟", MaxTime);
        }

    }
}
