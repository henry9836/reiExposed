using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
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
        if (once == true)
        {
            once = false;
        }
        else
        {
            once = true;
            float tmpSence = mouseSence;

            float t1mouseSence;
            float t2mouseSence = settingsmousesenceslider.GetComponent<Slider>().value;


            if (float.TryParse(settingsmousesencetext.GetComponent<InputField>().text, out t1mouseSence)){}
            else
            {
                t1mouseSence = 0.0f;
            }

            if (t2mouseSence != tmpSence)
            {
                t2mouseSence = 10.0f *((t2mouseSence / 10) * (t2mouseSence / 10)) / (1 + (1 - (t2mouseSence / 10) ) * 1.6f);

                mouseSence = t2mouseSence;
                mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);

                settingsmousesencetext.GetComponent<InputField>().text = mouseSence.ToString("F2");

            }
            else if (t1mouseSence != tmpSence)
            {

                mouseSence = t1mouseSence;
                mouseSence = Mathf.Clamp(mouseSence, 0.0f, 10.0f);

                settingsmousesencetext.GetComponent<InputField>().text = mouseSence.ToString();
                settingsmousesenceslider.GetComponent<Slider>().value = mouseSence;


            }

            CC.mouseSensitivity = mouseSence;

        }
    }
}
