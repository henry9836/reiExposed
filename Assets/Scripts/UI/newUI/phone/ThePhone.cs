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


    //swap BG based on current screen
    public Sprite BGnormal;
    public Sprite BGkey;
    public Sprite BGamazon;
    public Sprite BGinventory;

    //keyapp
    List<bool> clueStates = new List<bool>() { };

    public GameObject constantUI;
    public LayerMask ignoor;

    //camera
    public float sec1timer = 0.0f;
    public GameObject clueglow;
    public GameObject camflash;

    //public GameObject uitest;


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
        drone = GameObject.Find("Save&Dronemanage").GetComponent<plugindemo>();

        //StartCoroutine(testmove());
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

                    sec1timer += Time.deltaTime;
                    if (sec1timer > 1.0f)
                    {
                        sec1timer = 0.0f;
                        checkPhotoValid(false);
                    }


                    if (Input.GetMouseButtonDown(0))
                    {
                        checkPhotoValid(true);
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


                    if (Input.GetMouseButtonDown(0))
                    {
                        amazonshop(amazonselected);
                    }
                    

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


            //rei.wak off
            rei.GetComponent<umbrella>().phoneLock = true;

            screen = phonestates.HOME;
            constantUI.SetActive(false);
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
            //rei.wak on
            rei.GetComponent<umbrella>().phoneLock = false;


            constantUI.SetActive(true);
            ThePhoneUI.SetActive(false);
            screen = phonestates.NONE;

        }
    }



    public void thecamera()
    {
        screen = phonestates.CAMERA;

        //phonecam.transform.localRotation = rei.transform.GetChild(0).localRotation * rei.transform.localRotation;
        //rei.transform.localRotation = rei.transform.GetChild(0).localRotation * rei.transform.localRotation;
        //rei.transform.localEulerAngles = rei.transform.GetChild(0).localEulerAngles;

        //rei.transform.Rotate(rei.transform.GetChild(0).localEulerAngles);

        //Debug.Log($"ASBFEW {rei.transform.GetChild(0).eulerAngles}");
        //rei.transform.rotation = rei.transform.GetChild(0).transform.rotation;
        //Debug.Log($"ASBFEW After: {rei.transform.rotation.eulerAngles}");

        rei.transform.LookAt(transform.position + rei.transform.GetChild(0).transform.forward);


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
        //currency.Yen = save.safeItem("MythTraces", saveFile.types.INT).toint;
        currency.Yen = SaveSystemController.getIntValue("MythTraces");

        ThePhoneUI.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = currency.Yen + "¥";
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

        //save.saveitem("MythTraces", currency.Yen);
        SaveSystemController.updateValue("MythTraces", currency.Yen);

        ThePhoneUI.transform.GetChild(0).GetComponent<Image>().sprite = BGnormal;
        phonecam.GetComponent<Camera>().fieldOfView = 60.0f;

        clueglow.GetComponent<flash>().fadeout = true;
        clueglow.GetComponent<flash>().fadein = false;
    }

    public void amazonshop(int item)
    {
        if (item == 0)
        {
            currency.Yen -= 100;
            //save.saveitem("MythTraces", currency.Yen);
            SaveSystemController.updateValue("MythTraces", currency.Yen);

            drone.todrop = 0;
            drone.deliver();
        }
        else if (item == 1)
        {
            currency.Yen -= 100;
            //save.saveitem("MythTraces", currency.Yen);
            SaveSystemController.updateValue("MythTraces", currency.Yen);

            drone.todrop = 999;
            drone.deliver();
        }

        if (currency.Yen < 100)
        {
            //grey out or somthing
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            //grey out or somthing
            //ThePhoneUI.transform.GetChild(4).GetChild(1).GetComponent<Button>().interactable = true;
        }
        ThePhoneUI.transform.GetChild(4).GetChild(3).GetComponent<Text>().text = currency.Yen + "¥";

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
            //string picof = save.safeItem(filename, saveFile.types.STRING).tostring;
            string picof = SaveSystemController.getValue(filename);

            for (int j = 0; j < clues.Length; j++)
            {
                if (clues[j].gameObject.name == picof)
                {
                    ThePhoneUI.transform.GetChild(4).GetChild(j).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                }
            }
        }
    }



    public void checkPhotoValid(bool takingphoto)
    {
        string cluename = "bad";

        GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
        List<GameObject> clue = new List<GameObject>() { };
        List<List<Vector2>> cluepos = new List<List<Vector2>>() { };

        for (int i = 0; i < clues.Length; i++)
        {
            cluepos.Add(new List<Vector2>());
            clue.Add(clues[i]);
        }

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


            if (cluepos[i].Count > 2)
            {
                Vector2 lxly = new Vector2(0.0f, 0.0f);
                Vector2 lxby = new Vector2(0.0f, 1.0f);
                Vector2 bxly = new Vector2(1.0f, 0.0f);
                Vector2 bxby = new Vector2(1.0f, 1.0f);

                float lxlydis = 999.0f;
                float lxbydis = 999.0f;
                float bxlydis = 999.0f;
                float bxbydis = 999.0f;

                int lxlypos = -1;
                int lxbypos = -1;
                int bxlypos = -1;
                int bxbypos = -1;

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
                
                float objaera = Mathf.Abs((((a.x * b.y) - (a.y * b.x)) + ((b.x * c.y) - (b.y * c.x)) + ((c.x * d.y) - (c.y * d.x)) + ((d.x * a.y) - (d.y * a.x))) / 2.0f);
                float screenaera = 1.0f;

                float persenttaken = (objaera / screenaera) * 800.0f;
                //Debug.Log(persenttaken + "% taken up");

                if (persenttaken > 2.0f)
                {
                    cluename = clue[i].name;
                    //if (save.safeItem(cluename + " clue", saveFile.types.STRING).tostring == "yes")
                    if (SaveSystemController.getValue(cluename + " clue") == "yes")
                    {
                        Debug.Log("already taken");
                        clueglow.GetComponent<flash>().fadeout = true;
                        clueglow.GetComponent<flash>().fadein = false;
                    }
                    else
                    {
                        Debug.Log("not already teakmn");
                        clueglow.GetComponent<flash>().fadeout = false;
                        clueglow.GetComponent<flash>().fadein = true;
                        break;
                    }
                }
                else
                {
                    clueglow.GetComponent<flash>().fadeout = true;
                    clueglow.GetComponent<flash>().fadein = false;
                }
            }
            else
            {
                clueglow.GetComponent<flash>().fadeout = true;
                clueglow.GetComponent<flash>().fadein = false;
            }
        }

        if (takingphoto == true)
        {
            if (cluename != "bad")
            {
                //save.saveitem(cluename + " clue", "yes");
                SaveSystemController.updateValue(cluename + " clue", "yes");
                SaveSystemController.saveDataToDisk();
                //good phot
            }
            else
            {
                //bad phot
            }

            //any photo
        }


    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

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
            //string tmp = save.safeItem(clue[i].name + " clue", saveFile.types.STRING).tostring;
            string tmp = SaveSystemController.getValue(clue[i].name + " clue");
            if (tmp == "yes")
            {
                //Debug.Log(clue[i].name + " clue" + "    yesy");

                clueStates.Add(true);
            }
            else
            {
                //Debug.Log(clue[i].name + " clue" + "    noy");

                clueStates.Add(false);
            }
        }


        for (int i = 0; i < clueStates.Count; i++)
        {
            if (clueStates[i] == true)
            {
                ThePhoneUI.transform.GetChild(3).GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
            else
            {
                ThePhoneUI.transform.GetChild(3).GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            }
        }

        clueStates = new List<bool>() { };


    }


    //public IEnumerator testmove()
    //{
    //    Vector3 oldpos = new Vector3(-15.0f, 0.0f, 0.0f);
    //    Vector3 newpos = new Vector3(0.0f, 0.0f, 0.0f);

    //    float speed = 1.0f;

    //    uitest.SetActive(true);
    //    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * speed)
    //    {
    //        uitest.transform.position = Vector3.Lerp(oldpos, newpos, i);
    //    }

    //    yield return new WaitForSeconds(2.0f);

    //    for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * speed)
    //    {
    //        uitest.transform.position = Vector3.Lerp(newpos, oldpos, i);
    //    }

    //    uitest.SetActive(false);

    //    yield return null;
    //}

}
