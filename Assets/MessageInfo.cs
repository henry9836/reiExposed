using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageInfo : MonoBehaviour
{
    public Logger.LogContainer log;
    public Text text;
    public Image pinned;

    public void setup()
    {
        pinned.enabled = log.important;
        text.text = log.msg;
    }

}
