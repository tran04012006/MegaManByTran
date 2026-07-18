using UnityEngine;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    public ScrollRect scrollRect;

    public float scrollSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //bat dau tu vi tri dau tien
        scrollRect.verticalNormalizedPosition = 1f;
        // cai nay la vi tri cuon
        // 1f: bat dau tu ban dau, cuon xuong
        // 0f: bat dau tu phia duoi, cuon len
    }

    // Update is called once per frame
    void Update()
    {
        // cho text tu dong chay
        scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
        if (scrollRect.verticalNormalizedPosition <= 0)
        {
            //neu cuon het thi dung lai
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
