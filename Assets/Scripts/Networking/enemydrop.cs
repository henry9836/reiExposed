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

    private void Start()
    {
        clientCencorship = censor.GetComponent<clientcencorship>();
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        logger = canvas.transform.Find("MessageLogContainer").GetChild(0).GetComponent<Logger>();
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

    public void manualMessage(string message, int curr, int item1, int item2, int item3)
    {
        datadump tmp = new datadump(2, "steamid", message, curr, item1, item2, item3);
        packagetosend.enemieDrops.Add(tmp);
        clientcencorship.messages.Add(tmp.tmessage);
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

        logger.AddNewMessage(new Logger.LogContainer(msg)); // henry
        UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        UIpop.transform.GetChild(1).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tcurr.ToString();
        UIpop.transform.GetChild(2).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem1.ToString();
        UIpop.transform.GetChild(3).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem2.ToString();
        UIpop.transform.GetChild(4).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem3.ToString();

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

        UIpop.SetActive(false);
        messageDisplayFlag = false;
        yield return null;
    }

    public void dropondeath(Transform mythsTransform)
    {
        GameObject.Instantiate(dropmessage, mythsTransform);
    }
}
