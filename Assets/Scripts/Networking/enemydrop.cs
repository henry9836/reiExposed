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

    public GameObject dropmessage;

    public int messagesToShow = 0;

    [HideInInspector]
    public bool messageDisplayFlag = false;

    private clientcencorship clientCencorship;

    private void Start()
    {
        clientCencorship = censor.GetComponent<clientcencorship>();
    }

    private void Update()
    {
        if (test == true)
        {
            test = false;
            //enemyiskil();
        }
    }

    private void FixedUpdate() //no
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
        UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = clientCencorship.getMessageAndRemove(0);
        //cencor3ed
        //UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tmessage;

        UIpop.transform.GetChild(1).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].tcurr.ToString();
        UIpop.transform.GetChild(2).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem1.ToString();
        UIpop.transform.GetChild(3).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem2.ToString();
        UIpop.transform.GetChild(4).gameObject.GetComponent<Text>().text = packagetosend.enemieDrops[0].titem3.ToString();

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
