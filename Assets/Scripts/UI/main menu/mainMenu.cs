﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

using UnityEngine.Rendering.HighDefinition;


public class mainMenu : MonoBehaviour
{
    public GameObject MList;
    public GameObject SList;
    public GameObject LList;
    public GameObject Warning;
    public GameObject notification;

    private Vector3 Listtop;
    private Vector3 Listmid;
    private Vector3 Listbot;

    private Vector3 canvaspos;

    float menuspeed = 1.5f;

    bool loadedData = false;
    bool packageWaiting = false;
    public Volume post;

    //Audio
    public AudioClip MenuSelect;
    public AudioClip MenuBack;
    public AudioClip PressPlay;
    private AudioSource audio;


    public enum state
    { 
        menu,
        settings,
        leaderboard,
    }

    public state theState = state.menu;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        Debug.Log("My canvas height is: " + this.gameObject.GetComponent<RectTransform>().rect.height.ToString());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvaspos = new Vector3(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x, this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, 0.0f);
        Listtop = new Vector3(0.0f, Screen.height, 0.0f);
        Listmid = new Vector3(0.0f, 0.0f, 0.0f);
        Listbot = new Vector3(0.0f, -Screen.height, 0.0f);

        LiftGammaGain tmp;
        if (post.profile.TryGet(out tmp))
        {
            tmp.gamma.value = new Vector4(0.0f, 0.0f, 0.0f, SaveSystemController.getFloatValue("Gamma"));
        }
    }

    public void play()
    {
        StartCoroutine(playy());
    }

    public IEnumerator playy()
    {
        
        if (SaveSystemController.loadedValues && SaveSystemController.checkSaveValid())
        {
            audio.PlayOneShot(PressPlay);
            yield return new WaitForSeconds(1.0f);
            SceneToLoadPersistant.sceneToLoadInto = 3;
            SceneManager.LoadScene(1);
            Cursor.visible = false;
        }
        else if (SaveSystemController.loadedValues && !SaveSystemController.checkSaveValid())
        {
            Application.Quit();
        }
    }

    public void openBrightness()
    {
        audio.PlayOneShot(MenuSelect);
        SceneToLoadPersistant.sceneToLoadInto = 0;
        SceneManager.LoadScene(1);
        managerofPlay.playintro = false;
        managerofPlay.playGamma = true;
    }

    public void openCredits()
    {
        audio.PlayOneShot(MenuSelect);
        SceneToLoadPersistant.sceneToLoadInto = 4;
        SceneManager.LoadScene(1);
    }

    public void tomenu()
    {
        audio.PlayOneShot(MenuBack);
        buttonenable(state.menu, true);
        buttonenable(state.settings, false);
        buttonenable(state.leaderboard, false);


        StartCoroutine(Down(theState, state.menu));
        theState = state.menu;

    }
    public void tosettings()
    {
        audio.PlayOneShot(MenuSelect);
        buttonenable(state.menu, false);
        buttonenable(state.settings, true);
        buttonenable(state.leaderboard, false);


        StartCoroutine(Down(theState, state.settings));
        theState = state.settings;

    }

    public void toleaderboard()
    {
        audio.PlayOneShot(MenuSelect);
        buttonenable(state.menu, false);
        buttonenable(state.settings, false);
        buttonenable(state.leaderboard, true);


        StartCoroutine(Down(theState, state.leaderboard));
        theState = state.leaderboard;
        //if (SaveSystemController.loadedValues && SaveSystemController.checkSaveValid())
        //{
        //    buttonenable(state.menu, false);
        //    buttonenable(state.settings, false);
        //    buttonenable(state.leaderboard, true);


        //    StartCoroutine(Down(theState, state.leaderboard));
        //    theState = state.leaderboard;
        //}
        //else if (SaveSystemController.loadedValues && !SaveSystemController.checkSaveValid())
        //{
        //    Application.Quit();
        //}
    }

    public void closeWarning()
    {
        Warning.SetActive(false);
    }

    public void resetsave()
    {
        audio.PlayOneShot(MenuSelect);
        SaveSystemController.Reset();
        SceneToLoadPersistant.sceneToLoadInto = 0;
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        audio.PlayOneShot(MenuSelect);
        if (packageWaiting)
        {
            Warning.SetActive(true);
        }
        else
        {
            Application.Quit();
        }
    }

    public void forceQuit()
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

    private void FixedUpdate()
    {
        //Loads notification on leaderboard
        if (!loadedData)
        {
            if (SaveSystemController.readyForProcessing)
            {
                packageWaiting = (SaveSystemController.getBoolValue("PackagePending"));
                notification.SetActive(packageWaiting);
                loadedData = true;
            }
        }
    }

    public void updatemenupos(state moving, bool toonscreen, float completion)
    {
        switch (moving)
        {
            case state.menu:
                {
                    if (toonscreen)
                    {
                        MList.transform.position = Vector3.Lerp(Listtop + canvaspos, Listmid + canvaspos, completion);
                    }
                    else
                    {
                        Debug.Log("menbudown");
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
            case state.leaderboard:
                {
                    if (toonscreen)
                    {
                        LList.transform.position = Vector3.Lerp(Listtop + canvaspos, Listmid + canvaspos, completion);
                    }
                    else
                    {
                        LList.transform.position = Vector3.Lerp(Listmid + canvaspos, Listbot + canvaspos, completion);
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
            case state.leaderboard:
                {
                    if (enable)
                    {
                        LList.transform.GetChild(0).GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        LList.transform.GetChild(0).GetComponent<Button>().interactable = false;
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
