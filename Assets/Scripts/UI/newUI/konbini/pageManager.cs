using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageManager : MonoBehaviour
{
    public GameObject shopPage;
    public GameObject storagePage;
    public GameObject selectedUI;
    public List<Vector2> positions = new List<Vector2>() { };
    private Vector2 canvaspos = new Vector2 (0.0f, 0.0f);
    private GameObject canvas;
    private enterToTalk ETT;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        ETT = GameObject.FindGameObjectWithTag("Shop").GetComponent<enterToTalk>();
    }
    public void StorgePage()
    {
        storagePage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<slot>().itemchange();
        shopPage.SetActive(false);
        storagePage.SetActive(true);
        selectedUI.GetComponent<RectTransform>().localPosition = positions[1] + canvaspos;
    }


    public void ShopPage()
    {
        shopPage.SetActive(true);
        storagePage.SetActive(false);
        selectedUI.GetComponent<RectTransform>().localPosition = positions[0] + canvaspos;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ETT.ShopNowOpen(false);

        }
    }

    public void back()
    {
        ETT.ShopNowOpen(false);
    }
}
