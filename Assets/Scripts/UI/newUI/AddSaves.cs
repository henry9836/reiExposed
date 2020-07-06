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
        currency.Followers = saveFile.safeItem("Followers", saveFile.types.INT).toint;
        currency.MythTraces = saveFile.safeItem("MythTraces", saveFile.types.INT).toint;
        currency.credits = saveFile.safeItem("credits", saveFile.types.INT).toint;

        currency.tengu = saveFile.safeItem("tengu", saveFile.types.INT).toint;
        currency.kappa = saveFile.safeItem("kappa", saveFile.types.INT).toint;
        currency.oni = saveFile.safeItem("oni", saveFile.types.INT).toint;
    }

    public void saveCurincies()
    {
        saveFile.saveitem("Followers", currency.Followers);
        saveFile.saveitem("MythTraces", currency.MythTraces);
        saveFile.saveitem("credits", currency.credits);
        saveFile.saveitem("tengu", currency.tengu);
        saveFile.saveitem("kappa", currency.kappa);
        saveFile.saveitem("oni", currency.oni);
    }


    public void firstLoad()
    {
        saveFile.saveitem("Followers", 0);
        saveFile.saveitem("MythTraces", 0);
        saveFile.saveitem("credits", 0);
        saveFile.saveitem("tengu", 0);
        saveFile.saveitem("kappa", 0);
        saveFile.saveitem("oni", 0);
        saveFile.saveitem("volume", 1.0f);

    }

}
