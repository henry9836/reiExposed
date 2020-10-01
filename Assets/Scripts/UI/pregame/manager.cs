using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

//Keep track of game state
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
        Debug.Log($"P: {managerofPlay.playintro}, S: {SaveSystemController.getBoolValue("notFirstPlay")}");
        //If this is a launch
        if (managerofPlay.playintro)
        {
            //If this the first time we are playing?
            if (SaveSystemController.getBoolValue("notFirstPlay"))
            {
                //We have played before so set the gamma controls to hidden
                managerofPlay.playGamma = false;
            }
            else
            {
                //We haven't played before so set the gamma controls to visible
                managerofPlay.playGamma = true;
            }
        }
        //If we went here through settings
        else
        {
            managerofPlay.playGamma = true;
        }

        videoplayer.GetComponent<VideoPlayer>().loopPointReached += CheckOver;

        StartCoroutine(logic());
    }


    public IEnumerator logic()
    {

        //Play the video and wait till finished
        if (managerofPlay.playintro == true)
        {
            videoplayer.SetActive(true);

            while (waitforvideo)
            {
                yield return new WaitForEndOfFrame();
            }

            
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Video Ended");
        Debug.Log($"{managerofPlay.playGamma} || {managerofPlay.playintro}");

        //Show gamma settings
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
        else
        {
            SceneToLoadPersistant.sceneToLoadInto = 2;
            SceneManager.LoadScene(1);
        }


        yield return null;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        waitforvideo = false;
    }



}
