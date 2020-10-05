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


    public void Start()
    {
        Cursor.visible = false;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }


    public void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        Vector3 result = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        mouseCursor.transform.position = new Vector3(result.x, result.y, 97.0f);


    }



}
