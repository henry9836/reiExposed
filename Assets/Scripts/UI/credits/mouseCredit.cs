using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class mouseCredit : MonoBehaviour
{
    public Canvas parentCanvas;
    public GameObject mouseCursor;
    public Camera cam;

    RectTransform rect;
    public float zPos = 0.0f;

    float xOff = 0.0f;
    float yOff = 0.0f;

    public void Start()
    {
        Cursor.visible = false;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        zPos = mouseCursor.transform.position.z;
    }


    public void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xOff -= 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xOff += 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            yOff += 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            yOff -= 0.01f;
        }

        //Vector2 mousePos = Input.mousePosition;

        //Vector3 result = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 6.0f));

        //Vector3 resultVP = cam.ScreenToViewportPoint(Input.mousePosition);
        //resultVP = new Vector3(resultVP.x - 0.5f, resultVP.y - 0.5f, resultVP.z);

        //// mouseCursor.transform.position = new Vector3(result.x, result.y, 5.0f);
        //mouseCursor.transform.position = new Vector3(result.x - (resultVP.x / 10.0f), result.y - (resultVP.y / 10.0f), 5.0f);


        Vector3 cursorOnCanvas = cam.ScreenToViewportPoint(Input.mousePosition);

        //tmp.transform.parent = null;
        mouseCursor.transform.localScale = Vector3.one * Screen.width * 2.0f;
        mouseCursor.transform.position = Vector3.zero;
        //mouseCursor.transform.localPosition = new Vector3((rect.rect.width * (cursorOnCanvas.x - 0.5f)), rect.rect.height * (cursorOnCanvas.y + 0.07f), zPos);
        mouseCursor.transform.localPosition = new Vector3((rect.rect.width * (cursorOnCanvas.x + xOff)), rect.rect.height * (cursorOnCanvas.y + yOff), zPos);
        mouseCursor.transform.localPosition = new Vector3((rect.rect.width * (cursorOnCanvas.x - 0.52f)), rect.rect.height * (cursorOnCanvas.y + 0.07f), zPos);

        if (Input.GetMouseButton(0))
        {
            Debug.Log($"MP: {Input.mousePosition} | GP: {mouseCursor.transform.position} | VP: {cam.ScreenToViewportPoint(Input.mousePosition)} ||| {xOff}:{yOff}");
            mouseCursor.transform.localPosition += new Vector3(0.0f, 0.0f, 1.0f) * 10.0f;
        }

    }



}
