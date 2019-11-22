using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // fromPlace: 从哪个容器拖出来，toPlace: 拖到哪个容器
    [SerializeField] private Place fromPlace, toPlace;
    // fromPosition: 拖之前的坐标，offset: 鼠标拖动前点击位置和物体原点位移
    private Vector2 fromPosition, offset;
    // 这个物体的坐标系
    private RectTransform rt;

    void Start() {
        rt = GetComponent<RectTransform>();
        fromPosition = rt.position;
        transform.SetSiblingIndex(1); // 把这个物体放在最上，防止按不到
    }

    public void OnBeginDrag(PointerEventData eventData) {
        offset = eventData.position - new Vector2(rt.position.x, rt.position.y);
        toPlace.DragEffectBegin(this);
    }

    public void OnDrag(PointerEventData eventData) {
        rt.position = eventData.position - offset;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (toPlace.DragEffectEnd()) { // 如果到达了拖入区域
            fromPlace.DragAway();
            gameObject.SetActive(false); Destroy(gameObject);
        } else { // 回去
            rt.position = fromPosition;
        }
    }

}
