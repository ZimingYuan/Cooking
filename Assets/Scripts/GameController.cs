using UnityEngine.UI;
using UnityEngine;

public class GameController :MonoBehaviour
{
    //改成不是单例了，因为含有静态方法的类跳转场景时不会销毁，原来那样会导致出现很多gamecontroller使程序混乱
    
    public CookingStepCollection stepCollection;
    public CanvasGroup finishedPanel;
    // public Text text;
    [HideInInspector] public string dishinfo, dishinst;
    // private static GameController instance = new GameController();
    // Holder
    private TimeHolder timeHolder1;
    private TimeHolder timeHolder2;
    private MenuHolder MenuHolder;

    // public int maxTime = 0;

    // private GameController() {
    //     stepCollection = new CookingStepCollection();
    // }
    
    // public static GameController GetInstance()
    // {
    //     return instance;
    // }

    private void Awake() { // 必须比别的脚本先调用
        Screen.SetResolution(1600, 900, false);
        stepCollection = new CookingStepCollection();
        MenuHolder = GameObject.FindWithTag("MenuHolder").GetComponent<MenuHolder>();
        timeHolder1 = GameObject.FindWithTag("TimeHolder1").GetComponent<TimeHolder>();
        timeHolder2 = GameObject.FindWithTag("TimeHolder2").GetComponent<TimeHolder>();
    }

    /* public void Finish()
    {
        //if(MenuHolder.transform.childCount==0)
        {
            instance.maxTime = timeHolder1.maxTime > timeHolder2.maxTime ? timeHolder1.maxTime : timeHolder2.maxTime;
            Debug.Log(instance.maxTime);
            finishedPanel.alpha = 1;
            finishedPanel.interactable = true;
            finishedPanel.blocksRaycasts = true;
            text.text = ("耗时：" + instance.maxTime);
        }
    }

    public void ClosePanel()
    {
        finishedPanel.alpha = 0;
        finishedPanel.interactable = false;
        finishedPanel.blocksRaycasts = false;
    } */

}
