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
    public GameObject maincam;
    public GameObject camgrid;


    private GameObject[] myths;
    private bool sucess;

    public enum phonestates 
    {
        NONE,
        HOME,
        CAMERA,
        ROLL,
        AMAZON,
        CLUES,
    };
    public phonestates screen;

    void Start()
    {
        screen = phonestates.NONE;
        rei = GameObject.FindGameObjectWithTag("Player");
        canvas = this.gameObject;
        maincam = GameObject.Find("Main Camera");
        myths = GameObject.FindGameObjectsWithTag("Myth");

        //drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();
        //if (drone.candeliver == true)
        //{
        //    drone.deliver();
        //}
    }

    void Update()
    {
        switch (screen)
        {
            case phonestates.NONE:
                {

                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        openingephone(true);
                    }

                    break;
                }
            case phonestates.HOME:
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        openingephone(false);
                    }
                    break;
                }
            case phonestates.CAMERA:
                {

                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        takepicture();
                    }
                    else if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                    }

                    break;
                }
            case phonestates.ROLL:
                {

                    break;
                }
            case phonestates.AMAZON:
                {

                    break;
                }
            case phonestates.CLUES:
                {

                    break;
                }
            default:
                {
                    Debug.LogError("how the fuck did you get here lol");
                    break;
                }
        }
    }

    public void openingephone(bool open)
    {
        if (open == true)
        {
            ThePhoneUI.SetActive(true);
            rei.transform.GetChild(0).gameObject.SetActive(false);
            phonecam.SetActive(true);
            maincam.SetActive(false);
            rei.GetComponent<movementController>().enabled = false;
            rei.GetComponent<fistpersoncontroler>().enabled = true;
            rei.GetComponent<umbrella>().enabled = false;
            foreach (GameObject tmp in myths)
            {
                tmp.transform.GetChild(1).gameObject.SetActive(false);
            }
            rei.transform.GetChild(1).gameObject.SetActive(false);
            rei.GetComponent<Animator>().enabled = false;
            Time.timeScale = 0.0f;
            screen = phonestates.HOME;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            ThePhoneUI.SetActive(false);
            rei.transform.GetChild(0).gameObject.SetActive(true);
            phonecam.SetActive(false);
            maincam.SetActive(true);
            rei.GetComponent<movementController>().enabled = true;
            rei.GetComponent<fistpersoncontroler>().enabled = false;
            rei.GetComponent<umbrella>().enabled = true;
            foreach (GameObject tmp in myths)
            {
                tmp.transform.GetChild(1).gameObject.SetActive(true);
            }
            rei.transform.GetChild(1).gameObject.SetActive(true);
            rei.GetComponent<Animator>().enabled = true;
            Time.timeScale = 1.0f;
            screen = phonestates.NONE;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void clues()
    {
        screen = phonestates.CLUES;
    }

    public void cameraroll()
    {
        screen = phonestates.ROLL;

    }

    public void thecamera()
    {
        screen = phonestates.CAMERA;

        ThePhoneUI.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camgrid.SetActive(true);
    }

    public void amazon()
    {
        screen = phonestates.AMAZON;

    }

    public void BackToMenu()
    {
        screen = phonestates.HOME;

        Time.timeScale = 0.0f;
        ThePhoneUI.SetActive(true);
        camgrid.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void takepicture()
    {
        StartCoroutine(photo());
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
