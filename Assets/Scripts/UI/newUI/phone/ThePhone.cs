using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;
using System;
using System.Data.SqlTypes;


//public class photo
public class ThePhone : MonoBehaviour
{
    //refrances
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
    public int amazonselected;
    private float shortcutTime = 0.35f;
    public float shortcutTimer = 0.0f;



    //swap BG based on current screen
    public Sprite BGnormal;
    public Sprite BGkey;
    public Sprite BGamazon;
    public Sprite BGinventory;

    public GameObject constantUI;
    public LayerMask ignoor;

    //camera
    public float sec1timer = 0.0f;
    public GameObject clueglow;
    public GameObject camflash;
    public bool camMode = false;
    private bool scanbossmode = false;
    public GameObject drawtestref;

    //public GameObject uitest;
    ClueController clueCtrl;

    public bool inbossroom;
    public GameObject boss;

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

    private Animator playerAnimator;
    void Start()
    {
        screen = phonestates.NONE;
        rei = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = rei.GetComponent<Animator>();
        canvas = this.gameObject;
        maincam = GameObject.Find("Main Camera");
        myths = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MythWorkerUnion>();
        drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();
        clueCtrl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ClueController>();       
    }
    void Update()
    {
        //do stuff based on what menu you are in
        switch (screen)
        {
            case phonestates.NONE: // when phone is not open
                {
                    if (!this.transform.GetChild(7).gameObject.activeSelf && !this.transform.GetChild(2).GetComponent<pauseMenu>().paused)
                    {
                        if (Input.GetKeyDown(KeyCode.Tab))
                        {
                            openingephone(true);
                        }
                    }
                    break;
                }
            case phonestates.HOME: // from main menu
                {
                    //double tab to open camera
                    if (shortcutTimer < shortcutTime)
                    {
                        if (Input.GetKeyDown(KeyCode.Tab))
                        {
                            thecamera();
                            break;
                        }
                    }
                    shortcutTimer += Time.deltaTime;

                    //scroling menu
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

                    //scroling UI selected
                    if (prev != selected)
                    {
                        slotno oldslot = ThePhoneUI.transform.GetChild(2).GetChild(prev).GetComponent<slotno>();
                        slotno newslot = ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>();

                        if (oldslot.shriking != true)
                        {
                            oldslot.growing = false;
                            oldslot.shriking = true;
                            StartCoroutine(oldslot.toungrow());
                        }

                        if (newslot.growing != true)
                        {
                            newslot.growing = true;
                            newslot.shriking = false;
                            StartCoroutine(newslot.togrow());
                        }
                    }

                    //click to use what thing is selected
                    if (Input.GetMouseButtonDown(0))
                    {
                        switch (selected) // based on what item is used
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

                    //tab fully exists the phone
                    if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause"))
                    {
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1)) // RMB goes back 1 or in this case closes it aswell
                    {
                        openingephone(false);
                    }
                    break;
                }
            case phonestates.CAMERA:
                {
                    //zoom
                    camMode = true;
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
                    camgrid.SetActive(true);

                    fov = Mathf.Clamp(fov, 2f, 100.0f);
                    phonecam.GetComponent<Camera>().fieldOfView = fov;

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        scanbossmode = !scanbossmode;
                    }

                    if (scanbossmode == false)
                    {

                        camgrid.transform.GetChild(0).gameObject.SetActive(true);
                        camgrid.transform.GetChild(1).gameObject.SetActive(false);
                        camgrid.transform.GetChild(2).GetComponent<Text>().text = "press \"Q\" to swap to Photogrammetry mode";
                        if (!inbossroom)
                        {
                            sec1timer += Time.deltaTime;
                            if (sec1timer > 0.1f)
                            {
                                sec1timer = 0.0f;
                                checkPhotoValid(false, "Clue");
                            }

                            //take photo
                            if (Input.GetMouseButtonDown(0))
                            {
                                checkPhotoValid(true, "Clue");
                            }
                            else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause")) // close phone
                            {
                                float test = rei.transform.GetChild(0).rotation.eulerAngles.y;
                                Quaternion facing = Quaternion.Euler(0, test, 0);

                                rei.transform.GetChild(0).rotation = Quaternion.Euler(0, rei.GetComponent<fistpersoncontroler>().yaw, 0);
                                rei.transform.GetChild(0).GetChild(0).rotation = Quaternion.identity;
                                camMode = false;
                                BackToMenu();

                                openingephone(false);
                            }
                            else if (Input.GetMouseButtonDown(1))//back to menu
                            {
                                float test = rei.transform.GetChild(0).rotation.eulerAngles.y;
                                Quaternion facing = Quaternion.Euler(0, test, 0);

                                rei.transform.GetChild(0).rotation = Quaternion.Euler(0, rei.GetComponent<fistpersoncontroler>().yaw, 0);
                                rei.transform.GetChild(0).GetChild(0).rotation = Quaternion.identity;
                                camMode = false;
                                BackToMenu();
                            }
                        }
                        else
                        {
                            clueglow.transform.GetChild(0).GetComponent<Text>().text = "press \"Q\" to swap to Photogrammetry mode";
                        }
                    }
                    else
                    {
                        camgrid.transform.GetChild(0).gameObject.SetActive(false);
                        camgrid.transform.GetChild(1).gameObject.SetActive(true);
                        camgrid.transform.GetChild(2).GetComponent<Text>().text = "press \"Q\" to swap to Camera mode";
                        clueglow.transform.GetChild(0).GetComponent<Text>().text = "";


                        if (Input.GetMouseButton(0))
                        {
                            camgrid.transform.GetChild(1).GetChild(1).transform.Rotate(Vector3.back * 100.0f * Time.deltaTime);
                            drawtestref.GetComponent<drawTest>().toScanBoss();
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause")) // close phone
                    {
                        float test = rei.transform.GetChild(0).rotation.eulerAngles.y;
                        Quaternion facing = Quaternion.Euler(0, test, 0);

                        rei.transform.GetChild(0).rotation = Quaternion.Euler(0, rei.GetComponent<fistpersoncontroler>().yaw, 0);
                        rei.transform.GetChild(0).GetChild(0).rotation = Quaternion.identity;
                        camMode = false;
                        BackToMenu();

                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))//back to menu
                    {
                        float test = rei.transform.GetChild(0).rotation.eulerAngles.y;
                        Quaternion facing = Quaternion.Euler(0, test, 0);

                        rei.transform.GetChild(0).rotation = Quaternion.Euler(0, rei.GetComponent<fistpersoncontroler>().yaw, 0);
                        rei.transform.GetChild(0).GetChild(0).rotation = Quaternion.identity;
                        camMode = false;
                        BackToMenu();
                    }

                    break;
                } 
            case phonestates.AMAZON:
                {
                    //scroll
                    int prev = amazonselected;

                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    if (scroll > 0.0f)
                    {
                        amazonselected -= 1;
                        //ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                    }
                    else if (scroll < 0.0f)
                    {
                        amazonselected += 1;
                        //ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                    }
                    amazonselected = Mathf.Clamp(amazonselected, 0, 1);

                    //perchance item
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!playerAnimator.GetBool("UsingItem"))
                        {
                            playerAnimator.SetTrigger("UseItem");
                            amazonshop(amazonselected);
                        }
                    }
                    
                    //feedback for scroolling
                    if (prev != amazonselected)
                    {
                        slotno oldgm = ThePhoneUI.transform.GetChild(4).GetChild(prev).GetComponent<slotno>();
                        slotno newgm = ThePhoneUI.transform.GetChild(4).GetChild(amazonselected).GetComponent<slotno>();

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
                    if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause")) //close phone
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1)) //back to menu
                    {
                        BackToMenu();
                    }
                    break;
                }
            case phonestates.INVENTORY:
                {
                    //scroll
                    int prev = itemselected;

                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    if (scroll > 0.0f)
                    {
                        itemselected -= 1;
                        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();


                    }
                    else if (scroll < 0.0f)
                    {
                        itemselected += 1;
                        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();

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

                    //use item
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (canvas.GetComponent<Items>().equipped.Count > itemselected)
                        {
                            if (!playerAnimator.GetBool("UsingItem"))
                            {
                                playerAnimator.SetTrigger("UseItem");
                                canvas.GetComponent<Items>().removeitemequipped(itemselected, true);
                                ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
                            }
                        }
                    }

                    //close
                    if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause"))
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1))//back
                    {
                        BackToMenu();
                    }

