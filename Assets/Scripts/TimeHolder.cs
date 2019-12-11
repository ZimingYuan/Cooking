﻿using UnityEngine;

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
    private CanvasRenderer ShadowRender;
    // 自己的有效区域
    private float minx, maxx, miny, maxy;
    private Transform menuHolder;
    GameController gameController;

    void Start() {
        SelfRect = GetComponent<RectTransform>();
        Shadow = this.transform.GetChild(0).gameObject;
        ShadowRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        ShadowRender = Shadow.GetComponent<CanvasRenderer>();
        minx = SelfRect.position.x;
        maxx = SelfRect.position.x + SelfRect.rect.width;
        miny = SelfRect.position.y - 100;
        maxy = SelfRect.position.y + SelfRect.rect.height + 100;
        menuHolder = GameObject.FindWithTag("MenuHolder").transform;
        gameController = GameController.GetInstance();
    }

    // Update is called once per frame
    void Update() {
        if (DragRect) {
            if (DragRect.position.x > minx &&
                DragRect.position.y > miny &&
                DragRect.position.x + DragRect.rect.width < maxx &&
                DragRect.position.y + DragRect.rect.height < maxy)
            { // 在区域内
                
                Vector2 position = new Vector2(DragRect.position.x, SelfRect.position.y);
                ShadowRect.position = position; // 让阴影对准拖动物体
                if (ShadowRender.GetAlpha() < 0.5) {
                    ShadowRender.SetAlpha(1); // 让阴影显示出来
                }
            }
            else if (ShadowRender.GetAlpha() > 0.5)
                ShadowRender.SetAlpha(0); // 让阴影隐藏
        }
    }

    public override void DragEffectBegin(Dragable d) {
        DragRect = d.GetComponent<RectTransform>();
        dragStep = d.GetComponent<CookingStep>();
        ShadowRect.sizeDelta = new Vector2(unitWidth * dragStep.Time, unitHeight);
    }

    public override bool DragEffectEnd(Dragable d) {
        bool flag = false;
        //if (DragRect.position.x > minx &&
        //    DragRect.position.y > miny &&
        //    DragRect.position.x + DragRect.rect.width < maxx &&
        //    DragRect.position.y + DragRect.rect.height < maxy) {
        //DragRect.position = ShadowRect.position;
        //向左对齐
        if (ShadowRect.position.x < d.GetComponent<CookingStep>().LastRight)
        {
            DragRect.position = d.fromPosition;
        }
        else
        {
            DragRect.position = new Vector2(((int)(ShadowRect.position.x) / minScale) * minScale, ShadowRect.position.y);
            DragRect.sizeDelta = ShadowRect.sizeDelta;
            DragRect.transform.parent = this.transform;
            DragRect.SetAsFirstSibling();

            CookingStep deleteStep = d.GetComponent<CookingStep>();
            for (int i = 0; i < menuHolder.childCount; i++)
            {
                var stepChild = menuHolder.GetChild(i);
                var step = stepChild.GetComponent<CookingStep>();
                step.LastRight = Mathf.Max(step.LastRight, DragRect.position.x);
                step.DirectDepend.Remove(deleteStep);
                if (step.DirectDepend.Count == 0)
                    step.canDrag = true;
            }
            flag = true;
            //}
            ShadowRender.SetAlpha(0);
            ShadowRect.SetAsFirstSibling();
        }
            //Destroy(Shadow);
            //Shadow = null; ShadowRect = null;
            //DragRect = null;
            return flag;
        
    }

    public override void DragAway() {

    }
}
