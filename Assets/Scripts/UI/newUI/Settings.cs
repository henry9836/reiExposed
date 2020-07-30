using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject mousesencetext;
    public GameObject mousesenceslider;

    public GameObject audiotext;
    public GameObject audioslider;


    void Start()
    {
        float sence = SaveSystemController.getFloatValue("mouseSensitivity");
        mousesencetext.GetComponent<InputField>().text = sence.ToString();
        mousesenceslider.GetComponent<Slider>().value = sence;

        AudioListener.volume = SaveSystemController.getFloatValue("volume");
    }

}
