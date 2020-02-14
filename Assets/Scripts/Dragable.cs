using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Dragable : MonoBehaviour {

    // offset: 鼠标拖动前点击位置和物体原点位移
    [SerializeField] public Vector2 offset;
    
    //Holder
    private TimeHolder timeHolder1;
    private TimeHolder timeHolder2;
    private MenuHolder MenuHolder;
    //RectTransfrom
    [SerializeField]public RectTransform dragRect;
    private RectTransform timeRect1;
    private RectTransform timeRect2;
    private RectTransform menuRect;
    RectTransform rt;
    RectTransform frameImage;
    //step
    private CookingStep cookingStep;
    //clock
    private Transform[] children;
    private Transform clock;
    
    void Start() {
        dragRect = GetComponent<RectTransform>();
        transform.SetSiblingIndex(1); // 把这个物体放在最上，防止按不到

        MenuHolder = GameObject.FindWithTag("MenuHolder").GetComponent<MenuHolder>();
        timeHolder1 = GameObject.FindWithTag("TimeHolder1").GetComponent<TimeHolder>();
        timeHolder2 = GameObject.FindWithTag("TimeHolder2").GetComponent<TimeHolder>();

        menuRect = MenuHolder.transform.parent.GetComponent<RectTransform>();
        timeRect1 = timeHolder1.GetComponent<RectTransform>();
        timeRect2 = timeHolder2.GetComponent<RectTransform>();
        cookingStep = GetComponent<CookingStep>();

        children = GetComponentsInChildren<Transform>();
        clock = children.Where(x => x.name == "ClockImage").First();

         rt = children.Where(x => x.name == "Image").First().GetComponent<RectTransform>();
         frameImage = children.Where(x => x.name == "FrameImage").First().GetComponent<RectTransform>();

    }
    private void Update()
    {
        timeimage();
    }
    private void timeimage()
    {
        if (cookingStep.Belong) {
            clock.gameObject.SetActive(false); // 时间图片隐藏
            Color color = GetComponent<Image>().color;
            color.a = 1;
            GetComponent<Image>().color = color; // 背景出现
            rt.offsetMax = new Vector2(0, 0); // 把步骤图片（接受拖动事件的区域）居中并放大
            rt.GetComponent<CanvasRenderer>().SetAlpha(0); // 隐藏步骤图片
            Text nameText = GetComponentsInChildren<Text>().Where(x => x.name == "Name").First();
            //nameText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 步骤名字居中
            
            nameText.fontSize = 20; // 步骤名字放大
            //frameImage.GetComponent<CanvasRenderer>().SetAlpha(0);
            if(cookingStep.Belong ==timeHolder1)
            {
                nameText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 7);
                frameImage.anchoredPosition = new Vector2(0, 7);
                Vector2 size = GetComponent<RectTransform>().sizeDelta;
                frameImage.sizeDelta = size + new Vector2(0, 12);
            }
            else
            {
                nameText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -7);
                frameImage.anchoredPosition = new Vector2(0, -7);
                Vector2 size = GetComponent<RectTransform>().sizeDelta;
                frameImage.sizeDelta = size + new Vector2(0, 12);
            }
            nameText.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
            
            
            //frameImage.sizeDelta = GetComponent<RectTransform>().sizeDelta;
           

        } else {
            clock.gameObject.SetActive(true); // 时间图片出现
            Color color = GetComponent<Image>().color;
            color.a = 0;
            GetComponent<Image>().color = color; // 背景隐藏
            rt.offsetMax = new Vector2(-30, -20); // 把步骤图片（接受拖动事件的区域）居左并缩小
            rt.GetComponent<CanvasRenderer>().SetAlpha(1); // 步骤图片出现
            Text nameText = GetComponentsInChildren<Text>().Where(x => x.name == "Name").First();
            nameText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15, 40); //步骤名字置顶
            nameText.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 20);
            nameText.fontSize = 16; // 步骤名字缩小

            frameImage.anchoredPosition = new Vector2(-10, -10);
            frameImage.sizeDelta = new Vector2(110, 80);
        }
    }
    public void BeginDrag(PointerEventData eventData) { // 图片接受事件后转发到这里
        offset = eventData.position - new Vector2(dragRect.position.x, dragRect.position.y);
        if (cookingStep.canDrag)
        {
            transform.SetParent(MenuHolder.transform.parent.parent);
            MenuHolder.DragEffectBegin(this);
            timeHolder1.DragEffectBegin(this);
            timeHolder2.DragEffectBegin(this);

        }
    }

    public void Drag(PointerEventData eventData) {
        if (cookingStep.canDrag)
            dragRect.position = eventData.position - offset;
    }

    public void EndDrag(PointerEventData eventData) {
        if (cookingStep.canDrag) {
            if (cookingStep.Belong) { // 从时间条往外拖
                cookingStep.Belong.DeleteOrder();
                if (timeHolder1.ShadowRender.GetAlpha() < 0.5 && timeHolder2.ShadowRender.GetAlpha() < 0.5) {
                    // 步骤没有进入任何一个时间条，需要往菜单栏走
                    MenuHolder.DragEffectEndIn();
                    timeHolder1.DragEffectEndOut();
                    timeHolder2.DragEffectEndOut();
                } else if (cookingStep.Belong.ShadowRender.GetAlpha() < 0.5) {
                    // 步骤去了另一个时间条的区域
                    MenuHolder.DragEffectEndOut();
                    cookingStep.Belong.DragEffectEndOut();
                    (cookingStep.Belong == timeHolder1 ? timeHolder2 : timeHolder1).DragEffectEndIn();
                } else {
                    // 步骤还在原来的时间条区域内
                    MenuHolder.DragEffectEndOut();
                    cookingStep.Belong.DragEffectEndIn();
                    (cookingStep.Belong == timeHolder1 ? timeHolder2 : timeHolder1).DragEffectEndOut();
                }
            } else { // 从菜单栏往外拖
                if (timeHolder1.ShadowRender.GetAlpha() > 0.5) { // 进入时间条1
                    MenuHolder.DragEffectEndOut();
                    timeHolder1.DragEffectEndIn();
                    timeHolder2.DragEffectEndOut();
                } else if (timeHolder2.ShadowRender.GetAlpha() > 0.5) { // 进入时间条2
                    MenuHolder.DragEffectEndOut();
                    timeHolder2.DragEffectEndIn();
                    timeHolder1.DragEffectEndOut();
                } else { // 都没进入，回菜单栏
                    MenuHolder.DragEffectEndIn();
                    timeHolder1.DragEffectEndOut();
                    timeHolder2.DragEffectEndOut();
                }
            }
        }
    }

    public void SetDragSize(Vector2 size)
    {
        dragRect.sizeDelta = size;
    }

}
