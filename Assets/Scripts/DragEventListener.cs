using UnityEngine;
using UnityEngine.EventSystems;

public class DragEventListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //将处理监听事件的对象改为图片本身，这样方便拖滚动条

    public void OnBeginDrag(PointerEventData eventData) {
        GetComponentInParent<Dragable>().BeginDrag(eventData); // 进行转发
    }

    public void OnDrag(PointerEventData eventData) {
        GetComponentInParent<Dragable>().Drag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        GetComponentInParent<Dragable>().EndDrag(eventData);
    }
}