                    break;
                }
            case phonestates.KEY: // key page
                {
                    if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause")) // close
                    {
                        BackToMenu();
                        openingephone(false);
                    }
                    else if (Input.GetMouseButtonDown(1)) // back
                    {
                        BackToMenu();
                    }

                    break;
                }
            default:
                {
                    Debug.LogError("how did you get here lol");
                    break;
                }
        }
    }

    //openth phone
    public void openingephone(bool open)
    {
        //open
        if (open == true)
        {
            //set objects
            ThePhoneUI.SetActive(true);
            ThePhoneUI.transform.GetChild(2).gameObject.SetActive(true);
            ThePhoneUI.transform.GetChild(3).gameObject.SetActive(false);
            ThePhoneUI.transform.GetChild(4).gameObject.SetActive(false);
            ThePhoneUI.transform.GetChild(5).gameObject.SetActive(false);


            //sets seleted objects and adjusts UI
            selected = 0;
            itemselected = 0;
            amazonselected = 0;

            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().growing = true;
            ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().shriking = false;
            StartCoroutine(ThePhoneUI.transform.GetChild(2).GetChild(selected).GetComponent<slotno>().togrow());

            ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().growing = true;
            ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().shriking = false;
            StartCoroutine(ThePhoneUI.transform.GetChild(5).GetChild(itemselected + 1).GetComponent<slotno>().togrow());

            ThePhoneUI.transform.GetChild(4).GetChild(amazonselected).GetComponent<slotno>().growing = true;
            ThePhoneUI.transform.GetChild(4).GetChild(amazonselected).GetComponent<slotno>().shriking = false;
            StartCoroutine(ThePhoneUI.transform.GetChild(4).GetChild(amazonselected).GetComponent<slotno>().togrow());

            ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGnormal;

            shortcutTimer = 0.0f;

            //rei cant wak 
            rei.GetComponent<umbrella>().phoneLock = true;

            screen = phonestates.HOME;
            constantUI.SetActive(false);
        }
        else // close
        {
            //close eveythign UI
            for (int i = 0; i < 4; i++)
            {
                ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().growing = false;
                ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().shriking = true;
                float smol = ThePhoneUI.transform.GetChild(2).GetChild(i).GetComponent<slotno>().smol;
                ThePhoneUI.transform.GetChild(2).GetChild(i).transform.localScale = new Vector3(smol, smol, smol);
            }
            //rei can wak
            rei.GetComponent<umbrella>().phoneLock = false;


            constantUI.SetActive(true);
            ThePhoneUI.SetActive(false);
            screen = phonestates.NONE;

        }
    }


    //when the camera is opened
    public void thecamera()
    {
        screen = phonestates.CAMERA;
        
        //enables and disables stuff
        rei.transform.GetChild(0).gameObject.SetActive(false);
        phonecam.SetActive(true);
        maincam.SetActive(false);
        rei.GetComponent<movementController>().enabled = false;
        rei.GetComponent<umbrella>().enabled = false;
        rei.transform.GetChild(1).gameObject.SetActive(false);
        rei.GetComponent<Animator>().enabled = false;


        ThePhoneUI.SetActive(false);


        Debug.Log(rei.transform.GetChild(0).rotation.eulerAngles.y);
        float test = rei.transform.GetChild(0).rotation.eulerAngles.y;
        Quaternion facing = Quaternion.Euler(0, test, 0);

        rei.transform.rotation = facing;
        rei.GetComponent<fistpersoncontroler>().enabled = true;
        rei.GetComponent<fistpersoncontroler>().SetPitch(0);
        rei.GetComponent<fistpersoncontroler>().SetYaw(test);

    }

    //openiing amazon app
    public void amazon()
    {
        //set atacve and deactiove
        screen = phonestates.AMAZON;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(4).gameObject.SetActive(true);
        //currency.Yen = save.safeItem("MythTraces", saveFile.types.INT).toint;

        ThePhoneUI.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = SaveSystemController.getIntValue("MythTraces") + "¥";
        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGamazon;

    }

    //open invemntory
    public void inventoryopen()
    {
        //set stuff active and deactivce
        screen = phonestates.INVENTORY;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(5).gameObject.SetActive(true);
        ThePhoneUI.transform.GetChild(5).gameObject.GetComponent<eqitems>().itemchange();
        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGinventory;


    }

    //returns to the main menu from anywhere so needs to be robust
    public void BackToMenu()
    {
        //disbale and enable UI stuff
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

        //save.saveitem("MythTraces", currency.Yen);

        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGnormal;
        phonecam.GetComponent<Camera>().fieldOfView = 60.0f;

        clueglow.GetComponent<flash>().fadeout = true;
        clueglow.GetComponent<flash>().fadein = false;

        clueglow.transform.GetChild(0).GetComponent<Text>().text = "";

    }

    //buy item
    public void amazonshop(int item)
    {
        //mroe then enough to buy
        if (SaveSystemController.getIntValue("MythTraces") >= 100)
        {
            //buy HPpack
            if (item == 0)
            {

                //save.saveitem("MythTraces", currency.Yen);
                SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - 100);

                drone.todrop = 0;
                drone.deliver();
            }
            else if (item == 1) // buy uber drone
            {
                //save.saveitem("MythTraces", currency.Yen);
                SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - 100);

                drone.todrop = 999;
                drone.deliver();
            }

            //grey out or somthing
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            //grey out or somthing
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = true;
        }




        ThePhoneUI.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = SaveSystemController.getIntValue("MythTraces") + "¥";

        BackToMenu();


    }

    //checks if photo is valid
    public void checkPhotoValid(bool takingphoto, string checkTag)
    {
        int element = 0;
        string cluename = "bad";
        bool isQRCode = false;
        bool cluePicTaken = false;

        GameObject[] clues = GameObject.FindGameObjectsWithTag(checkTag);
        GameObject[] qr = GameObject.FindGameObjectsWithTag("QRCode");

        List<GameObject> clue = new List<GameObject>() { };
        List<List<Vector2>> cluepos = new List<List<Vector2>>() { };

        //grab all clues to manage them
        for (int i = 0; i < clues.Length; i++)
        {
            cluepos.Add(new List<Vector2>());
            clue.Add(clues[i]);
        }

        for (int i = 0; i < qr.Length; i++)
        {
            cluepos.Add(new List<Vector2>());
            clue.Add(qr[i]);
        }

        

        //find all clues
        for (int i = 0; i < clue.Count; i++)
        {
            Vector3 sumTotal = Vector3.zero;
            Vector3[] vertexMesh = { };

            vertexMesh = clue[i].GetComponent<MeshFilter>().mesh.vertices;
            var crc = clue[i].GetComponent<ClueReCentre>();
            Vector3 offset = crc == null ? Vector3.zero : crc.offset;

            for (int j = 0; j < vertexMesh.Length; j++)
            {
                Vector3 worldPos = clue[i].transform.TransformPoint(0.5f * (offset + vertexMesh[j]));
                sumTotal += worldPos;
                var viewportPos = phonecam.GetComponent<Camera>().WorldToViewportPoint(worldPos);

                if (testvertex(viewportPos, worldPos))
                {
                    cluepos[i].Add(viewportPos);
                }
            }
            Vector3 averageWorldPos = sumTotal / vertexMesh.Length;
            var averageViewportPos = phonecam.GetComponent<Camera>().WorldToViewportPoint(averageWorldPos);


            if (testvertex(averageViewportPos, averageWorldPos))
            {
                cluepos[i].Add(averageViewportPos);
            }



            //more the 2 vertexts visable
            if (cluepos[i].Count > 2)
            {
                cluename = clue[i].name;
                element = i;
                isQRCode = clue[i].tag == "QRCode";

                if (SaveSystemController.getValue(cluename + "[CLUE]") == "yes") //already taken
                {
                    clueglow.transform.GetChild(0).GetComponent<Text>().text = "clue already photgraphed";
                    clueglow.GetComponent<flash>().fadeout = true;
                    clueglow.GetComponent<flash>().fadein = false;
                    break;
                }
                else if (SaveSystemController.getBoolValue("[QR]" + cluename)) //already taken
                {
                    clueglow.transform.GetChild(0).GetComponent<Text>().text = "QR Code Already Scanned";
                    clueglow.GetComponent<flash>().fadeout = true;
                    clueglow.GetComponent<flash>().fadein = false;
                    break;
                }
                else
                {
                    //create loos square the:
                    Vector2 lxly = new Vector2(0.0f, 0.0f); //low low point
                    Vector2 lxby = new Vector2(0.0f, 1.0f); //low big
                    Vector2 bxly = new Vector2(1.0f, 0.0f); // big low
                    Vector2 bxby = new Vector2(1.0f, 1.0f); //big big

                    float lxlydis = 999.0f;
                    float lxbydis = 999.0f;
                    float bxlydis = 999.0f;
                    float bxbydis = 999.0f;

                    int lxlypos = -1;
                    int lxbypos = -1;
                    int bxlypos = -1;
                    int bxbypos = -1;

                    //finds the points on the vertext closest to the points

                    for (int k = 0; k < cluepos[i].Count; k++)
                    {
                        if (Vector2.Distance(cluepos[i][k], lxly) < lxlydis)
                        {
                            lxlydis = Vector2.Distance(cluepos[i][k], lxly);
                            lxlypos = k;
                        }
                        if (Vector2.Distance(cluepos[i][k], lxby) < lxbydis)
                        {
                            lxbydis = Vector2.Distance(cluepos[i][k], lxby);
                            lxbypos = k;
                        }
                        if (Vector2.Distance(cluepos[i][k], bxby) < bxbydis)
                        {
                            bxbydis = Vector2.Distance(cluepos[i][k], bxby);
                            bxbypos = k;
                        }
                        if (Vector2.Distance(cluepos[i][k], bxly) < bxlydis)
                        {
                            bxlydis = Vector2.Distance(cluepos[i][k], bxly);
                            bxlypos = k;
                        }
                    }

                    Vector2 a = cluepos[i][lxlypos];
                    Vector2 b = cluepos[i][lxbypos];
                    Vector2 c = cluepos[i][bxbypos];
                    Vector2 d = cluepos[i][bxlypos];


                    //Debug.DrawLine(a, b, Color.green, 10.0f);
                    //Debug.DrawLine(b, c, Color.green, 10.0f);
                    //Debug.DrawLine(c, d, Color.green, 10.0f);
                    //Debug.DrawLine(d, a, Color.green, 10.0f);

                    //Debug.DrawLine(a, lxly, Color.green, 10.0f);
                    //Debug.DrawLine(b, lxby, Color.green, 10.0f);
                    //Debug.DrawLine(c, bxby, Color.green, 10.0f);
                    //Debug.DrawLine(d, bxly, Color.green, 10.0f);

                    //calc the aera of the square
                    float objaera = Mathf.Abs((((a.x * b.y) - (a.y * b.x)) + ((b.x * c.y) - (b.y * c.x)) + ((c.x * d.y) - (c.y * d.x)) + ((d.x * a.y) - (d.y * a.x))) / 2.0f);
                    float screenaera = 1.0f;

                    //get the persent of the screen taken up
                    float persenttaken = (objaera / screenaera) * 800.0f;

                    //more then 2%
                    if (persenttaken > 2.0f)
                    {
                        if (isQRCode)
                        {
                            clueglow.transform.GetChild(0).GetComponent<Text>().text = "QRCode Visible";
                        }
                        else
                        {
                            clueglow.transform.GetChild(0).GetComponent<Text>().text = "Clue Visible";
                        }

                        Debug.Log("not already teakmn");
                        clueglow.GetComponent<flash>().fadeout = false;
                        clueglow.GetComponent<flash>().fadein = true;
                        break;
                    }
                    else
                    {
                        clueglow.transform.GetChild(0).GetComponent<Text>().text = "Object Partially Visible";

                        clueglow.GetComponent<flash>().fadeout = true;
                        clueglow.GetComponent<flash>().fadein = false;
                    }
                }
            }
            else
            {
                clueglow.transform.GetChild(0).GetComponent<Text>().text = "Clue Not Visible";

                clueglow.GetComponent<flash>().fadeout = true;
                clueglow.GetComponent<flash>().fadein = false;
            }
        }

        if (takingphoto == true)
        {
            if (cluename != "bad")
            {
                if (!isQRCode)
                {
                    SaveSystemController.updateValue(cluename + "[CLUE]", "yes");
                    clueCtrl.cluesCollected.Add(cluename);
                    SaveSystemController.saveDataToDisk();
                }
                else
                {
                    //Update Save Controller
                    if (!cluename.Contains("Myth"))
                    {
                        SaveSystemController.updateValue("QRCodeFound", true);
                        SaveSystemController.updateValue("[QR]" + cluename, true);
                        SaveSystemController.saveDataToDisk();
                    }
                    //Trigger stuff :)
                    clue[element].GetComponent<QRCodeController>().triggerTweet();
                    Debug.Log("Done.");
                }

                //good photo add to save and stuff

            }
            //any photo 
        }
    }

    //test if vertex is in viewpornt and can be seen unobstruted
    private bool testvertex(Vector3 viewportPos, Vector3 worldPos)
    {
        if (viewportPos.x > 0.0f && viewportPos.x < 1.0f)
        {
            if (viewportPos.y > 0.0f && viewportPos.y < 1.0f)
            {
                if (viewportPos.z > 0.0f)
                {
                    Vector3 fromPosition = phonecam.transform.position;
                    Vector3 toPosition = worldPos;
                    Vector3 direction = toPosition - fromPosition;
                    float dis = Vector3.Distance(fromPosition, toPosition);

                    RaycastHit hit;
                    //Debug.DrawRay(fromPosition, direction, Color.white, 5.0f);
                    if (Physics.Raycast(fromPosition, direction, out hit, dis, ignoor))
                    {
                        //Debug.Log(hit.collider.name);
                        //Debug.DrawLine(hit.point, hit.point + (Vector3.up * 999), Color.cyan, 10.0f);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        return false;

    }

    //if key app is opened
    public void keyopen()
    {
        //enable and disabe stuff
        screen = phonestates.KEY;
        ThePhoneUI.transform.GetChild(2).gameObject.SetActive(false);
        ThePhoneUI.transform.GetChild(3).gameObject.SetActive(true);

        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGkey;

        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
        List<GameObject> clue = new List<GameObject>() { };
        List<bool> clueStates = new List<bool>() { };


        for (int i = 0; i < clues.Length; i++)
        {
            clue.Add(clues[i]);
        }

        //set clues to true or flase based on if the phot has been taken of them

        for (int i = 0; i < clue.Count; i++)
        {
            //string tmp = save.safeItem(clue[i].name + "[CLUE]", saveFile.types.STRING).tostring;
            string tmp = SaveSystemController.getValue(clue[i].name + "[CLUE]");
            if (tmp == "yes")
            {
                //Debug.Log(clue[i].name + "[CLUE]" + "    yesy");

                clueStates.Add(true);
            }
            else
            {
                //Debug.Log(clue[i].name + "[CLUE]" + "    noy");

                clueStates.Add(false);
            }
        }

        int truecount = 0;

        //UI feedback for results of above
        Debug.Log(clueStates.Count);
        for (int i = 0; i < clueStates.Count; i++)
        {
            if (clueStates[i] == true)
            {
                truecount++;
                ThePhoneUI.transform.GetChild(3).GetChild(3).GetChild(0).GetChild(i).GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                ThePhoneUI.transform.GetChild(3).GetChild(3).GetChild(0).GetChild(i).GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            }
        }

        string insert = truecount.ToString() + "/3";
        ThePhoneUI.transform.GetChild(3).GetChild(4).GetChild(2).GetComponent<Text>().text = insert;
        ThePhoneUI.transform.GetChild(3).GetChild(4).GetChild(1).GetComponent<Image>().fillAmount = (float)truecount / 3.0f;

    }

    public void enterbossroom(bool enter)
    {
        inbossroom = enter;
    }
}
