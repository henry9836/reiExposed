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

        AudioListener.volume = SaveSystemController.getFloatValue("volume");

        Debug.Log(tocencor);

        cencortoggle.GetComponent<Toggle>().isOn = tocencor;
    }

    public void toggleCencorship(Toggle change)
    {
        tocencor = change.isOn;
        SaveSystemController.updateValue("toCensor", tocencor);
    }
}
