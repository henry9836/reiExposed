﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Steamworks.Data;
using JetBrains.Annotations;

public class mainMenu : MonoBehaviour
{
    public GameObject cList;
    public GameObject MList;
    public GameObject SList;

    private Vector3 Listtop;
    private Vector3 Listmid;
    private Vector3 Listbot;

    private Vector3 canvaspos;

    float menuspeed = 1.5f;

    public enum state
    { 
        credits,
        menu,
        settings,
    }

    public state theState = state.menu;

    private void Awake()
    {
        SaveSystemController.loadDataFromDisk();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvaspos = new Vector3(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x, this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, 0.0f);
        Listtop = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        Listmid = new Vector3(0.0f, 0.0f, 0.0f);
        Listbot = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
    }

    public void play()
    {
        SceneToLoadPersistant.sceneToLoadInto = 2;
        SceneManager.LoadScene(1);
        Cursor.visible = false;
    }


    public void tocredits()
    {
        buttonenable(state.menu, false);
        buttonenable(state.credits, true);
        buttonenable(state.settings, false);

        StartCoroutine(Down(theState, state.credits));
        theState = state.credits;

    }
    public void tomenu()
    {
        buttonenable(state.menu, true);
        buttonenable(state.credits, false);
        buttonenable(state.settings, false);

        StartCoroutine(Down(theState, state.menu));
        theState = state.menu;

    }
    public void tosettings()
    {
        buttonenable(state.menu, false);
        buttonenable(state.credits, false);
        buttonenable(state.settings, true);

        StartCoroutine(Down(theState, state.settings));
        theState = state.settings;

    }
    public void quit()
    {
        Application.Quit();
    }
    public IEnumerator Down(state fromX, state toY)
    {
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * menuspeed)
        {
            float iinterprate = Mathf.Sin((i * Mathf.PI) / 2);
            updatemenupos(fromX, false, iinterprate);
            updatemenupos(toY, true, iinterprate);
            yield return null;
        }

        updatemenupos(fromX, false, 1.0f);
        updatemenupos(toY, true, 1.0f);


        yield return null;
    }

    public void updatemenupos(state moving, bool toonscreen, float completion)
    {
        switch (moving)
        {
            case state.credits:
                {
                    if (toonscreen)
                    {
                        cList.transform.position = Vector3.Lerp(Listtop + canvaspos, Listmid + canvaspos, completion);
                    }
                    else
                    {
                        cList.transform.position = Vector3.Lerp(Listmid + canvaspos, Listbot + canvaspos, completion);
                    }

                    break;
                }
            case state.menu:
                {
                    if (toonscreen)
                    {
                        MList.transform.position = Vector3.Lerp(Listtop + canvaspos, Listmid + canvaspos, completion);
                    }
                    else
                    {
                        MList.transform.position = Vector3.Lerp(Listmid + canvaspos, Listbot + canvaspos, completion);
                    }
                    break;
                }
            case state.settings:
                {
                    if (toonscreen)
                    {
                        SList.transform.position = Vector3.Lerp(Listtop + canvaspos, Listmid + canvaspos, completion);
                    }
                    else
                    {
                        SList.transform.position = Vector3.Lerp(Listmid + canvaspos, Listbot + canvaspos, completion);
                    }
                    break;
                }
            default:
                {
                    Debug.LogWarning("maine menu bad switch");
                    break;
                }
        }
    }

    public void buttonenable(state buttons, bool enable)
    {
        switch (buttons)
        {
            case state.credits:
                {
                    if (enable)
                    {
                        cList.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        cList.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    }
                    break;
                }
            case state.menu:
                {
                    if (enable)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            MList.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            MList.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = false;
                        }
                    }
                    break;
                }
            case state.settings:
                {
                    if (enable)
                    {
                        SList.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        SList.transform.GetChild(0).GetComponent<Button>().interactable = false;
                    }
                    break;
                }
            default:
                {
                    Debug.LogWarning("maine menu bad switch button");
                    break;
                }
        }
    }
}