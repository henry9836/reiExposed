using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public bool paused = false;
    public bool settinged = false;

    private GameObject camMove;

    public List<GameObject> pauseitems = new List<GameObject>() { };
    public List<GameObject> settingsItem = new List<GameObject>() { };
    public List<GameObject> doNotPauseIfPresent = new List<GameObject>();

    private Vector3 canvaspos;
    public GameObject smoke1;
    private Vector3 smoketopoff;
    private Vector3 smoketop;
    private Vector3 smokeonscreen;
    private Vector3 smokehalfon;
    private Vector3 smokoebottom;

    public GameObject smoke2;
    public GameObject smoke3;
    public GameObject smoke4;

    private IEnumerator smokeblow;

    public Settings sett;

    //Sounds
    public AudioClip PausedSound;
    public AudioClip MenuSift;
    public AudioClip MenuSelect;
    private AudioSource audio;


    void Start()  
    {
        smokeonscreen = new Vector3(0.0f, 0.0f, 0.0f);
        smoketopoff = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height / 2.0f, 0.0f);
        smokoebottom = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        smokehalfon = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height / 2.0f, 0.0f);
        smoketop = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        canvaspos = new Vector3(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x, this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, 0.0f);
        camMove = GameObject.Find("camParent");

        audio = GetComponent<AudioSource>();

        GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<cameraControler>().mouseSensitivity = AdjusterInfo.calcSlider(SaveSystemController.getFloatValue("mouseSensitivity"));
        sett.tocencor = SaveSystemController.getBoolValue("toCensor");
        AudioListener.volume = AdjusterInfo.calcSlider(SaveSystemController.getFloatValue("volume")) / 10.0f;
    }

    //Restricts pausing of the game
    bool canPause()
    {
        for (int i = 0; i < doNotPauseIfPresent.Count; i++)
        {
            if (doNotPauseIfPresent[i].activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canPause())
            {
                pause();
            }
        }
#else
        if (Input.GetButtonDown("Pause"))
        {   
            if (canPause())
            {
                pause();
            }
        }
#endif
    }

    public void pause()
    {
        paused = !paused;

        if (paused == true)
        {
            audio.PlayOneShot(PausedSound);
            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(true);
            }

            Cursor.visible = true;

            Cursor.lockState = CursorLockMode.None;
            camMove.GetComponent<cameraControler>().enabled = false;
            Time.timeScale = 0.0f;

            smokeblow = smokin();
            StartCoroutine(smokeblow);

            //Audio

        }
        else
        {


            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(false);
            }

            for (int i = 0; i < settingsItem.Count; i++)
            {
                settingsItem[i].SetActive(false);
            }

            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;
            camMove.GetComponent<cameraControler>().enabled = true;

            Time.timeScale = 1.0f;

            StopCoroutine(smokeblow);
        }


    }

    public void loadLVL1()
    {
        SaveSystemController.saveDataToDisk();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        paused = !paused;
        SceneToLoadPersistant.sceneToLoadInto = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(1);

    }

    public void menu()
    {
        Cursor.visible = true;

        Time.timeScale = 1.0f;
        paused = !paused;
        SceneToLoadPersistant.sceneToLoadInto = 2;
        SceneManager.LoadScene(1);

    }

    public void settings()
    {
        if (settinged)
        {
            //settingsapply.GetComponent<Settings>().apply();
            settinged = false;
            SaveSystemController.saveDataToDisk();
            for (int i = 0; i < settingsItem.Count; i++)
            {
                settingsItem[i].SetActive(false);
            }
            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(true);
            }
        }
        else
        {
            settinged = true;

            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(false);
            }
            for (int i = 0; i < settingsItem.Count; i++)
            {
                settingsItem[i].SetActive(true);
            }
        }
    }


    public IEnumerator smokin()
    {

        for (float i = 0.0f; i < 1.0f; i += Time.unscaledDeltaTime * 0.35f)
        {
            smoke1.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(smokoebottom, smokehalfon, i);
            smoke2.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(smokehalfon, smokeonscreen, i);
            smoke3.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(smokeonscreen, smoketopoff, i);
            smoke4.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(smoketopoff, smoketop, i);

            yield return null;
        }

        StartCoroutine(smokin());
        yield return null;
    }
}
