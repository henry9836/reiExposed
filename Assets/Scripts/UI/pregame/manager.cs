using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public static class managerofPlay
{
    public static bool playintro = true;
    public static bool playGamma = false;
}

public class manager : MonoBehaviour
{
    public List<Text> brightnessText;
    public List<GameObject> brightnessimage;

    public GameObject videoplayer;
    public bool waitforvideo = true;


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

        videoplayer.GetComponent<VideoPlayer>().loopPointReached += CheckOver;

        StartCoroutine(logic());


    }


    public IEnumerator logic()
    {
        if (managerofPlay.playintro == true)
        {
            videoplayer.SetActive(true);

            while (waitforvideo)
            {
                yield return new WaitForEndOfFrame();
            }

            
            yield return new WaitForSeconds(0.5f);
        }

       
        if (managerofPlay.playGamma == true)
        {
            videoplayer.SetActive(false);
            this.GetComponent<AudioSource>().Play();

            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
            {
                for (int j = 0; j < brightnessText.Count; j++)
                {
                    Color tmp = brightnessText[j].color;
                    brightnessText[j].color = new Color(tmp.r, tmp.g, tmp.b, i);
                }

                for (int j = 0; j < brightnessimage.Count; j++)
                {
                    Color tmp = brightnessimage[j].GetComponent<Image>().color;
                    brightnessimage[j].GetComponent<Image>().color = new Color(tmp.r, tmp.g, tmp.b, i);
                }

                yield return null;
            }

            for (int j = 0; j < brightnessText.Count; j++)
            {
                Color tmp = brightnessText[j].color;
                brightnessText[j].color = new Color(tmp.r, tmp.g, tmp.b, 1.0f);
            }

            for (int j = 0; j < brightnessimage.Count; j++)
            {
                Color tmp = brightnessimage[j].GetComponent<Image>().color;
                brightnessimage[j].GetComponent<Image>().color = new Color(tmp.r, tmp.g, tmp.b, 1.0f);
            }

        }


        yield return null;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        waitforvideo = false;
    }



}
