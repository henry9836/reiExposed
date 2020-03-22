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

    void Start()
    {
        camMove = GameObject.Find("camParent");
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            pauseitems.Add(this.gameObject.transform.GetChild(i).gameObject);
           
        }
    }

    void Update()
    {
        Debug.Log(Time.timeScale);

        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            pause();
        }
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
            Cursor.lockState = CursorLockMode.None;
            camMove.GetComponent<cameraControler>().enabled = false;
            Debug.Log("paused");
            Time.timeScale = 0.0f;
        }
        else
        {
            for (int i = 0; i < pauseitems.Count; i++)
            {
                pauseitems[i].SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            camMove.GetComponent<cameraControler>().enabled = true;

            Debug.Log("unpaused");
            Time.timeScale = 1.0f;
        }


    }

    public void loadLVL1()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        paused = !paused;
        SceneManager.LoadScene(1);



    }

    public void menu()
    {
        Time.timeScale = 1.0f;
        paused = !paused;
        SceneManager.LoadScene(0);

    }
}
