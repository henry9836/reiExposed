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
        Debug.Log($"{SaveSystemController.loadedValues} && {!checkedHash} && {(SceneManager.GetActiveScene().buildIndex == 0)}");

        //Ready to interface with and on the main menu
        if (SaveSystemController.loadedValues && !checkedHash && (SceneManager.GetActiveScene().buildIndex == 0))
        {
            Debug.Log($"{!SaveSystemController.checkSaveValid()} || {SaveSystemController.getBoolValue("notFirstPlay")}");
            //Check Hash
            if (!SaveSystemController.checkSaveValid())
            {
                //CHEATS!!!!
                Debug.LogError("CHEATER DETECTED!!!");
                SaveSystemController.Reset();
                StartCoroutine(delayKickOut());
            }
            //Hash valid load in
            else
            {
                //If we have played the game before
                if (SaveSystemController.getBoolValue("notFirstPlay"))
                {
                    //Debug.Log("Hash Passed Loading into the game...");
                    //SceneManager.LoadScene(2);
                }
            }
            //We have checked the hash we don't need to check it again
            checkedHash = true;
        }
    }

    IEnumerator delayKickOut()
    {
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }

}
