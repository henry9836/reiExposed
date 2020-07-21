using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;
using System;


//public class photo
public class ThePhone : MonoBehaviour
{
    //refrances
    public saveFile save;
    private plugindemo drone;
    public GameObject ThePhoneUI;
    public GameObject rei;
    public GameObject phonecam;
    public GameObject canvas;
    public GameObject maincam;
    public GameObject camgrid;
    public Sprite emptyPhotoSpot;

    //henry
    public MythWorkerUnion myths;
    private Vector2 restorescale;
    private Vector3 restorePos;
    private int restoreID;

    //menu navigations
    public int selected;
    public int itemselected;

    //swap BG based on current screen
    public Sprite BGnormal;
    public Sprite BGkey;
    public Sprite BGamazon;
    public Sprite BGinventory;

    //keyapp
    List<bool> clueStates = new List<bool>() { };

    public GameObject constantUI;



    public enum phonestates 
    {
        NONE,
        HOME,
        CAMERA,
        AMAZON,
        INVENTORY,
        KEY,
    };

    public phonestates screen;

    void Start()
    {
        screen = phonestates.NONE;
        rei = GameObject.FindGameObjectWithTag("Player");
        canvas = this.gameObject;
        maincam = GameObject.Find("Main Camera");
        myths = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MythWorkerUnion>();
        save = GameObject.Find("Save&Dronemanage").GetComponent<saveFile>();
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
                    int prev = selected;

                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    if (scroll > 0.0f)
                    {
                        selected -= 1;

                        if (drone.candeliver == true)
                        {
                            selected = Mathf.Clamp(selected, 0, 3);
                        }
                        else
                        {
                            //grey out or somth8ihng
                            selected = Mathf.Clamp(selected, 0, 2);
                        }
                    }
                    else if (scroll < 0.0f)
                    {
                        selected += 1;

                        if (drone.candeliver == true)
                        {
                            selected = Mathf.Clamp(selected, 0, 3);
                        }
                        else
                        {
                            //grey out or somth8ihng
                            selected = Mathf.Clamp(selected, 0, 2);
                        }
                    }

                    if (prev != selected)
                    {
                        if (ThePhoneUI.transform.GetChild(2).GetChild(prev).GetComponent<slotno>().shriking != true)
                        {
                            ThePhoneUI.transform.GetChild(2).GetChild(prev).GetComponent<slotno>().growing = false;
                            ThePhoneUI.transform.GetChild(2).GetChild(prev).GetComponent<slotno>().shriking = true;
                            StartCoroutine(ThePhoneUI.transform.GetChild(2).GetChild(prev).GetComponent<slotno>().toungrow());
                        }

                        if (ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().growing != true)
                        {
                            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().growing = true;
                            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().shriking = false;
                            StartCoroutine(ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().togrow());
                        }
                    }


                    if (Input.GetMouseButtonDown(0))
                    {
                        switch (selected)
                        {
                            case (0):
                                {
                                    inventoryopen();
                                    break;
                                }
                            case (1):
                                {
                                    thecamera();
                                    break;
                                }
                            case (2):
                                {
                                    keyopen();
                                    break;
                                }
                            case (3):
                                {
                                    amazon();
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        openingephone(false);
                    }
                    break;
                }
            case phonestates.CAMERA:
                {
                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    float fov = phonecam.GetComponent<Camera>().fieldOfView;
                    if (scroll > 0.0f)
                    {
                        fov -= 1.0f;
                    }
                    else if (scroll < 0.0f)
                    {
                        fov += 1.0f;
                    }

                    fov = Mathf.Clamp(fov, 2f, 100.0f);
                    phonecam.GetComponent<Camera>().fieldOfView = fov;

                    if (Input.GetMouseButtonDown(0))
                    {
                        checkPhotoValid();
                    }
                    else if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))
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
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        BackToMenu();
                    }
                    break;
                }
            case phonestates.INVENTORY:
                {
                    int prev = itemselected;

                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    if (scroll > 0.0f)
                    {
                        itemselected -= 1;
                        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                        Debug.Log(itemselected);


                    }
                    else if (scroll < 0.0f)
                    {
                        itemselected += 1;
                        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                        Debug.Log(itemselected);

                    }
                    itemselected = Mathf.Clamp(itemselected, 0, 7);


                    if (prev != itemselected)
                    {
                        slotno oldgm = ThePhoneUI.transform.GetChild(5).GetChild(prev + 1).GetComponent<slotno>();
                        slotno newgm = ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>();

                        if (oldgm.shriking != true)
                        {
                            oldgm.growing = false;
                            oldgm.shriking = true;
                            StartCoroutine(oldgm.toungrow());
                        }
                        
                        if (newgm.growing != true)
                        {
                            newgm.growing = true;
                            newgm.shriking = false;
                            StartCoroutine(newgm.togrow());
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (canvas.GetComponent<Items>().equipped.Count > itemselected)
                        {
                            canvas.GetComponent<Items>().removeitemequipped(itemselected, true);
                            ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        BackToMenu();
                    }

                    break;
                }
            case phonestates.KEY:
                {

                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        BackToMenu();
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
            ThePhoneUI.transform.GetChild(2).gameObject.SetActive(true);
            ThePhoneUI.transform.GetChild(3).gameObject.SetActive(false);
            ThePhoneUI.transform.GetChild(4).gameObject.SetActive(false);
            ThePhoneUI.transform.GetChild(5).gameObject.SetActive(false);

            selected = 0;
            itemselected = 0;

            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().growing = true;
            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().shriking = false;
            StartCoroutine(ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().togrow());

            ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().growing = true;
            ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().shriking = false;
            StartCoroutine(ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().togrow());

            ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGnormal;

            screen = phonestates.HOME;
            //constantUI.SetActive(false);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().growing = false;
                ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().shriking = true;
                float smol = ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().smol;
                ThePhoneUI.transform.GetChild(2).GetChild(i).transform.localScale = new Vector3(smol, smol, smol);
            }

            //constantUI.SetActive(true);
            ThePhoneUI.SetActive(false);
            screen = phonestates.NONE;
        }
    }



    public void thecamera()
    {
        screen = phonestates.CAMERA;

        rei.transform.GetChild(0).gameObject.SetActive(false);
        phonecam.SetActive(true);
        maincam.SetActive(false);
        rei.GetComponent<movementController>().enabled = false;
        rei.GetComponent<fistpersoncontroler>().enabled = true;
        rei.GetComponent<umbrella>().enabled = false;
        rei.transform.GetChild(1).gameObject.SetActive(false);
        rei.GetComponent<Animator>().enabled = false;


        ThePhoneUI.SetActive(false);

        camgrid.SetActive(true);
    }

    public void amazon()
    {
        screen = phonestates.AMAZON;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(4).gameObject.SetActive(true);
        currency.MythTraces = save.safeItem("MythTraces", saveFile.types.INT).toint;

        if (currency.MythTraces < 100)
        {
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = true;
        }

        ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = currency.MythTraces + "¥";
        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGamazon;


    }

    public void inventoryopen()
    {
        screen = phonestates.INVENTORY;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(5).gameObject.SetActive(true);
        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGinventory;


    }

    public void BackToMenu()
    {
        screen = phonestates.HOME;

        ThePhoneUI.SetActive(true);
        camgrid.SetActive(false);

        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(true);
        ThePhoneUI.transform.GetChild(3).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(4).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(5).gameObject.SetActive(false);


        rei.transform.GetChild(0).gameObject.SetActive(true);
        phonecam.SetActive(false);
        maincam.SetActive(true);
        rei.GetComponent<movementController>().enabled = true;
        rei.GetComponent<fistpersoncontroler>().enabled = false;
        rei.GetComponent<umbrella>().enabled = true;
        rei.transform.GetChild(1).gameObject.SetActive(true);
        rei.GetComponent<Animator>().enabled = true;

        save.saveitem("MythTraces", currency.MythTraces);

        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGnormal;
        phonecam.GetComponent<Camera>().fieldOfView = 60.0f;


    }

    public void amazonshop(int item)
    {
        if (item == 0)
        {
            currency.MythTraces -= 100;
            save.saveitem("MythTraces", currency.MythTraces);

            drone.todrop = 0;
            drone.deliver();
           
        }

        if (currency.MythTraces < 100)
        {
            ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = true;
        }
        ThePhoneUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Yen: " + currency.MythTraces;

        BackToMenu();


    }

    //check all phots for clues
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



    public void checkPhotoValid()
    {
        string cluename = "bad";

        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
        List<GameObject> clue = new List<GameObject>() { };
        for (int i = 0; i < clues.Length; i++)
        {
            clue.Add(clues[i]);
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
                    cluename = clue[i].name;
                }
            }
        }

        if (cluename != "bad")
        {
            save.saveitem(cluename + " clue", "yes");
        }


        //ADD SFX here
    }

    public void keyopen()
    {
        screen = phonestates.KEY;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(3).gameObject.SetActive(true);

        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGkey;

        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
        List<GameObject> clue = new List<GameObject>() { };

        for (int i = 0; i < clues.Length; i++)
        {
            clue.Add(clues[i]);
        }

        for (int i = 0; i < clue.Count; i++)
        {
            string tmp = save.safeItem(clue[i].name + " clue", saveFile.types.STRING).tostring;
            if (tmp == "yes")
            {
                clueStates.Add(true);
            }
            else
            {
                clueStates.Add(false);
            }
        }


    }

}
