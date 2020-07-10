using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float mouseSence;
    public cameraControler CC;

    private GameObject settingsmousesencetext;
    private GameObject settingsmousesenceslider;



    void Start()
    {
        settingsmousesenceslider = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
        settingsmousesencetext = this.gameObject.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
    }

    public void apply()
    {
        mouseSence = float.Parse(settingsmousesencetext.GetComponent<Text>().text);
        Debug.Log($"In mouse: {mouseSence} and we turned it into {Mathf.Clamp(mouseSence, 0.0f, 10.0f)}");
        mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);
        CC.mouseSensitivity = mouseSence;
        settingsmousesencetext.GetComponent<Text>().text = mouseSence.ToString("F2");
        settingsmousesenceslider.GetComponent<Slider>().value = mouseSence;
    }

}
