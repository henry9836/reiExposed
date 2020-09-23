using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveSystemInvoker : MonoBehaviour
{
    private void Awake()
    {
        //Load this first for things to work
        SaveSystemController.loadDataFromDisk();
    }

#if UNITY_EDITOR
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            Debug.Log(SaveSystemController.checkSaveValid());
            if (!SaveSystemController.checkSaveValid() && SaveSystemController.loadedValues)
            {
                //CHEATS!!!!
                SaveSystemController.Reset();
            }
        }
    }
#endif
}
