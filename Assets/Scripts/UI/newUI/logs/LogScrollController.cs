using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScrollController : MonoBehaviour
{

    public float scrollSpeed = 750.0f;
    public RectTransform msgAnchor;
    public RectTransform topMsg;
    public RectTransform bottomMsg;
    public RectTransform clampTop;
    public RectTransform clampBottom;

    Vector3 mouseScroll;

    public void updateInfo(RectTransform t, RectTransform b)
    {
        topMsg = t;
        bottomMsg = b;
    }

    // Update is called once per frame
    void Update()
    {
        if (msgAnchor.childCount > 0)
        {

            mouseScroll = new Vector3(Input.mouseScrollDelta.x, Input.mouseScrollDelta.y, 0.0f);

            //Clamp Out Of Bounds
            //Going up
            if (bottomMsg.position.y >= clampTop.position.y)
            {
                if (mouseScroll.y > 0.0f)
                {
                    mouseScroll = Vector3.zero;
                }
            }
            //Going down
            else if (topMsg.position.y <= clampBottom.position.y)
            {
                if (mouseScroll.y < 0.0f)
                {
                    mouseScroll = Vector3.zero;
                }
            }

            //Move Messages
            msgAnchor.localPosition += mouseScroll * scrollSpeed * Time.deltaTime;
        }
    }
}
