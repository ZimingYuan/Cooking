using UnityEngine;

public class TimeHolder: Place {

    readonly int unitWidth = 100;
    readonly int unitHeight = 100;

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

    void Start() {
        SelfRect = GetComponent<RectTransform>();
        Shadow = this.transform.GetChild(0).gameObject;
        ShadowRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        ShadowRender = Shadow.GetComponent<CanvasRenderer>();
        minx = SelfRect.position.x;
        maxx = SelfRect.position.x + SelfRect.rect.width;
        miny = SelfRect.position.y - 100;
        maxy = SelfRect.position.y + SelfRect.rect.height + 100;
    }

    // Update is called once per frame
    void Update() {
        if (DragRect) {
            if (DragRect.position.x > minx &&
                DragRect.position.y > miny &&
                DragRect.position.x + DragRect.rect.width < maxx &&
                DragRect.position.y + DragRect.rect.height < maxy) { // 在区域内
                Vector2 position = new Vector2(DragRect.position.x, SelfRect.position.y);
                ShadowRect.position = position; // 让阴影对准拖动物体
                if (ShadowRender.GetAlpha() < 0.5) {
                    ShadowRender.SetAlpha(1); // 让阴影显示出来
                }
            } else if (ShadowRender.GetAlpha() > 0.5) ShadowRender.SetAlpha(0); // 让阴影隐藏
        }
    }

    public override void DragEffectBegin(Dragable d) {
        DragRect = d.GetComponent<RectTransform>();
        //Shadow = Instantiate<GameObject>(ShadowModel,transform); // 生成阴影
        //ShadowRect = Shadow.GetComponent<RectTransform>();
        //Shadow.GetComponent<CanvasRenderer>().SetAlpha(0);
        dragStep = d.GetComponent<CookingStep>();
        ShadowRect.sizeDelta = new Vector2(unitWidth * dragStep.Time, unitHeight);
    }

    public override bool DragEffectEnd(Dragable d) {
        bool flag = false;
        if (DragRect.position.x > minx &&
            DragRect.position.y > miny &&
            DragRect.position.x + DragRect.rect.width < maxx &&
            DragRect.position.y + DragRect.rect.height < maxy) {
            // 生成固定在时间轴上的步骤
            //RectTransform rt = (Instantiate<GameObject>(StepModel)).GetComponent<RectTransform>();
            //rt.position = ShadowRect.position;
            //rt.transform.SetParent(transform.parent);
            DragRect.position = ShadowRect.position;
            DragRect.sizeDelta = ShadowRect.sizeDelta;
            DragRect.transform.parent = this.transform;
            DragRect.SetAsFirstSibling();
            flag = true;
        }
        ShadowRender.SetAlpha(0);
        ShadowRect.SetAsFirstSibling();
        //Destroy(Shadow);
        //Shadow = null; ShadowRect = null;
        //DragRect = null;
        return flag;
    }

    public override void DragAway() {

    }
}
