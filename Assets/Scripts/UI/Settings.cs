using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public GameObject mousesencetext;
    public GameObject mousesenceslider;

    public GameObject audiotext;
    public GameObject audioslider;

    public GameObject cencortoggle;
    public GameObject toShowTimertoggle;

    public bool tocencor = false;
    public bool toShowTimer = false;



    void Start()
    {
        float sence = SaveSystemController.getFloatValue("mouseSensitivity");
        mousesencetext.GetComponent<InputField>().text = sence.ToString();
        mousesenceslider.GetComponent<Slider>().value = sence;
        cencortoggle.GetComponent<Toggle>().isOn = tocencor;
        float vol = SaveSystemController.getFloatValue("volume");
        audiotext.GetComponent<InputField>().text = vol.ToString();
        audioslider.GetComponent<Slider>().value = vol;

        bool tmp = SaveSystemController.getBoolValue("toShowTimer");
        if (SceneManager.GetActiveScene().name != "mainMenu")
        {
            toShowTimer = tmp;
            toShowTimertoggle.GetComponent<Toggle>().isOn = tmp;
        }

    }

    public void toggleCencorship(Toggle change)
    {
        tocencor = change.isOn;
        SaveSystemController.updateValue("toCensor", tocencor);
    }

    public void toggleTimer(Toggle change)
    {
        toShowTimer = change.isOn;
        SaveSystemController.updateValue("toShowTimer", toShowTimer);
    }
}
