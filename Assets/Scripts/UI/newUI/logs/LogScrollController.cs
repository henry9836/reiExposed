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

    Logger logger;
    Vector3 mouseScroll;
    bool showingUI = false;
    GameObject phoneUI;

    public void updateInfo(RectTransform t, RectTransform b)
    {
        topMsg = t;
        bottomMsg = b;
    }

    private void Start()
    {
        phoneUI = transform.root.GetComponent<ThePhone>().ThePhoneUI;
        logger = GetComponent<Logger>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!phoneUI.activeInHierarchy)
        {
            if (msgAnchor.childCount > 0)
            {

                mouseScroll = new Vector3(Input.mouseScrollDelta.x, Input.mouseScrollDelta.y, 0.0f);

                if (mouseScroll.y > 0 && !showingUI)
                {
                    showingUI = true;
                    logger.showMsgs();
                }
                else if (mouseScroll.y < 0 && showingUI && (topMsg.position.y <= clampBottom.position.y))
                {
                    showingUI = false;
                    logger.hideMsgs();
                    return;
                }

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
        else
        {
            logger.hideMsgs();
            logger.resetMsgs();
            showingUI = false;
        }
    }
}
