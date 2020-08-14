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

    private bool alreadyTriggered = false;

    private void Start()
    {

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

        if (!logger)
        {
            logger = GameObject.Find("MessageLog").GetComponent<Logger>();
        }
        if (!dropControl)
        {
            dropControl = GameObject.Find("Canvas").GetComponent<enemydrop>();
        }
        if (!loreOne)
        {
            loreOne = GameObject.Find("txtClueLore1").GetComponent<Text>();
        }
        if (!loreTwo)
        {
            loreTwo = GameObject.Find("txtClueLore2").GetComponent<Text>();
        }
        if (!loreThree)
        {
            loreThree = GameObject.Find("txtClueLore3").GetComponent<Text>();
        }
    }

    public void triggerTweet()
    {
        if (!alreadyTriggered)
        {
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
