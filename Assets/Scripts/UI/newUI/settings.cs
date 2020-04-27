using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    saveFile save;

    void Start()
    {
        float volume = 0.69f;
        save = GameObject.Find("save").GetComponent<saveFile>();  
        
        save.saveitem("volume", volume);

        Debug.Log(save.safeItem("volume", saveFile.types.FLOAT).tofloat);


    }


}
