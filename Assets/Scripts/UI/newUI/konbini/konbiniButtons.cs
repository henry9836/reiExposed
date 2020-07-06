using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class konbiniButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool mouseover = false;
    public bool mousedown = false;
    private float downtimer = 0.0f;
    private float downtimer2 = 0.0f;
    private float liltimer = 0.0f;

    private int transferAmmount = 0;


    public enum classes
    { 
        tengu,
        kappa,
        oni,
    }

    public classes selected;
    public bool Y4plusN4minus;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
        mousedown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
        mousedown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseover = false;
        mousedown = false;
    }


    void Update()
    {
        if (mouseover == true)
        {
            if (mousedown == true)
            {
                downtimer += Time.deltaTime;
                downtimer2 += Time.deltaTime;

                if (downtimer < 6.0f)
                {
                    transferAmmount = 1;
                }
                else
                {
                    transferAmmount = 5;
                }

                if (downtimer <= 1.0f)
                {
                    if (downtimer2 >= liltimer)
                    {
                        liltimer += 0.4f;
                        call();
                    }
                }
                else if (downtimer <= 3.0f)
                {
                    if (downtimer2 >= liltimer)
                    {
                        liltimer += 0.1f;
                        call();
                    }
                }
                else
                {
                    call();
                }

                if (downtimer2 > 1.0f)
                {
                    downtimer2 = 0.0f;
                    liltimer = 0.0f;
                }

            }
            else
            {
                transferAmmount = 0;
                downtimer = 0.0f;
                downtimer2 = 0.0f;
                liltimer = 0.0f;
            }
        }
    }

    public void call()
    {
        if (Y4plusN4minus == true)
        {
            if (currency.Followers >= transferAmmount)
            {
                if (selected == classes.kappa)
                {
                    currency.Followers -= transferAmmount;
                    currency.kappa += transferAmmount;
                }
                else if (selected == classes.oni)
                {
                    currency.Followers -= transferAmmount;
                    currency.oni += transferAmmount;
                }
                else if (selected == classes.tengu)
                {
                    currency.Followers -= transferAmmount;
                    currency.tengu += transferAmmount;
                }
            }
            else
            {
                downtimer =3.0f;
            }
        }
        else
        {
            if (selected == classes.kappa)
            {
                if (currency.kappa >= transferAmmount)
                {
                    currency.kappa -= transferAmmount;
                    currency.Followers += transferAmmount;
                }
                else
                {
                    downtimer = 3.0f;
                }
            }
            else if (selected == classes.oni)
            {
                if (currency.oni >= transferAmmount)
                {
                    currency.oni -= transferAmmount;
                    currency.Followers += transferAmmount;
                }
                else
                {
                    downtimer = 3.0f;
                }
            }
            else if (selected == classes.tengu)
            {
                if (currency.tengu >= transferAmmount)
                {
                    currency.tengu -= transferAmmount;
                    currency.Followers += transferAmmount;
                }
                else
                {
                    downtimer = 3.0f;
                }
            }
        }
    }
}

