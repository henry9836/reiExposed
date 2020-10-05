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
    public Transform hand;
    public Camera cam;


    

    private void Start()
    {
        manager = GameObject.Find("Canvas");
        UnityAction tmp = updatescore;
        here.onClick.AddListener(tmp);
        hand = GameObject.Find("justTheArm").transform;
        cam = Camera.main;
    }

    void Update()
    {
        this.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 50.0f * Time.deltaTime, 0.0f);
    }

    public void updatescore()
    {
        Vector3 cursorOnCanvas = cam.ScreenToViewportPoint(Input.mousePosition);

        manager.GetComponent<creditsManager>().score += 1;
        manager.GetComponent<creditsManager>().scoreRef.GetComponent<Text>().text = "Score:" + manager.GetComponent<creditsManager>().score.ToString();
        GameObject tmp = GameObject.Instantiate(particles, Vector3.zero, Quaternion.identity);
        tmp.transform.parent = this.transform.parent;
        tmp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        tmp.transform.position = Vector3.zero;
        tmp.transform.localPosition = new Vector3((manager.GetComponent<RectTransform>().rect.width * (cursorOnCanvas.x - 0.52f)), manager.GetComponent<RectTransform>().rect.height * (cursorOnCanvas.y + 0.07f), this.transform.position.z);

        Destroy(this.gameObject);
    }







}
