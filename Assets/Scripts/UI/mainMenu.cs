using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void play()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;

    }

    public void options()
    { 

    }

    public void credits()
    { 
    

    }

    public void quit()
    {

        Application.Quit();
    }

}
