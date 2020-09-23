using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SaveSystemInvoker : MonoBehaviour
{

    bool checkedHash = false;

    private void Awake()
    {
        //Load this first for things to work
        SaveSystemController.loadDataFromDisk();
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            Debug.Log(SaveSystemController.checkSaveValid());
            if (!SaveSystemController.checkSaveValid() && SaveSystemController.loadedValues)
            {
                //CHEATS!!!!
                Debug.LogError("CHEATER DETECTED!!!");
                SaveSystemController.Reset();
                //SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("No Cheats Detected :)");
            }
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Debug.Log(SaveSystemController.calcCurrentHash());
        }
#endif

        //Ready to interface with and on the main menu
        if (SaveSystemController.loadedValues && !checkedHash && (SceneManager.GetActiveScene().buildIndex == 0))
        {
            //Check Hash
            if (!SaveSystemController.checkSaveValid())
            {
                //CHEATS!!!!
                //Debug.LogError("CHEATER DETECTED!!!");
                //SaveSystemController.Reset();
                //SceneManager.LoadScene(0);
            }
        }
    }
}
