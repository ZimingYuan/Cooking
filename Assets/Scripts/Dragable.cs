using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // fromPlace: 从哪个容器拖出来，toPlace: 拖到哪个容器
    [SerializeField] public Place fromPlace, toPlace;
    // fromPosition: 拖之前的坐标，offset: 鼠标拖动前点击位置和物体原点位移
    private Vector2 fromPosition, offset;
    // 这个物体的坐标系
    private RectTransform rt;
    //Menu
    private GameObject menu;

    void Start() {
        rt = GetComponent<RectTransform>();
        fromPosition = rt.position;
        transform.SetSiblingIndex(1); // 把这个物体放在最上，防止按不到
        menu = GameObject.FindWithTag("MenuHolder");
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("1111");
        offset = eventData.position - new Vector2(rt.position.x, rt.position.y);
        fromPosition = this.transform.position;
        toPlace.DragEffectBegin(this);
    }

    public void OnDrag(PointerEventData eventData) {
        rt.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (toPlace.DragEffectEnd(this)) { // 如果到达了拖入区域
            fromPlace.DragAway();
            //gameObject.SetActive(false); Destroy(gameObject);
        } else { // 回去
            //rt.position = this.GetComponent<CookingStep>().origin;
            //transform.parent = menu.transform;
            rt.position = fromPosition;
        }
    }

}
