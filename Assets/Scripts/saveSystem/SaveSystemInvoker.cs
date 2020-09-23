using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemInvoker : MonoBehaviour
{
    private void Awake()
    {
        //Load this first for things to work
        SaveSystemController.loadDataFromDisk();
    }
}
