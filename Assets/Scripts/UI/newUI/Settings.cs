using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject mousesencetext;
    public GameObject mousesenceslider;

    private bool once = false;

    void Start()
    {
        float sence = SaveSystemController.getFloatValue("mouseSensitivity");
        mousesencetext.GetComponent<InputField>().text = sence.ToString();
        mousesenceslider.GetComponent<Slider>().value = sence;


    }

}
