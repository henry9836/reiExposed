using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class managerofPlay
{
    public static bool playintro = true;
    public static bool playGamma = false;
}

public class manager : MonoBehaviour
{
    public List<GameObject> brightness;
    public GameObject logo;
    public GameObject text;



    void Start()
    {
        if (managerofPlay.playGamma == false)
        {
            if (SaveSystemController.getBoolValue("notFirstPlay"))
            {
                managerofPlay.playGamma = false;
            }
            else
            {
                managerofPlay.playGamma = true;
            }
        }


        StartCoroutine(logic());


    }


    public IEnumerator logic()
    {
        if (managerofPlay.playintro == true)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
            {
                logo.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, i);
                yield return null;
            }
            logo.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(0.5f);

            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
            {
                text.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, i);
                yield return null;
            }
            text.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);




            yield return new WaitForSeconds(1.5f);

            for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
            {
                logo.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, i);
                text.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, i);
                yield return null;
            }
            logo.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            text.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            yield return new WaitForSeconds(1.0f);

        }

       
        if (managerofPlay.playGamma == true)
        {
            for (int i = 0; i < brightness.Count; i++)
            {
                brightness[i].gameObject.SetActive(true);
            }
        }


        yield return null;
    }

}
