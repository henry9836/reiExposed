using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemydrop : MonoBehaviour
{
    public GameObject UIpop;
    public float movespeed;
    public bool test = false;
    public GameObject censor;
    public GameObject canvas;
    public GameObject dropmessage;

    public int messagesToShow = 0;

    [HideInInspector]
    public bool messageDisplayFlag = false;

    private clientcencorship clientCencorship;
    private Logger logger;

    public Settings tocencor;

    [Header("Items")]
    public List<Sprite> sprites = new List<Sprite>();
    public List<string> titles = new List<string>();

    Image itemOneImg;
    Image itemTwoImg;
    Image itemThreeImg;
    Text itemOneTitle;
    Text itemTwoTitle;
    Text itemThreeTitle;
    Text messageText;
    Text currencyText;

    private void Start()
    {
        clientCencorship = censor.GetComponent<clientcencorship>();
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        logger = canvas.transform.Find("MessageLogContainer").GetChild(0).GetComponent<Logger>();

        //Init Popup Message
        UIpop.SetActive(true);

        currencyText = UIpop.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        messageText = UIpop.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        itemOneImg = UIpop.transform.GetChild(0).GetChild(2).GetComponent<Image>();
        itemTwoImg = UIpop.transform.GetChild(0).GetChild(3).GetComponent<Image>();
        itemThreeImg = UIpop.transform.GetChild(0).GetChild(4).GetComponent<Image>();
        itemOneTitle = itemOneImg.transform.GetChild(0).GetComponent<Text>();
        itemTwoTitle = itemTwoImg.transform.GetChild(0).GetComponent<Text>();
        itemThreeTitle = itemThreeImg.transform.GetChild(0).GetComponent<Text>();

        //Populate items list
        for (int i = 0; i < GetComponent<Items>().images.Count; i++)
        {
            sprites.Add(GetComponent<Items>().images[i]);
        }

        //Make images invisible
        itemOneImg.gameObject.SetActive(false);
        itemTwoImg.gameObject.SetActive(false);
        itemThreeImg.gameObject.SetActive(false);

        UIpop.SetActive(false);

    }

    private void FixedUpdate() 
    {
        if (messagesToShow > 0) {
            if (!messageDisplayFlag)
            {
                if (clientCencorship.getMessageCount() > 0)
                {
                    StartCoroutine(mess());
                    messagesToShow--;
                }
            }
        }
    }

    public void manualMessage(string message, int curr, int item1, int item2, int item3, bool important)
    {
        datadump tmp = new datadump(2, important.ToString(), message, curr, item1, item2, item3);
        packagetosend.enemieDrops.Insert(0, tmp);
        clientcencorship.messages.Insert(0, message);
        messagesToShow++;
    }

    public void processMessage()
    {
        if (tocencor.tocencor == true)
        {
            Debug.Log("censord");
            StartCoroutine(clientCencorship.watchYourProfanity(packagetosend.enemieDrops[0].tmessage));
        }
        else
        {
            Debug.Log("notcensord");
            clientCencorship.dontWatchYourProfanity(packagetosend.enemieDrops[0].tmessage);
        }
    }

    public IEnumerator mess()
    {
        messageDisplayFlag = true;
        string msg = clientCencorship.getMessageAndRemove(0);
        bool tryattempt;
        bool.TryParse(packagetosend.enemieDrops[0].tID, out tryattempt);
        if (tryattempt)
        {
            logger.AddNewMessage(new Logger.LogContainer(msg, true)); // henry
        }
        else
        {
            logger.AddNewMessage(new Logger.LogContainer(msg)); // henry
        }

        //Replace this mess
        //UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        //UIpop.transform.GetChild(1).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tcurr.ToString();
        //UIpop.transform.GetChild(2).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem1.ToString();
        //UIpop.transform.GetChild(3).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem2.ToString();
        //UIpop.transform.GetChild(4).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem3.ToString();

        //Update and show popup
        UIpop.SetActive(true);
        messageText.text = msg;
        currencyText.text = packagetosend.enemieDrops[0].tcurr.ToString();

        //Make images visible if we have an item and assign title



        if ((packagetosend.enemieDrops[0].titem1 != 0))
        {
            itemOneImg.gameObject.SetActive(true);
            itemOneImg.sprite = sprites[packagetosend.enemieDrops[0].titem1];
            itemOneTitle.text = titles[packagetosend.enemieDrops[0].titem1];
        }
        if ((packagetosend.enemieDrops[0].titem2 != 0))
        {
            itemTwoImg.gameObject.SetActive(true);
            itemTwoImg.sprite = sprites[packagetosend.enemieDrops[0].titem2];
            itemTwoTitle.text = titles[packagetosend.enemieDrops[0].titem2];
        }
        if ((packagetosend.enemieDrops[0].titem3 != 0))
        {
            itemThreeImg.gameObject.SetActive(true);
            itemThreeImg.sprite = sprites[packagetosend.enemieDrops[0].titem3];
            itemThreeTitle.text = titles[packagetosend.enemieDrops[0].titem3];
        }

        //Save new infomation to save system
        SaveSystemController.updateValue("MythTraces", packagetosend.enemieDrops[0].tcurr + SaveSystemController.getIntValue("MythTraces")); 

        if (canvas.GetComponent<Items>().gaineditem((Items.AllItems)packagetosend.enemieDrops[0].titem1))
        {
            canvas.GetComponent<Items>().removeitembiginvin(canvas.GetComponent<Items>().biginvin.Count - 1, true);
        }
        canvas.GetComponent<Items>().gaineditem((Items.AllItems)packagetosend.enemieDrops[0].titem2);
        canvas.GetComponent<Items>().gaineditem((Items.AllItems)packagetosend.enemieDrops[0].titem3);

        packagetosend.enemieDrops.RemoveAt(0);

        UIpop.SetActive(true);

        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * movespeed)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3.0f);

        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * movespeed)
        {
            yield return null;
        }

        //Hide attachments
        itemOneImg.gameObject.SetActive(false);
        itemTwoImg.gameObject.SetActive(false);
        itemThreeImg.gameObject.SetActive(false);
        UIpop.SetActive(false);
        messageDisplayFlag = false;
        yield return null;
    }

    public void dropondeath(Transform mythsTransform)
    {
        GameObject.Instantiate(dropmessage, mythsTransform);
    }
}
