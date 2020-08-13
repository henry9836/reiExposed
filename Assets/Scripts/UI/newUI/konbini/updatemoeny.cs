using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updatemoeny : MonoBehaviour
{
    void Update()
    {
        this.GetComponent<Text>().text = SaveSystemController.getIntValue("MythTraces") + "¥";
    }
}
