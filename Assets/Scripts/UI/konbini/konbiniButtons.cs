using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class konbiniButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool mouseover = false;
    private bool mousedown = false;
    private float downtimer = 0.0f;
    private float downtimer2 = 0.0f;
    private float liltimer = 0.4f;

    private Button myself;


    public void OnPointerDown(PointerEventData eventData)
    {
        mousedown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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

    void Start()
    {
        myself = this.gameObject.GetComponent<Button>();
    }


    void Update()
    {
        if (mouseover == true)
        {
            if (mousedown == true)
            {
                downtimer += Time.deltaTime;
                downtimer2 += Time.deltaTime;

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
                    if (downtimer2 >= liltimer)
                    {
                        liltimer += 0.05f;
                        call();
                    }
                }

                if (downtimer2 > 1.0f)
                {
                    downtimer2 = 0.0f;
                    liltimer = 0.0f;
                }

            }
            else
            {
                downtimer = 0.0f;
                downtimer2 = 0.0f;
                liltimer = 0.4f;
            }
        }
    }
    
    public void call()
    {
        if (myself.interactable == true)
        {
            myself.onClick.Invoke();
        }
        else
        {
            downtimer = 3.0f;
        }
    }
}

