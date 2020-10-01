using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCredit : MonoBehaviour
{
    public Canvas parentCanvas;
    public Image mouseCursor;

    public void Start()
    {
        Cursor.visible = false;
    }


    public void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out movePos);

        Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        mouseCursor.transform.position = mousePos;

        transform.position = mousePos;
    }
}
