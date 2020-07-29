using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class flash : MonoBehaviour
{
    private float timer;
    private float fadetimer;

    public float speed;
    public float fadespeed;

    private float rCol = 0.0f;
    private float gCol = 1.0f;
    private float bCol = 1.0f;
    private float aCol = 0.0f;


    public bool fadein;
    public bool fadeout;


    void Update()
    {
        if (fadein == true)
        {
            if (fadetimer < 1.0f)
            {
                fadetimer += Time.deltaTime * fadespeed * 2.0f;
            }
            else
            {
                fadetimer = 1.0f;
            }
            aCol = Mathf.Lerp(0.0f, 1.0f, fadetimer);
        }
        else if (fadeout == true)
        {
            if (fadetimer > 0.0f)
            {
                fadetimer -= Time.deltaTime * fadespeed;
            }
            else
            {
                fadetimer = 0.0f;
            }

            aCol = Mathf.Lerp(0.0f, 1.0f, fadetimer);
        }

        if (aCol > 0.0f)
        {
            timer += Time.deltaTime * speed;

            if (timer > 6.0f)
            {
                timer = 0.0f;
            }
            else if (timer > 5.0f)
            {
                gCol = Mathf.Lerp(0.0f, 1.0f, timer - 5.0f);
            }
            else if (timer > 4.0f)
            {
                rCol = Mathf.Lerp(0.0f, 1.0f, timer - 4.0f);
            }
            else if (timer > 3.0f)
            {
                bCol = Mathf.Lerp(0.0f, 1.0f, timer - 3.0f);
            }
            else if (timer > 2.0f)
            {
                gCol = Mathf.Lerp(1.0f, 0.0f, timer - 2.0f);
            }
            else if (timer > 1.0f)
            {
                rCol = Mathf.Lerp(0.0f, 1.0f, timer - 1.0f);
            }
            else
            {
                bCol = Mathf.Lerp(1.0f, 0.0f, timer);
            }

        }


        this.GetComponent<Image>().color = new Color(rCol, gCol, bCol, aCol);
    }
}
