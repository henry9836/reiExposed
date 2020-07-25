using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystemController
{

    private const string saveFile = "rei.sav";

    public class entry
    {
        public string id = "Untitled";
        public string value = "-1";

        public entry(string _id, string _val)
        {
            id = _id;
            value = _val;
        }

    }

    public static List<entry> saveInfomation = new List<entry>();
    public static bool ioBusy = false; //Used for telling user not to alt-f4

    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk() { saveDataToDisk(saveFile); }
    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk(string filePath)
    {
        //Set busy bit
        ioBusy = true;

        //Create saveFile if it doesn't exist and Open for writing
        StreamWriter writer = new StreamWriter(filePath, false);

        //For each entry in our save infomation overwrite file
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            writer.WriteLine(saveInfomation[i]);
        }

        //Close writer
        writer.Close();

        //Unset busy bit
        ioBusy = false;
    }

    //Update a value in our saveInfomation
    public static void updateValue(string _id, string _newValue)
    {
        //Find value
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                saveInfomation[i].value = _newValue;
                return;
            }
        }

        //Value was not found make a new entry
        saveInfomation.Add(new entry(_id, _newValue));
    }

    //Get a value from our saveInfomation
    public static string getValue(string _id)
    {
        //Find value
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                return saveInfomation[i].value;
            }
        }

        //ERROR
        return "-1";
    }


}
