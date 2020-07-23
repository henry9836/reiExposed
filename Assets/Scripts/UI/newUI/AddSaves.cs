using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSaves : MonoBehaviour
{
    saveFile saveFile;

    void Start()
    {
        saveFile = this.GetComponent<saveFile>();
        //firstSave();

        loadCurincies();
        saveSettings();
    }

    public void saveSettings()
    {
        saveFile.saveitem("volume", 0.69f);
    }

    public void loadSettings()
    {
        Debug.Log(saveFile.safeItem("volume", saveFile.types.FLOAT).tofloat);
    }

    public void loadCurincies()
    {
        currency.Yen = saveFile.safeItem("Yen", saveFile.types.INT).toint;
    }

    public void saveCurincies()
    {
        saveFile.saveitem("Yen", currency.Yen);
    }


    public void firstLoad()
    {
        saveFile.saveitem("Yen", 0);
    }

}
