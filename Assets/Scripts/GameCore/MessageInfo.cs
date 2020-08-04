using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageInfo : MonoBehaviour
{
    public Logger.LogContainer log;
    public Text text;
    public Image pinned;

    Image backdrop;

    public void setup()
    {
        pinned.enabled = log.important;
        text.text = log.msg;
        backdrop = GetComponent<Image>();
    }

    public void hideUI()
    {
        pinned.color = new Color(pinned.color.r, pinned.color.g, pinned.color.b, 0.0f);
        backdrop.color = new Color(backdrop.color.r, backdrop.color.g, backdrop.color.b, 0.0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
    }

    public void showUI()
    {
        pinned.color = new Color(pinned.color.r, pinned.color.g, pinned.color.b, 1.0f);
        backdrop.color = new Color(backdrop.color.r, backdrop.color.g, backdrop.color.b, 1.0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }

}
