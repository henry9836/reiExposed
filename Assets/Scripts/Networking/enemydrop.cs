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
    public saveFile save;
    public GameObject canvas;
    public GameObject dropmessage;

    public int messagesToShow = 0;

    [HideInInspector]
    public bool messageDisplayFlag = false;

    private clientcencorship clientCencorship;
    private Logger logger;

    private void Start()
    {
        clientCencorship = censor.GetComponent<clientcencorship>();
        save = GameObject.Find("Save&Dronemanage").GetComponent<saveFile>();
        canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        logger = canvas.transform.Find("MessageLog").GetComponent<Logger>();
    }

    private void Update()
    {
        if (test == true)
        {
            test = false;
        }
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

    public void processMessage()
    {
        StartCoroutine(clientCencorship.watchYourProfanity(packagetosend.enemieDrops[0].tmessage));
    }

    public IEnumerator mess()
    {
        messageDisplayFlag = true;
        string msg = clientCencorship.getMessageAndRemove(0);
        logger.AddNewMessage(new Logger.LogContainer(msg));
        UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        //cencor3ed
        //UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tmessage;

        UIpop.transform.GetChild(1).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tcurr.ToString();
        UIpop.transform.GetChild(2).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem1.ToString();
        UIpop.transform.GetChild(3).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem2.ToString();
        UIpop.transform.GetChild(4).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem3.ToString();

        currency.MythTraces = save.safeItem("MythTraces", saveFile.types.INT).toint;
        currency.MythTraces += packagetosend.enemieDrops[0].tcurr;
        save.saveitem("MythTraces", currency.MythTraces);

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
