using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePhone : MonoBehaviour
{
    private plugindemo drone;
    public GameObject thephone;
    public GameObject rei;
    public GameObject phonecam;



    void Start()
    {
        thephone = GameObject.Find("phone");
        rei = GameObject.FindGameObjectWithTag("Player");

        //drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();

        //if (drone.candeliver == true)
        //{
        //    //drone.deliver();

        //}
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Tab) == true)
         {
            openingephone(true);
         }
    }

    public void openingephone(bool open)
    {
        if (open == true)
        {
            thephone.SetActive(true);
            rei.transform.GetChild(0).gameObject.SetActive(false);
            phonecam.SetActive(true);
            Time.timeScale = 0.0f;
            //disbale and enable mesh rendere
        }
        else
        {
            thephone.SetActive(false);
            rei.transform.GetChild(0).gameObject.SetActive(true);
            phonecam.SetActive(false);
            Time.timeScale = 1.0f;
            //disbale and enable mesh rendere

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
        thephone.SetActive(false);
        Time.timeScale = 1.0f;

    }
    public void amazon()
    { 
    
    }

    public void BackToMenu()
    {
        Time.timeScale = 0.0f;
    }

}
