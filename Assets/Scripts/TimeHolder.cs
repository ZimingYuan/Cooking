using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeHolder: Place {

    readonly int unitWidth = 100;
    readonly int unitHeight = 100;
    readonly int minScale = 50;
    // 阴影和显示在时间轴的步骤的prefab
    [SerializeField] private GameObject ShadowModel, StepModel;
    // 阴影物体
    private GameObject Shadow;
    // 阴影、自己、拖动物体的坐标系、拖动的步骤
    private RectTransform ShadowRect, SelfRect, DragRect;
    private CookingStep dragStep;
    public CanvasRenderer ShadowRender;
    private Transform menuHolder;
    GameController gameController;

    // 自己的有效区域
    private Rect SelfRectRect;

    void Start() {
        SelfRect = GetComponent<RectTransform>();
        Shadow = this.transform.GetChild(0).gameObject;
        ShadowRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        ShadowRender = Shadow.GetComponent<CanvasRenderer>();
        menuHolder = GameObject.FindWithTag("MenuHolder").transform;
        gameController = GameController.GetInstance();
    }

    private void ShowShadow() {
        if (ShadowRender.GetAlpha() < 0.5) ShadowRender.SetAlpha(1);
    }
    private void HideShadow() {
        if (ShadowRender.GetAlpha() > 0.5) ShadowRender.SetAlpha(0);
    }

    // Update is called once per frame
    void Update() {
        if (DragRect) {
            SelfRectRect = new Rect(SelfRect.position.x, SelfRect.position.y - 100,
                SelfRect.sizeDelta.x + DragRect.sizeDelta.x, SelfRect.sizeDelta.y + 200);
            if (SelfRectRect.Contains(DragRect.position)
                && SelfRectRect.Contains(new Vector2(DragRect.position.x, DragRect.position.y) + DragRect.sizeDelta)) { // 在区域内
                Vector2 position = new Vector2((int)(DragRect.position.x) / minScale * minScale, SelfRect.position.y);
                ShadowRect.position = position; // 让阴影对准拖动物体
                List<Rect> HoldSteps =
                    new List<GameObject>(GameObject.FindGameObjectsWithTag("StepInTimeHolder"))
                    .Select(x => x.GetComponent<RectTransform>())
                    .Where(x => x.GetComponent<CookingStep>().Belong == this)
                    .Select(x => new Rect(x.anchoredPosition.x, 0, x.sizeDelta.x, 1))
                    .ToList();
                Rect ShadowRectRect = new Rect(ShadowRect.anchoredPosition.x, 0, ShadowRect.sizeDelta.x, 1);
                if (!HoldSteps.Exists((x) => x.Overlaps(ShadowRectRect)) && ShadowRectRect.min.x > 0 && ShadowRectRect.max.x < SelfRect.sizeDelta.x) ShowShadow();
                else HideShadow();
            } else HideShadow();
        }
    }

    public override void DragEffectBegin(Dragable d) {
        DragRect = d.GetComponent<RectTransform>();
        dragStep = d.GetComponent<CookingStep>();
        ShadowRect.sizeDelta = new Vector2(unitWidth * dragStep.Time, unitHeight);
    }

    public override void DragEffectEnd(Dragable d) {
        DragRect.position = ShadowRect.position;
        DragRect.sizeDelta = ShadowRect.sizeDelta;
        DragRect.transform.parent = this.transform;
        DragRect.SetAsFirstSibling();

        CookingStep deleteStep = d.GetComponent<CookingStep>();
        for (int i = 0; i < menuHolder.childCount; i++)
        {
            var stepChild = menuHolder.GetChild(i);
            var step = stepChild.GetComponent<CookingStep>();
            //step.LastRight = Mathf.Max(step.LastRight, DragRect.position.x);
            if (step.DependNotSatisfied.Contains(deleteStep))
                step.DependNotSatisfied.Remove(deleteStep);
            if (step.DependNotSatisfied.Count == 0)
                step.canDrag = true;
        }

        dragStep.Belong = this; dragStep = null;  DragRect = null;
        HideShadow(); ShadowRect.SetAsFirstSibling();
    }

    public override void DragAway() {

    }
}
