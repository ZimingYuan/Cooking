using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // fromPlace: 从哪个容器拖出来，toPlace: 拖到哪个容器
    [SerializeField] public Place fromPlace, toPlace;
    // fromPosition: 拖之前的坐标，offset: 鼠标拖动前点击位置和物体原点位移
    private Vector2 fromPosition, offset;
    //Holder
    private TimeHolder timeHolder1;
    private TimeHolder timeHolder2;
    private MenuHolder MenuHolder;
    //RectTransfrom
    private RectTransform dragRect;
    private RectTransform timeRect1;
    private RectTransform timeRect2;
    private RectTransform menuRect;

    void Start() {
        dragRect = GetComponent<RectTransform>();
        fromPosition = dragRect.position;
        transform.SetSiblingIndex(1); // 把这个物体放在最上，防止按不到

        MenuHolder = GameObject.FindWithTag("MenuHolder").GetComponent<MenuHolder>();
        timeHolder1 = GameObject.FindWithTag("TimeHolder1").GetComponent<TimeHolder>();
        timeHolder2 = GameObject.FindWithTag("TimeHolder2").GetComponent<TimeHolder>();

        menuRect = MenuHolder.GetComponent<RectTransform>();
        timeRect1 = timeHolder1.GetComponent<RectTransform>();
        timeRect2 = timeHolder2.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        offset = eventData.position - new Vector2(dragRect.position.x, dragRect.position.y);
        fromPosition = this.transform.position;
        Debug.Log(fromPosition);
        timeHolder1.DragEffectBegin(this);
        timeHolder2.DragEffectBegin(this);
    }

    public void OnDrag(PointerEventData eventData) {
        dragRect.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData) {
        toPlace = null;
        if (dragRect.position.x > timeRect1.position.x)
        {
            if (dragRect.position.y >= timeRect1.position.y - timeRect1.rect.height &&
                dragRect.position.y <= timeRect1.position.y + timeRect1.rect.height)
            {
                toPlace = timeHolder1;
            }
            else if (dragRect.position.y <= timeRect2.position.y + timeRect2.rect.height &&
                      dragRect.position.y >= timeRect2.position.y - timeRect2.rect.height)
                toPlace = timeHolder2;
        }
        else if (dragRect.position.x <= menuRect.position.x + menuRect.rect.width &&
                  dragRect.position.x >= menuRect.position.x - menuRect.rect.width &&
                  dragRect.position.y <= menuRect.position.y + menuRect.rect.height &&
                  dragRect.position.y >= menuRect.position.y - menuRect.rect.height)
            toPlace = MenuHolder;

        if(toPlace)
        {
            if (toPlace.DragEffectEnd(this))
            { // 如果到达了拖入区域
                fromPlace.DragAway();
            }
        }
        else
        { // 回去
            dragRect.position = fromPosition;
            //dragRect.parent = toPlace.transform;
        }
    }

}
