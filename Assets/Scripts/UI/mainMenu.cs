using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{

    public bool creditsToggle = false;


    public GameObject cList;
    public GameObject MList;
    private Vector3 cListtop;
    private Vector3 cListmid;
    private Vector3 cListbot;
    private Vector3 MListtop;
    private Vector3 MListmid;
    private Vector3 MListbot;

    private Vector3 canvaspos;

    float menuspeed = 1.5f;

    public enum state
    { 
        credits,
        menu,
    }
    public state theState = state.menu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        canvaspos = new Vector3(this.gameObject.GetComponent<RectTransform>().anchoredPosition.x, this.gameObject.GetComponent<RectTransform>().anchoredPosition.y, 0.0f);
        cListtop = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        cListbot = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        MListtop = new Vector3(0.0f, this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
        MListbot = new Vector3(0.0f, -this.gameObject.GetComponent<RectTransform>().rect.height, 0.0f);
    }

    public void play()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;
    }

    public void credits()
    {
        if (creditsToggle == true)
        {
            creditsToggle = false;
            StartCoroutine(Down(state.credits, state.menu));
        }
        else 
        {
            creditsToggle = true;
            StartCoroutine(Down(state.menu, state.credits));
        }
    }

    public void quit()
    {
        Application.Quit();
    }


    public IEnumerator Down(state fromX, state toY)
    {

        if (fromX == state.menu  && toY == state.credits)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * menuspeed)
            {
                float iinterprate = Mathf.Sin((i * Mathf.PI) / 2);

                MList.transform.position = Vector3.Lerp(MListmid + canvaspos, MListbot + canvaspos, iinterprate);
                cList.transform.position = Vector3.Lerp(cListtop + canvaspos, cListmid + canvaspos, iinterprate);

                yield return null;
            }
            MList.transform.position = MListbot + canvaspos;
            cList.transform.position = cListmid + canvaspos;
        }

        if (fromX == state.credits && toY == state.menu)
        {
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * menuspeed)
            {
                float iinterprate = Mathf.Sin((i * Mathf.PI) / 2);

                MList.transform.position = Vector3.Lerp(MListtop + canvaspos, MListmid + canvaspos, iinterprate);
                cList.transform.position = Vector3.Lerp(cListmid + canvaspos, cListbot + canvaspos, iinterprate);

                yield return null;
            }
            MList.transform.position = MListmid + canvaspos;
            cList.transform.position = cListbot + canvaspos;
        }

        yield return null;
    }


}
