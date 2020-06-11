using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public bool paused = false;
    private GameObject camMove;

    public List<GameObject> pauseitems = new List<GameObject>() { };

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



    void Start()
    {
        smokeonscreen = new Vector3(0.0f, 0.0f, 0.0f);
        smoketopoff = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height / 2.0f, 0.0f);
        smokoebottom = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        smokehalfon = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height / 2.0f, 0.0f);
        smoketop = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);

        canvaspos = new Vector3(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x, this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, 0.0f);

        camMove = GameObject.Find("camParent");
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            pauseitems.Add(this.gameObject.transform.GetChild(i).gameObject);
           
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            pause();
        }
#else
        if (Input.GetButtonDown("Pause"))
        {
            pause();
        }
#endif
    }

    public void pause()
    {
        paused = !paused;

        if (paused == true)
        {


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
        }
        else
        {


            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(false);
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
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        paused = !paused;
        SceneManager.LoadScene(1);



    }

    public void menu()
    {
        Cursor.visible = true;

        Time.timeScale = 1.0f;
        paused = !paused;
        SceneToLoadPersistant.sceneToLoadInto = 0;
        SceneManager.LoadScene(1);

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
