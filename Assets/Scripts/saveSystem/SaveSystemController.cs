﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class SaveSystemController
{

    private const string saveFile = "rei.sav";
    private const string IDFLAG = "#{ID}#";
    private const string VALFLAG = "#{VAL}#";

    public class entry
    {
        public string id = "Untitled";
        public string value = "-1";

        public entry(string _id)
        {
            id = _id;
        }

        public entry(string _id, string _val)
        {
            id = _id;
            value = _val;
        }

    }

    public static List<entry> saveInfomation = new List<entry>();
    public static bool ioBusy = false; //Used for telling user not to alt-f4

    //Loads data from savefile into saveInfomation
    public static void loadDataFromDisk() { loadDataFromDisk(saveFile); } 
    //Loads data from savefile into saveInfomation
    public static void loadDataFromDisk(string filePath)
    {
        //Queue A Thread Task
        ThreadPool.QueueUserWorkItem(loadDataFromDiskThread, filePath);

    }

    //Load Data Thread
    static void loadDataFromDiskThread(System.Object stateInfo)
    {
        string filePath = stateInfo as string;

        if (!File.Exists(filePath))
        {
            return;
        }

        //Wait for file to avaible
        while (ioBusy){}

        //Set busy bit
        ioBusy = true;

        //Read all lines into array
        string[] lines = File.ReadAllLines(filePath);

        //Decode
        for (int i = 0; i < lines.Length; i++)
        {
            //Is there a flag?
            if (lines[i].Contains(IDFLAG))
            {
                //Add id
                saveInfomation.Add(new entry(lines[i].Substring(IDFLAG.Length)));
            }
            else if (lines[i].Contains(VALFLAG))
            {
                //Set value of latest seen entry
                saveInfomation[saveInfomation.Count - 1].value = lines[i].Substring(VALFLAG.Length);
            }
            else
            {
                Debug.LogWarning($"Unknown Line {lines[i]}");
            }
        }

        //Unset busy bit
        ioBusy = false;
    }


    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk() { saveDataToDisk(saveFile); }
    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk(string filePath)
    {
        //Queue A Thread Task
        ThreadPool.QueueUserWorkItem(saveDataFromDiskThread, filePath);
    }
    //Save Data Thread
    static void saveDataFromDiskThread(System.Object stateInfo)
    {
        string filePath = stateInfo as string;

        //Wait for file to avaible
        while (ioBusy) { }

        //Set busy bit
        ioBusy = true;

        //Create saveFile if it doesn't exist and Open for writing
        StreamWriter writer = new StreamWriter(filePath, false);

        //For each entry in our save infomation overwrite file
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            writer.WriteLine(IDFLAG + saveInfomation[i].id);
            writer.WriteLine(VALFLAG + saveInfomation[i].value);
        }

        //Close writer
        writer.Close();

        //Unset busy bit
        ioBusy = false;
    }


    //Update a value in our saveInfomation
    public static void updateValue(string _id, bool _newValue) { updateValue(_id, _newValue.ToString()); }
    //Update a value in our saveInfomation
    public static void updateValue(string _id, float _newValue) { updateValue(_id, _newValue.ToString()); }
    //Update a value in our saveInfomation
    public static void updateValue(string _id, int _newValue) { updateValue(_id, _newValue.ToString()); }

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

    //Load data as int type
    public static int getIntValue(string _id)
    {
        int result = -1;
        if (int.TryParse(getValue(_id), out result))
        {
            return result;
        }
        else
        {
            return -1;
        }
    }
    //Load data as float type
    public static float getFloatValue(string _id)
    {
        float result = -1.0f;
        if (float.TryParse(getValue(_id), out result))
        {
            return result;
        }
        else
        {
            return -1.0f;
        }
    }
    //Load data as bool type
    public static bool getBoolValue(string _id)
    {
        bool result = false;
        if (bool.TryParse(getValue(_id), out result))
        {
            return result;
        }
        else
        {
            return false;
        }
    }
}