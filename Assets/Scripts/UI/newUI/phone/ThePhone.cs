using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.ComponentModel;

//public class photo
public class ThePhone : MonoBehaviour
{
    private saveFile save;
    private plugindemo drone;
    public GameObject ThePhoneUI;
    public GameObject rei;
    public GameObject phonecam;
    public GameObject canvas;
    public GameObject maincam;
    public GameObject camgrid;
    public Sprite emptyPhotoSpot;

    private GameObject[] myths;

    private Vector2 restorescale;
    private Vector3 restorePos;
    private int restoreID;

    

    public enum phonestates 
    {
        NONE,
        HOME,
        CAMERA,
        ROLL,
        AMAZON,
        CLUES,
        PICZOOM,
    };
    public phonestates screen;

    void Start()
    {
        screen = phonestates.NONE;
        rei = GameObject.FindGameObjectWithTag("Player");
        canvas = this.gameObject;
        maincam = GameObject.Find("Main Camera");
        myths = GameObject.FindGameObjectsWithTag("Myth");
        StartCoroutine(LoadScreenShot(0));
        save = GameObject.Find("Save&Dronemanage").GetComponent<saveFile>();

        savephotoinit();
        drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();
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
                    if (Input.GetMouseButtonDown(0))
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
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                    }
                    break;
                }
            case phonestates.AMAZON:
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                    }
                    break;
                }
            case phonestates.CLUES:
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                    }
                    break;
                }
            case phonestates.PICZOOM:
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        picUnzoom();
                    }
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

        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(4).gameObject.SetActive(true);

        updateclues();
    }

    public void cameraroll()
    {

        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(3).gameObject.SetActive(true);

        loadPhotos();

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
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(5).gameObject.SetActive(true);
        currency.MythTraces = save.safeItem("MythTraces", saveFile.types.INT).toint;
        if (currency.MythTraces < 100)
        {
            ThePhoneUI.transform.GetChild(5).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            ThePhoneUI.transform.GetChild(5).GetChild(1).GetComponent<Button>().interactable = true;
        }
        ThePhoneUI.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "Mythtraces: " + currency.MythTraces;

    }

    public void BackToMenu()
    {
        screen = phonestates.HOME;

        Time.timeScale = 0.0f;
        ThePhoneUI.SetActive(true);
        camgrid.SetActive(false);

        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(true);
        ThePhoneUI.transform.GetChild(3).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(4).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(5).gameObject.SetActive(false);


        save.saveitem("MythTraces", currency.MythTraces);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void picZoom(int photo)
    {
        screen = phonestates.PICZOOM;
        restorePos = ThePhoneUI.transform.GetChild(3).GetChild(photo).transform.localPosition;
        restorescale = ThePhoneUI.transform.GetChild(3).GetChild(photo).transform.localScale;
        restoreID = photo; 
        ThePhoneUI.transform.GetChild(3).GetChild(photo).transform.localScale = new Vector2(5, 5);
        ThePhoneUI.transform.GetChild(3).GetChild(photo).transform.localPosition = new Vector3(0, 0, -3);
        ThePhoneUI.transform.GetChild(3).GetChild(photo).GetComponent<Button>().enabled = false;

        for (int i = 0; i < 10; i++)
        {
            if (i != photo)
            {
                ThePhoneUI.transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
            }
        }
        ThePhoneUI.transform.GetChild(3).GetChild(10).gameObject.SetActive(true);
    }

    public void picUnzoom()
    {
        screen = phonestates.ROLL;
        ThePhoneUI.transform.GetChild(3).GetChild(restoreID).transform.localScale = restorescale;
        ThePhoneUI.transform.GetChild(3).GetChild(restoreID).transform.localPosition = restorePos;
        ThePhoneUI.transform.GetChild(3).GetChild(restoreID).GetComponent<Button>().enabled = true;


        for (int i = 0; i < 10; i++)
        {
            ThePhoneUI.transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }

        loadPhotos();

        ThePhoneUI.transform.GetChild(3).GetChild(10).gameObject.SetActive(false);
    }


    public void amazonshop(int item)
    {
        if (item == 0)
        {
            currency.MythTraces -= 100;
            save.saveitem("MythTraces", currency.MythTraces);
            if (drone.candeliver == true)
            {
                drone.todrop = 0;
                drone.deliver();
            }
        }

        if (currency.MythTraces < 100)
        {
            ThePhoneUI.transform.GetChild(5).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            ThePhoneUI.transform.GetChild(5).GetChild(1).GetComponent<Button>().interactable = true;
        }
        ThePhoneUI.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = "Mythtraces: " + currency.MythTraces;

    }


    public void updateclues()
    {
        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");

        for (int i = 0; i < 3; i++)
        {
            ThePhoneUI.transform.GetChild(4).GetChild(i).GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);

        }

        for (int i = 0; i < 10; i++)
        {
            string filename = ("state " + (i).ToString() + ".png");
            string picof = save.safeItem(filename, saveFile.types.STRING).tostring;

            for (int j = 0; j < clues.Length; j++)
            {
                if (clues[j].gameObject.name == picof)
                {
                    ThePhoneUI.transform.GetChild(4).GetChild(j).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }

    public void takepicture()
    {
        StartCoroutine(photo());
    }

    public void deletePhoto()
    {
        int tmp = 10;
        for (int i = 0; i < 10; i++)
        {
            if (ThePhoneUI.transform.GetChild(3).GetChild(i).gameObject.activeSelf)
            {
                tmp = i;
            }
        }
        savePhotosData(tmp, "del");

        picUnzoom();
    }

    public void checkPhotoValid()
    {
        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");

        string nametoset = "bad";
        int imagecount = save.safeItem("imageCount", saveFile.types.INT).toint;

        List<GameObject> clue = new List<GameObject>() { };

        for (int i = 0; i < clues.Length; i++)
        {
            clue.Add(clues[i]);
        }

        List<string> saveddata = new List<string>() { };
        for (int i = 0; i < 10; i++)
        {
            string filename = ("state " + (i).ToString() + ".png");
            string picof = save.safeItem(filename, saveFile.types.STRING).tostring;
            saveddata.Add(picof);
        }

        float closes = 999.0f;

        for (int i = 0; i < clue.Count; i++)
        {
            //its in the camera frame
            if (clue[i].gameObject.GetComponent<Renderer>().isVisible)
            {
                //10 meters or closer
                float dis = Vector3.Distance(clue[i].transform.position, rei.transform.position);
                if (dis < 10.0f && dis < closes) 
                {
                    if (dis < closes)
                    {
                        closes = dis;
                    }
                    nametoset = clue[i].name;
                }
            }
        }

        savePhotosData(imagecount, nametoset); 

        //1 photo per clue
        //bool pass = true;
        //for (int j = 0; j < saveddata.Count; j++)
        //{
        //    if (saveddata[j] == clue[i].name)
        //    {
        //        pass = false;
        //    }
        //}
        //if (pass == true)
        //{
        //    Debug.Log("not alreayd photgraphed");
        //    nametoset = clue[i].name;

        //}


    }

    public IEnumerator photo()
    {

        checkPhotoValid();

        List<GameObject> reenable = new List<GameObject>() { };

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.activeSelf == true)
            {
                reenable.Add(this.transform.GetChild(i).gameObject);
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        string fn = Directory.GetCurrentDirectory();
        string foldername = fn + "\\" + "shhhhhSecretFolder";

        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        yield return new WaitForSecondsRealtime(0.1f);

        for (int i = 0; i < 10; i++)
        {
            if (!FileExists(foldername + "\\" + i.ToString() + ".png"))
            {
                ScreenCapture.CaptureScreenshot(foldername + "\\" + i.ToString() + ".png");
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

    public void loadPhotos()
    {
        int i = 0;


        for (; i < save.safeItem("imageCount", saveFile.types.INT).toint; i++)
        {
            StartCoroutine(LoadScreenShot(i));

        }

        for (; i < 10; i++)
        {
            ThePhoneUI.transform.GetChild(3).GetChild(i).GetComponent<Image>().sprite = emptyPhotoSpot;
            ThePhoneUI.transform.GetChild(3).GetChild(i).GetComponent<Button>().enabled = false;
        }
    }

    IEnumerator LoadScreenShot(int i)
    {
        string name = i.ToString() + ".png";
        string pathPrefix = @"file://";
        string foldername = "shhhhhSecretFolder";
        string filename2 = @name;

        string path = "";

#if UNITY_STANDALONE_LINUX
            path = Directory.GetCurrentDirectory() + "/" + foldername +"/";
#endif

#if UNITY_STANDALONE_WIN
        path = Directory.GetCurrentDirectory() + "\\" + foldername + "\\";
#endif

#if UNITY_EDITOR
        path = Directory.GetCurrentDirectory() + "\\" + foldername + "\\";
#endif

        string fullFilename = pathPrefix + path + filename2;

        WWW www = new WWW(fullFilename);
        Texture2D screenshot = new Texture2D(1920, 1080, TextureFormat.DXT1, false);
        www.LoadImageIntoTexture(screenshot);

        Image tmp = ThePhoneUI.transform.GetChild(3).GetChild(i).GetComponent<Image>();
        tmp.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        tmp.gameObject.GetComponent<Button>().enabled = true;

        yield return null;
    }

    public void savephotoinit()
    {
        int count = save.safeItem("imageCount", saveFile.types.INT).toint;
        if (count == -999999)
        {
            count = 0;
        }
        save.saveitem("imageCount", count);

        for (int i = 0; i < 10; i++)
        {
            string location = ("state " + i + ".png");
            string photodata = save.safeItem(location, saveFile.types.STRING).tostring;

            if (photodata == null)
            {
                photodata = "del";
            }

            save.saveitem("state " + i + ".png", photodata);
        }
    }

    public void savePhotosData(int i, string state)
    {
        int count = save.safeItem("imageCount", saveFile.types.INT).toint;

        string location = ("state " + i + ".png");
        string tmp = save.safeItem(location, saveFile.types.STRING).tostring;


        if (tmp == "del" || tmp == "")
        {
            if (state != "del")
            {
                if (i < 10)
                {
                    save.saveitem(location, state);
                    save.saveitem("imageCount", count + 1);
                }
            }
        }
        else
        {
            if (state == "del")
            {
                save.saveitem(location, state);

                if (i != (count))
                {
                    for (int j = 0; j < ((count) - i); j++)
                    {
                        //swap save file
                        string x = "state " + (i + 0 + j).ToString() + ".png";
                        string y = "state " + (i + 1 + j).ToString() + ".png";

                        string xdata = save.safeItem(x, saveFile.types.STRING).tostring;
                        string ydata = save.safeItem(y, saveFile.types.STRING).tostring;

                        save.saveitem(x, ydata);
                        save.saveitem(y, xdata);


                        //swap images
                        string foldername = Directory.GetCurrentDirectory() + "\\" + "shhhhhSecretFolder";
                        string pre = foldername + "\\" + (i + 1 + j).ToString() + ".png";              
                        string post = foldername + "\\" + (i + 0 + j ).ToString() + ".png";

                        if (j == 0)
                        {
                            File.Delete(post);
                        }

                        if (FileExists(pre))
                        {
                            File.Move(pre, post);
                        }
                    }
                }

                save.saveitem("imageCount", count - 1);
            }
        }
    }  
}
