using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class DragUI: MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]private GameObject panal;//时间轴一
    RectTransform rt;
 
    // 位置偏移量
    Vector3 offset = Vector3.zero;
    // 最小、最大X、Y坐标
    float minX, maxX, minY, maxY;
    //道具位置初始值
    private readonly float locationX = 167;
    private readonly float locationY = 232;
    //
    private bool inpanal;
    //原始宽度
    private float p_width = 100;
    private float p_height = 100;
    //进入panal里的宽度
    private float m_width = 40.0f;
 
 
    void Start()
    {
        inpanal = false;
        rt = GetComponent<RectTransform>();
        transform.SetSiblingIndex(1);
        
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            float nowX = gameObject.transform.position.x;
            float nowY = gameObject.transform.position.y;
            if (nowX > 335 && nowY > 229 && nowY < 280) inpanal = true;
            if (inpanal == false)
            {
                gameObject.transform.position = new Vector3(locationX, locationY, 0.0f);
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(p_width, p_height);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_width, 50.4f);
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            inpanal = false;
        }
    
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