using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRCodeController : MonoBehaviour
{

    public Logger logger;
    public enemydrop dropControl;
    public Text loreOne;
    public Text loreTwo;
    public Text loreThree;

    [Header("Online Mode")]
    public bool useOnlineDatabase = false;

    [Header("Message Parameters")]
    public string hint = "Important Hint";
    [Range(30, 300)]
    public int currency = 100;
    public Items.AllItems item1 = Items.AllItems.NONE;
    public Items.AllItems item2 = Items.AllItems.NONE;
    public Items.AllItems item3 = Items.AllItems.NONE;
    public bool hintImportant = true;
    public bool addOnToLore = true;
    public bool randomiseCurrency = false;
    public bool randomiseAttachments = false;
    public float timeTillDestory = 1.0f;
    public Material qrCodeMat;
    public enemydrop enemydropCtrl;

    private bool alreadyTriggered = false;
    private MeshRenderer meshRenderer;
    private float timer = 0.0f;

    private void Start()
    {
        //Create a new material to make each material have unique dissolvness
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(qrCodeMat);
        timer = 0.0f;

        if (randomiseCurrency)
        {
            currency = Random.Range(30, 300);
        }
        if (randomiseAttachments)
        {
            item1 = (Items.AllItems)(Random.Range((int)Items.AllItems.NONE, (int)Items.AllItems.MOVEBUFF_SMALL));
            item2 = (Items.AllItems)(Random.Range((int)Items.AllItems.NONE, (int)Items.AllItems.MOVEBUFF_SMALL));
            item3 = (Items.AllItems)(Random.Range((int)Items.AllItems.NONE, (int)Items.AllItems.MOVEBUFF_SMALL));
        }

        alreadyTriggered = SaveSystemController.getBoolValue("[QR]" + name);

        if (logger == null)
        {
            logger = GameObject.Find("MessageLog").GetComponent<Logger>();
        }
        if (enemydropCtrl == null)
        {
            enemydropCtrl = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<enemydrop>();
        }
        if (dropControl == null)
        {
            dropControl = GameObject.Find("Canvas").GetComponent<enemydrop>();
        }
        if (loreOne == null)
        {
            loreOne = GameObject.Find("txtClueLore1").GetComponent<Text>();
        }
        if (loreTwo == null)
        {
            loreTwo = GameObject.Find("txtClueLore2").GetComponent<Text>();
        }
        if (loreThree == null)
        {
            loreThree = GameObject.Find("txtClueLore3").GetComponent<Text>();
        }

    }

    private void Update()
    {
       if (alreadyTriggered)
        {
            meshRenderer.material.SetFloat("Vector1_9DEB93D9", timer);
            timer += Time.deltaTime;

            if (timer >= timeTillDestory)
            {
                Destroy(gameObject);
            }
        }
    }

    public void triggerTweet()
    {
        if (!alreadyTriggered)
        {
            //If it is an offline message or we did not connect to the database
            if (!useOnlineDatabase || packagetosend.enemieDrops.Count <= 0) {
                //Display hint
                dropControl.manualMessage(hint, currency, (int)item1, (int)item2, (int)item3, hintImportant);

                //Add hint to lore
                if (addOnToLore)
                {
                    if (loreOne.text == "")
                    {
                        loreOne.text = hint;
                    }
                    else if (loreTwo.text == "")
                    {
                        loreTwo.text = hint;
                    }
                    else if (loreThree.text == "")
                    {
                        loreThree.text = hint;
                    }
                }
            }
            //Else if we are an online message then
            else if (useOnlineDatabase)
            {
                enemydropCtrl.processMessage();
                enemydropCtrl.messagesToShow++;
            }

            //Activate VFX
            transform.GetChild(0).gameObject.SetActive(true);

            alreadyTriggered = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;
            triggerTweet();
            Destroy(gameObject);
        }
    }

}
