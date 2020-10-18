using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enterToTalk : MonoBehaviour
{

    public GameObject UIelement;
    public bool standing = false;

    public GameObject konbiniUI;
    public GameObject rei;
    public GameObject biginvinstorage;
    public GameObject konobiniCam;

    public GameObject BGmusic;
    public GameObject BGkonobini;

    private bool toggle = true;
    private GameObject gameMNGR;

    private void Start()
    {
        gameMNGR = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player") && (toggle == true))
        {
            toggle = false;
            StartCoroutine(doormat());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            toggle = true;
        }
    }

    public void bakcbutton()
    {
        StartCoroutine(Leave());
    }

    //do stuff when enter / leave
    public void ShopNowOpen(bool isOpen)
    {
        if (isOpen == true)
        {
            rei.GetComponent<PlayerController>().health = rei.GetComponent<PlayerController>().maxHealth;

            BGkonobini.GetComponent<AudioSource>().Play();
            BGmusic.GetComponent<MusicPlayer>().StartNone();
            BGmusic.GetComponent<MusicPlayer>().Pause();

            konbiniUI.transform.root.GetChild(3).gameObject.SetActive(false);
            GetComponent<AudioSource>().Play();
            konbiniUI.SetActive(true);
            biginvinstorage.transform.root.GetChild(8).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            biginvinstorage.transform.root.GetComponent<ThePhone>().constantUI.SetActive(false);
            rei.GetComponent<upgradeUmbrella>().updateInteractable();
            biginvinstorage.GetComponent<slot>().itemchange();
            rei.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
            konobiniCam.SetActive(true);
            konbiniUI.transform.GetChild(6).GetComponent<pageManager>().ShopPage();
        }
        else
        {
            BGkonobini.GetComponent<AudioSource>().Pause();
            BGmusic.GetComponent<MusicPlayer>().StartCalm();
            BGmusic.GetComponent<MusicPlayer>().Resume();

            konbiniUI.transform.root.GetChild(3).gameObject.SetActive(true);

            SaveSystemController.saveDataToDisk();
            konbiniUI.SetActive(false);
            rei.GetComponent<CharacterController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            biginvinstorage.transform.root.GetComponent<ThePhone>().constantUI.SetActive(true);
            rei.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            konobiniCam.SetActive(false);
        }


    }

    public IEnumerator doormat()
    {
        UIelement.SetActive(true);
        gameMNGR.GetComponent<GameManager>().stopPlayer(true);

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            UIelement.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);

            yield return null;
        }

        ShopNowOpen(true);

        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            UIelement.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);

            yield return null;
        }

        UIelement.SetActive(false);


        yield return null;
    }

    public IEnumerator Leave()
    {
        UIelement.SetActive(true);

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
        {
            UIelement.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);

            yield return null;
        }

        ShopNowOpen(false);

        for (float i = 1.0f; i > 0.0f; i -= Time.deltaTime)
        {
            UIelement.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, i);

            yield return null;
        }

        UIelement.SetActive(false);
        gameMNGR.GetComponent<GameManager>().stopPlayer(false);


        yield return null;
    }

   
}
