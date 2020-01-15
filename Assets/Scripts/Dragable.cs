using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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
        cookingStep = this.GetComponent<CookingStep>();

        children = GetComponentsInChildren<Transform>();
        clock = children[2];
    }
    private void Update()
    {
        timeimage();
    }
    //把菜拖上去把时间去掉
    private void timeimage()
    {
        if (cookingStep.Belong){
            clock.gameObject.SetActive(false);
        }
        else clock.gameObject.SetActive(true);
    }
    public void OnBeginDrag(PointerEventData eventData) {
        offset = eventData.position - new Vector2(dragRect.position.x, dragRect.position.y);
        if (cookingStep.canDrag)
        {
            transform.SetParent(MenuHolder.transform.parent.parent);
            MenuHolder.DragEffectBegin(this);
            timeHolder1.DragEffectBegin(this);
            timeHolder2.DragEffectBegin(this);

        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (cookingStep.canDrag)
            dragRect.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData) {
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
