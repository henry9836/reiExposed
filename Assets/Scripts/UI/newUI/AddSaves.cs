using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSaves : MonoBehaviour
{
    saveFile saveFile;

    public float V_volume = 0.68f;
    public string I_volume = "volume";

    public int V_level = 1;
    public string I_level = "level";

    public List<string> saves = new List<string>() { };

    void Start()
    {
        saveFile = GameObject.Find("save").GetComponent<saveFile>();


        saveSettings();
        saveProgress();
    }

    public void saveSettings()
    {
        saveFile.saveitem(I_volume, V_volume);
        //Debug.Log(saveFile.safeItem("I_volume", saveFile.types.FLOAT).tofloat);

    }

    public void saveProgress()
    { 
    
    }


}
