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
        Debug.Log("ENTERED SCENE: " + SceneManager.GetActiveScene().buildIndex.ToString());
        SaveSystemController.loadDataFromDisk();
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            SaveSystemController.Reset();
        }
#endif

        //Ready to interface with and on the main menu
        //if (SaveSystemController.loadedValues && !checkedHash && (SceneManager.GetActiveScene().buildIndex == 0))
        //{
        //    //Check Hash
        //    if (!SaveSystemController.checkSaveValid())
        //    {
        //        //CHEATS!!!!
        //        Debug.LogError("CHEATER DETECTED!!!");
        //        SaveSystemController.Reset();
        //        StartCoroutine(delayKickOut());
        //    }
        //}
    }

    IEnumerator delayKickOut()
    {
        yield return new WaitForSeconds(5.0f);
        Application.Quit();
    }

}
