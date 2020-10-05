using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class textManager : MonoBehaviour
{
    private GameObject manager;
    public Button here;
    public GameObject particles;
    public Transform hand;
    public Camera cam;
    //public AudioSource audio;
    

    private void Start()
    {
        manager = GameObject.Find("Canvas");

        if (this.gameObject.GetComponent<Text>().text != "")
        {
            UnityAction tmp = updatescore;
            here.onClick.AddListener(tmp);
            hand = GameObject.Find("justTheArm").transform;
            cam = Camera.main;
        }

    }

    void Update()
    {
        this.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, manager.GetComponent<creditsManager>().speed * Time.deltaTime, 0.0f);
    }

    public void updatescore()
    {
        Vector3 cursorOnCanvas = cam.ScreenToViewportPoint(Input.mousePosition);

        manager.GetComponent<creditsManager>().score += 1;
        manager.GetComponent<creditsManager>().scoreRef.GetComponent<Text>().text = "Score:" + manager.GetComponent<creditsManager>().score.ToString();

        if (manager.GetComponent<creditsManager>().score % 10 == 0)
        {
            StartCoroutine(scorepading());
        }

        GameObject tmp = GameObject.Instantiate(particles, Vector3.zero, Quaternion.identity);
        tmp.transform.parent = this.transform.parent;
        tmp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        tmp.transform.position = Vector3.zero;
        tmp.transform.localPosition = new Vector3((manager.GetComponent<RectTransform>().rect.width * (cursorOnCanvas.x - 0.52f)), manager.GetComponent<RectTransform>().rect.height * (cursorOnCanvas.y + 0.07f), this.transform.position.z);

        Destroy(this.gameObject);
    }



    public IEnumerator scorepading()
    {
        manager.GetComponent<creditsManager>().speed *= 1.5f;
        //audio.Play();
        Text textref = manager.GetComponent<creditsManager>().scoreRef.GetComponent<Text>();

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * 2.0f)
        {
            textref.fontSize = (int)Mathf.Lerp(74, 108, i);
            textref.color = Color.Lerp(Color.white, Color.red, i);

            yield return null;
        }
        textref.fontSize = 108;
        textref.color = Color.red;

        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime * 2.0f)
        {
            textref.fontSize = (int)Mathf.Lerp(74, 108, i);
            textref.color = Color.Lerp(Color.white, Color.red, i);

            yield return null;
        }
        textref.fontSize = 74;
        textref.color = Color.white;

        yield return null;
    }




}
