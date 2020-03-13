using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeHolder: Place {

    private float minScale = 50;
    // 阴影和显示在时间轴的步骤的prefab
    [SerializeField] private GameObject ShadowModel;
    // 阴影、自己、拖动物体的坐标系、拖动的步骤
    private RectTransform ShadowRect, SelfRect, dragRect;
    private CookingStep dragStep;
    [HideInInspector] public CanvasRenderer ShadowRender;
    private Transform menuHolder;
    private int oldStartTime = -1;
    private bool isFirst = true;
    private int startTime;
    public int maxTime = 0;
    GameController gameController;
    public int[] Order = new int[50];

    void Start() {
        SelfRect = GetComponentsInChildren<RectTransform>().Where(x => x.name == "StepContent").First();
        ShadowRect = GetComponentsInChildren<RectTransform>().Where(x => x.name == "ShadowModel").First();
        ShadowRender = GetComponentsInChildren<CanvasRenderer>().Where(x => x.name == "ShadowModel").First();
        menuHolder = GameObject.FindWithTag("MenuHolder").transform;
        gameController = GameController.GetInstance();
        minScale = SelfRect.sizeDelta.x / 30;
        for (int i = 0; i < 50; i++)
            Order[i] = 0;
    }

    private void ShowShadow() {
        if (ShadowRender.GetAlpha() < 0.5) ShadowRender.SetAlpha(1);
    }
    private void HideShadow() {
        if (ShadowRender.GetAlpha() > 0.5) ShadowRender.SetAlpha(0);
    }

    private Vector2 position2anchorposition(RectTransform x, RectTransform y) {
        Vector2 anchorposition;
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, x.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(y, screenP, null, out anchorposition);
        return anchorposition;
    }

    // Update is called once per frame
    void Update() {
        if (dragRect) {
            Rect SelfRectRect = new Rect(0, -50, SelfRect.sizeDelta.x + dragRect.sizeDelta.x, SelfRect.sizeDelta.y + 100);
            Vector2 DragPos = position2anchorposition(dragRect, SelfRect);
            // 边界矩形
            // Debug.Log(SelfRectRect + " " + dragRect.position + " " + (new Vector2(dragRect.position.x, dragRect.position.y) + dragRect.sizeDelta));
            if (SelfRectRect.Contains(DragPos) && SelfRectRect.Contains(DragPos + dragRect.sizeDelta))
            { // 在区域内
                startTime = (int)(DragPos.x / minScale);
                if (isFirst)
                {
                    oldStartTime = startTime;
                    isFirst = false;
                }
                float relativex = startTime * minScale;
                ShadowRect.anchoredPosition = new Vector2(relativex, 0); // 阴影和物体及格子对齐
                List<Rect> HoldSteps =
                    new List<RectTransform>(GetComponentsInChildren<RectTransform>())
                    .Where(x => x.name.StartsWith("StepModel"))
                    .Select(x => new Rect(x.anchoredPosition.x, 0, x.sizeDelta.x - 0.01f, 1))
                    .ToList();
                Rect ShadowRectRect = new Rect(ShadowRect.anchoredPosition.x, 0, ShadowRect.sizeDelta.x, 1);
                if (!HoldSteps.Exists((x) => x.Overlaps(ShadowRectRect)) && ShadowRectRect.min.x >= 0 && ShadowRectRect.max.x <= SelfRect.sizeDelta.x)
                    ShowShadow(); // 阴影没有和别的步骤交错且没有出边界
                else HideShadow();
            }
               else HideShadow();
        }
        else
        {
            isFirst = true;
        }
    }

    public override void DragEffectBegin(Dragable d) {
        dragRect = d.GetComponent<RectTransform>();
        dragStep = d.GetComponent<CookingStep>();
        ShadowRect.sizeDelta = new Vector2(minScale * dragStep.Time - minScale * 7 / 60, SelfRect.sizeDelta.y);
    }

    public override void DragEffectEndIn() {
        dragRect.position = ShadowRect.position;
        dragRect.sizeDelta = ShadowRect.sizeDelta;
        dragRect.transform.SetParent(SelfRect.transform);
        dragRect.SetAsFirstSibling();

        CookingStep deleteStep = dragRect.GetComponent<CookingStep>();
        for (int i = 0; i < menuHolder.childCount; i++) //被依赖的步骤可以拖了
        {
            var stepChild = menuHolder.GetChild(i);
            var step = stepChild.GetComponent<CookingStep>();
            if (step.DependNotSatisfied.Contains(deleteStep))
                step.DependNotSatisfied.Remove(deleteStep);
            if (step.DependNotSatisfied.Count == 0)
                step.canDrag = true;
        }
        AddOrder();
        dragStep.Belong = this; dragStep = null;  dragRect = null;
        HideShadow(); ShadowRect.SetAsFirstSibling();
    }

    public override void DragEffectEndOut() {
        dragStep = null;  dragRect = null;
        oldStartTime = -1; isFirst = true;
    }

    void FullOrder(int start,int len,int value)
    {
        for(int i = start; i < start + len; i++)
        {
            Order[i] = value;
        }
    }

    void CheckMax()
    {
        bool iszero = true;
        for(int i = maxTime - 1; i >= 0; i--)
            if (Order[i] == 1)
            {
                maxTime = i + 1;
                iszero = false;
                break;
            }
        if (iszero)
            maxTime = 0;
    }

    public void AddOrder()
    {
        FullOrder(startTime, dragStep.Time, 1);
        int finishedTime = startTime + dragStep.Time;
        maxTime = maxTime > finishedTime ? maxTime : finishedTime;
    }

    public void DeleteOrder()
    {
        FullOrder(oldStartTime, dragStep.Time, 0);
        CheckMax();
    }
}
