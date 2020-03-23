using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{

    public void play()
    {
        SceneManager.LoadScene(1);
        Cursor.visible = false;

    }

    public void options()
    { 
        //SceneManager.LoadScene()
    }

    public void credits()
    { 
    

    }

    public void quit()
    {

        Application.Quit();
    }

}
