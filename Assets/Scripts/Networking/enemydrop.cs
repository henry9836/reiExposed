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

    private void Update()
    {
        if (test == true)
        {
            test = false;
            enemyiskil();
        }
    }

    public void enemyiskil() //no
    { 
        StartCoroutine(mess());
    }

    public IEnumerator mess()
    {
        UIpop.transform.GetChild(0).gameObject.GetComponent<Text>().text = censor.GetComponent<clientcencorship>().watchYourProfanity(packagetosend.enemieDrops[0].tmessage);
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

        yield return null;
    }

    public void dropondeath(Transform mythsTransform)
    {
        GameObject.Instantiate(dropmessage, mythsTransform);
    }
}
