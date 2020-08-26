using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class introSequence : MonoBehaviour
{
    public GameObject introUI;
    public bool playintro = true;

    public GameObject tabandscrollhelp;
    private GameObject img;
    private GameObject txt;
    private GameObject gameMNGR;
    private string VA1 = "Urban legends subsist on mystery-  "; //2x space at end
    private string VA2 = "they live so long as they are obscure.  ";
    private string VA3 = "\nTo be documented is their sole,  ";
    private string VA4 = "absolute poison.  ";


    void Start()
    {
        gameMNGR = GameObject.Find("GameManager");
        if (playintro)
        {
            StartCoroutine(intro());

        }
    }

    public IEnumerator intro()
    {

        tabandscrollhelp.SetActive(false);
        img = introUI.transform.GetChild(0).gameObject;
        txt = introUI.transform.GetChild(4).gameObject;
        float volcalc = 0.25f;

        this.GetComponent<AudioSource>().volume = volcalc;
        txt.SetActive(true);
        img.SetActive(true);
        img.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        yield return new WaitForEndOfFrame();
        gameMNGR.GetComponent<GameManager>().stopPlayer(true);


        yield return new WaitForSeconds(1.5f);
        this.transform.GetChild(0).GetComponent<AudioSource>().Play();




        StartCoroutine(type(2.25f, VA1));
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(type(2.0f, VA2));
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(type(1.5f, VA3));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(type(1.75f, VA4));
        yield return new WaitForSeconds(3.0f);


        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            volcalc += (Time.deltaTime / 4.0f) * 3.0f;
            img.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);
            txt.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, i);

            this.GetComponent<AudioSource>().volume = volcalc;

            yield return null;
        }

        this.GetComponent<AudioSource>().volume = 1.0f;
        img.SetActive(false);
        img.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);


        txt.SetActive(false);
        tabandscrollhelp.SetActive(true);
        gameMNGR.GetComponent<GameManager>().stopPlayer(false);

        yield return null;

    }


    public IEnumerator type(float readingTime, string toappend)
    {
        float characters = toappend.Length;
        float timeperchar = readingTime / characters;
        float totypelog = 0.0f;
        int count = 0;

        for (float i = 0.0f; i < readingTime; i += Time.deltaTime)
        {
            totypelog += Time.deltaTime;
            while (totypelog > timeperchar)
            {
                totypelog -= timeperchar;
                txt.GetComponent<Text>().text += toappend[count].ToString();
                count++;
            }

            yield return null;
        }

        yield return null;
    }

}
