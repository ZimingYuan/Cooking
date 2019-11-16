using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class DragUI: MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private GameObject panal;//时间轴一
    RectTransform rt;
 
    // 位置偏移量
    Vector3 offset = Vector3.zero;
    // 最小、最大X、Y坐标
    float minX, maxX, minY, maxY;
    
 
    void Start()
    {
        rt = GetComponent<RectTransform>();
        transform.SetSiblingIndex(1);
        
    }
    private void Update()
    {
        
    
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
        {
            // 计算偏移量
            offset = rt.position - globalMousePos;
        }
    }
 

    public void OnDrag(PointerEventData eventData)
    {
        // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            // 设置拖拽范围
            SetDragRange();
            rt.position = DragRangeLimit(globalMousePos + offset);
        }
    }
 
    // 设置最大、最小坐标
    void SetDragRange()
    {
        minX = rt.rect.width * rt.pivot.x;
        maxX = Screen.width - rt.rect.width * (1 - rt.pivot.x);
        minY = rt.rect.height * rt.pivot.y;
        maxY = Screen.height - rt.rect.height * (1 - rt.pivot.y);
    }
 
    // 限制坐标范围
    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
    public void OnEndDrag(PointerEventData eventData)
    {

    }
}