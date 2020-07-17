using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float mouseSence = 1.0f;
    public cameraControler CC;

    public GameObject settingsmousesencetext;

    private GameObject settingsmousesenceslider;

    private bool once = false;


    void Start()
    {
        settingsmousesenceslider = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
    }

    public void apply()
    {
        if (once)
        {
            once = false;
            return;
        }
        else
        {
            once = true;
        }

        mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);



        float t1mouseSence;
        float t2mouseSence = settingsmousesenceslider.GetComponent<Slider>().value;


        if (float.TryParse(settingsmousesencetext.GetComponent<InputField>().text, out t1mouseSence)){}
        else
        {
            
            t1mouseSence = 0.0f;
        }

        float t3 = 10.0f * ((t2mouseSence / 10) * (t2mouseSence / 10)) / (1 + (1 - (t2mouseSence / 10)) * 1.6f);

        if (t3 != mouseSence)
        {
            t2mouseSence = 10.0f *((t2mouseSence / 10) * (t2mouseSence / 10)) / (1 + (1 - (t2mouseSence / 10) ) * 1.6f);

            mouseSence = t2mouseSence;
            mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);

            settingsmousesencetext.GetComponent<InputField>().text = mouseSence.ToString("F2");

        }
        else if (t1mouseSence != mouseSence)
        {

            mouseSence = t1mouseSence;
            mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);

            settingsmousesenceslider.GetComponent<Slider>().value = 10.0f * ((mouseSence / 10) * (mouseSence / 10)) / (1 + (1 - (mouseSence / 10)) * 1.6f);


            settingsmousesencetext.GetComponent<InputField>().text = mouseSence.ToString();


        }

        CC.mouseSensitivity = mouseSence;

        
    }
}
