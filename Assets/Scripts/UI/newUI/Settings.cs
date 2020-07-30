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

    public GameObject cencortoggle;

    public bool tocencor = false;

    void Start()
    {
        float sence = SaveSystemController.getFloatValue("mouseSensitivity");
        mousesencetext.GetComponent<InputField>().text = sence.ToString();
        mousesenceslider.GetComponent<Slider>().value = sence;
        cencortoggle.GetComponent<Toggle>().isOn = tocencor;
        float vol = SaveSystemController.getFloatValue("volume");
        audiotext.GetComponent<InputField>().text = vol.ToString();
        audioslider.GetComponent<Slider>().value = vol;
    }

    public void toggleCencorship(Toggle change)
    {
        tocencor = change.isOn;
        SaveSystemController.updateValue("toCensor", tocencor);
    }
}
