using UnityEngine;
using UnityEngine.UI;

public class Ruler : MonoBehaviour {

    [SerializeField] private Text NumberModel = null;
    private float width;

    void Start() {
        width = GetComponent<RectTransform>().sizeDelta.x / 30f;
        for (int i = 0; i < 30; i++) { // 添加标尺上面的数字
            Text Number = Instantiate(NumberModel);
            Number.transform.SetParent(transform);
            RectTransform rt = Number.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * width + width / 2, 8);
            Number.text = (i + 1).ToString();
        }
    }

    void Update() {
        
    }
}
