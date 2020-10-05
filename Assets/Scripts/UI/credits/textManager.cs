using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class textManager : MonoBehaviour
{
    private GameObject manager;
    public Button here;
    public GameObject particles;
    private void Start()
    {
        manager = GameObject.Find("Canvas");
        UnityAction tmp = updatescore;
        here.onClick.AddListener(tmp);
    }

    void Update()
    {
        this.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 50.0f * Time.deltaTime, 0.0f);
    }

    public void updatescore()
    {
        manager.GetComponent<creditsManager>().score += 1;
        manager.GetComponent<creditsManager>().scoreRef.GetComponent<Text>().text = "Score:" + manager.GetComponent<creditsManager>().score.ToString();
        GameObject tmp = GameObject.Instantiate(particles, this.transform.position, Quaternion.identity);
        tmp.transform.parent = this.transform.parent;
        tmp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Destroy(this.gameObject);
    }



}
