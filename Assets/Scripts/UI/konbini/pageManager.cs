﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageManager : MonoBehaviour
{
    public GameObject shopPage;
    public GameObject storagePage;
    public GameObject umbrellaUpgrade;
    public GameObject extraPage;
    public GameObject selectedUI;
    public List<Vector2> positions = new List<Vector2>() { };
    private Vector2 canvaspos = new Vector2 (0.0f, 0.0f);
    private GameObject canvas;
    private enterToTalk ETT;

    public GameObject konobinicam;

    public camMove.locations current;

    public GameObject stlyeBG;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        ETT = GameObject.FindGameObjectWithTag("Shop").GetComponent<enterToTalk>();
        current = camMove.locations.ITEM;
    }
    public void StorgePage()
    {
        storagePage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<slot>().itemchange();
        shopPage.SetActive(false);
        storagePage.SetActive(true);
        umbrellaUpgrade.SetActive(false);
        extraPage.SetActive(false);

        selectedUI.GetComponent<RectTransform>().localPosition = positions[1] + canvaspos;
        if (current != camMove.locations.STORAGE)
        {
            konobinicam.GetComponent<camMove>().move(camMove.locations.STORAGE);
            current = camMove.locations.STORAGE;
        }
        stlyeBG.SetActive(true);

    }


    public void ShopPage()
    {
        shopPage.SetActive(true);
        storagePage.SetActive(false);
        umbrellaUpgrade.SetActive(false);
        extraPage.SetActive(false);

        selectedUI.GetComponent<RectTransform>().localPosition = positions[0] + canvaspos;
        if (current != camMove.locations.ITEM)
        {
            konobinicam.GetComponent<camMove>().move(camMove.locations.ITEM);
            current = camMove.locations.ITEM;
        }
        stlyeBG.SetActive(true);


    }

    public void umbrellaPage()
    {
        GameObject.Find("PLAYER_rei").GetComponent<upgradeUmbrella>().updateInteractable();

        shopPage.SetActive(false);
        storagePage.SetActive(false);
        umbrellaUpgrade.SetActive(true);
        extraPage.SetActive(false);

        selectedUI.GetComponent<RectTransform>().localPosition = positions[2] + canvaspos;
        if (current != camMove.locations.UMBRELLA)
        {
            konobinicam.GetComponent<camMove>().move(camMove.locations.UMBRELLA);
            current = camMove.locations.UMBRELLA;
        }
        stlyeBG.SetActive(false);

    }

    public void ExtraPage()
    {
        shopPage.SetActive(false);
        storagePage.SetActive(false);
        umbrellaUpgrade.SetActive(false);
        extraPage.SetActive(true);
        selectedUI.GetComponent<RectTransform>().localPosition = positions[3] + canvaspos;
        if (current != camMove.locations.EXTRA)
        {
            konobinicam.GetComponent<camMove>().move(camMove.locations.EXTRA);
            current = camMove.locations.EXTRA;
        }
        stlyeBG.SetActive(true);

    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ETT.bakcbutton();
        }
    }

    public void back()
    {
        ETT.bakcbutton();
    }
}
