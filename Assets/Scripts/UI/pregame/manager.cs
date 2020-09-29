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
    public List<GameObject> brightness;
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

            for (int i = 0; i < brightness.Count; i++)
            {
                brightness[i].gameObject.SetActive(true);
            }
        }


        yield return null;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        waitforvideo = false;
    }



}
