using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data.SqlClient;

public class ThePhone : MonoBehaviour
{
    private plugindemo drone;
    public GameObject ThePhoneUI;
    public GameObject rei;
    public GameObject phonecam;
    public GameObject canvas;

    private bool sucess;

    void Start()
    {
        ThePhoneUI = GameObject.Find("phone");
        rei = GameObject.FindGameObjectWithTag("Player");
        canvas = this.gameObject;

        //drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();

        //if (drone.candeliver == true)
        //{
        //    drone.deliver();
        //}
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Tab) == true)
         {
            openingephone(true);
         }

        if (Input.GetKeyDown(KeyCode.L))
        {
            takepicture();
        }
    }

    public void openingephone(bool open)
    {
        if (open == true)
        {
            ThePhoneUI.SetActive(true);
            rei.transform.GetChild(0).gameObject.SetActive(false);
            phonecam.SetActive(true);
            Time.timeScale = 0.0f;
            //disbale mesh rendere
            //disable all other UI
        }
        else
        {
            ThePhoneUI.SetActive(false);
            rei.transform.GetChild(0).gameObject.SetActive(true);
            phonecam.SetActive(false);
            Time.timeScale = 1.0f;
            //enable mesh rendere
            //enable all other UI


        }
    }

    public void clues()
    { 
    
    }

    public void cameraroll()
    { 
    
    }

    public void thecamera()
    {
        ThePhoneUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void amazon()
    { 
    
    }

    public void BackToMenu()
    {
        Time.timeScale = 0.0f;
    }

    public void takepicture()
    {
        StartCoroutine(photo());
        Debug.Log(sucess);

    }

    public IEnumerator photo()
    {
        List<GameObject> reenable = new List<GameObject>() { };

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.activeSelf == true)
            {
                reenable.Add(this.transform.GetChild(i).gameObject);
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        string foldername = "shhhhhSecretFolder/";
        sucess = false; 

        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        yield return new WaitForSecondsRealtime(0.1f);

        for (int i = 0; i < 10; i++)
        {
            if (!FileExists(foldername + i.ToString() + ".png"))
            {
                sucess = true;
                ScreenCapture.CaptureScreenshot(foldername + i.ToString() + ".png");
                i = 10;
            }

            yield return null;
        }

        for (int i = 0; i < reenable.Count; i++)
        {
            reenable[i].SetActive(true);
        }

        yield return null;
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }
}
