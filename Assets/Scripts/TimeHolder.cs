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
    private RectTransform ShadowRect, SelfRect, DragRect;
    private CookingStep dragStep;
    [HideInInspector] public CanvasRenderer ShadowRender;
    private Transform menuHolder;
    GameController gameController;

    // 自己的有效区域
    private Rect SelfRectRect;

    void Start() {
        SelfRect = GetComponentsInChildren<RectTransform>().Where(x => x.name == "GridContent").First();
        ShadowRect = GetComponentsInChildren<RectTransform>().Where(x => x.name == "ShadowModel").First();
        ShadowRender = GetComponentsInChildren<CanvasRenderer>().Where(x => x.name == "ShadowModel").First();
        menuHolder = GameObject.FindWithTag("MenuHolder").transform;
        gameController = GameController.GetInstance();
        minScale = SelfRect.sizeDelta.x / 30;
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
            // 边界矩形
            if (SelfRectRect.Contains(DragRect.position)
                && SelfRectRect.Contains(new Vector2(DragRect.position.x, DragRect.position.y) + DragRect.sizeDelta)) { // 在区域内
                float relativex = (int)((DragRect.position.x - SelfRect.position.x) / minScale) * minScale;
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
            } else HideShadow();
        }
    }

    public override void DragEffectBegin(Dragable d) {
        DragRect = d.GetComponent<RectTransform>();
        dragStep = d.GetComponent<CookingStep>();
        ShadowRect.sizeDelta = new Vector2(minScale * dragStep.Time - minScale * 7 / 60, SelfRect.sizeDelta.y);
    }

    public override void DragEffectEndIn() {
        DragRect.position = ShadowRect.position;
        DragRect.sizeDelta = ShadowRect.sizeDelta;
        DragRect.transform.SetParent(SelfRect.transform);
        DragRect.SetAsFirstSibling();

        CookingStep deleteStep = DragRect.GetComponent<CookingStep>();
        for (int i = 0; i < menuHolder.childCount; i++) //被依赖的步骤可以拖了
        {
            var stepChild = menuHolder.GetChild(i);
            var step = stepChild.GetComponent<CookingStep>();
            if (step.DependNotSatisfied.Contains(deleteStep))
                step.DependNotSatisfied.Remove(deleteStep);
            if (step.DependNotSatisfied.Count == 0)
                step.canDrag = true;
        }
        dragStep.Belong = this; dragStep = null;  DragRect = null;
        HideShadow(); ShadowRect.SetAsFirstSibling();
        gameController.stepCollection.CheckDepend();
    }

    public override void DragEffectEndOut() {
        dragStep = null;  DragRect = null;
    }
}
