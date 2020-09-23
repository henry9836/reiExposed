using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public static class SaveSystemController
{

    private const string saveFile = "rei.sav";
    private const string IDFLAG = "#{ID}#";
    private const string VALFLAG = "#{VAL}#";
    private const string SEPERATOR = "toCensor";
    private const string HASHID = "MAGIC";

    public class entry
    {
        enum TYPES
        {
            NONASSIGNED,
            INT,
            FLOAT,
            STRING
        }

        public string id = "Untitled";
        public string value = "-1";

        private int offset = 95999; //Used to hide from memory searches
        private TYPES type = TYPES.NONASSIGNED; 

        public entry(string _id)
        {
            id = _id;
        }

        public entry(string _id, string _val)
        {
            id = _id;
            value = _val;
        }

        private void setup(string hint)
        {
            //ID the type of data we are using
            int iNum;
            float fNum;

            if (int.TryParse(hint, out iNum))
            {
                //Apply Type
                type = TYPES.INT;
                
                //Apply Offset
                offset = UnityEngine.Random.Range(-9999, 9999);
                value = (iNum + offset).ToString();
            }
            else if (float.TryParse(hint, out fNum))
            {
                //Apply Type
                type = TYPES.FLOAT;

                //Apply Offset
                offset = UnityEngine.Random.Range(-9999, 9999);
                value = (fNum + offset).ToString();
            }
            else
            {
                type = TYPES.STRING;
            }
        }

        public void updateValue(string newVal)
        {
            //If first time adjusting
            if (type == TYPES.NONASSIGNED)
            {
                setup(newVal);
                return;
            }

            if (type == TYPES.INT)
            {
                //Get a new offset
                offset = UnityEngine.Random.Range(-9999, 9999);

                //Apply new value with new offset
                value = (int.Parse(newVal) + offset).ToString();

            }
            else if (type == TYPES.FLOAT)
            {
                //Get a new offset
                offset = UnityEngine.Random.Range(-9999, 9999);

                //Apply new value with new offset
                value = (float.Parse(newVal) + offset).ToString();
            }
            else
            {
                //Can't do much here
                value = newVal;
            }
        }
        public string getValue()
        {
            //If first time adjusting
            if (type == TYPES.NONASSIGNED)
            {
                setup(value);
            }

            if (type == TYPES.INT)
            {
                //Get Value
                int val = int.Parse(value);

                //Remove offset
                val -= offset;

                //Get a new offset and apply it
                offset = UnityEngine.Random.Range(-9999, 9999);
                value = (val + offset).ToString();

                //Return the value without the offset
                return val.ToString();
            }
            else if (type == TYPES.FLOAT)
            {
                //Get Value
                float val = float.Parse(value);

                //Remove offset
                val -= offset;

                //Get a new offset and apply it
                offset = UnityEngine.Random.Range(-9999, 9999);
                value = (val + offset).ToString();

                //Return the value without the offset
                return val.ToString();
            }
            else
            {
                //Can't do much here
                return value;
            }
        }

    }

    public static List<entry> saveInfomation = new List<entry>();
    public static bool readyForProcessing = false;
    public static bool ioBusy = false; //Used for telling user not to alt-f4
    public static bool loadedValues = false;

    private static List<entry> tmpList = new List<entry>();

    //Checks if the save file matches the currentHash
    public static bool checkSaveValid()
    {
        ulong hash = calcCurrentHash();
        if (getValue(HASHID) == "-1")
        {
            Debug.LogWarning("Hash not found/loaded!");
            return false;
        }
        return (hash == ulong.Parse(getValue(HASHID)));
    }

    //Creates a hash for the save file
    public static void updateHash()
    {
        updateValue(HASHID, calcCurrentHash().ToString());
    }

    //Generates a hash for validation
    public static ulong calcCurrentHash()
    {
        ulong hash = 1;

        //Spread values out more evenly, mod is a prime number to avoid collisons
        ulong mod = 2147483647;
        //Make a big number
        ulong mul = 99643;

        //We were using a chunksize but that was letting too much modifiation through
        //int chunkSize = 4;
        int chunkSize = 1; 

        string raw = "";
        raw += getValue("MythTraces");
        raw += getValue("shotgunDamageLVL");
        raw += getValue("shotgunRangeLVL");
        raw += getValue("shotgunBulletSpreadADSLVL");
        raw += getValue("shotgunBulletSpreadRunningLVL");
        raw += getValue("meeleeDamageLVL");
        raw += getValue("ammo");
        raw += getValue("ammoTwo");
        raw += getValue("PackagePending");
        raw += getValue("Package_Name");
        raw += getValue("Package_Time");

        byte[] bytes = Encoding.Default.GetBytes(raw);

        for (int i = 0; i < bytes.Length; i++)
        {
            if (i % chunkSize == 0)
            {
                hash += ulong.Parse(bytes[i].ToString()) * mul;
            }
        }

        return hash % mod;
    }

    //Resets a save file
    public static void Reset()
    {
        //Wait till we are allowed access to file
        while (ioBusy)
        {
            Debug.Log("ioBusy");
        }

        //Set busy bit
        ioBusy = true;
        string nameOfuser = "";
        if (SaveSystemController.getBoolValue("PackagePending"))
        {
            nameOfuser = SaveSystemController.getValue("Package_Name");
        }
        //Load default values
        //Read all lines into array
        tmpList.Clear();
        string[] lines = File.ReadAllLines(saveFile);

        bool nextValBreak = false;

        //Decode until we hit the default values Seperator
        for (int i = 0; i < lines.Length; i++)
        {
            //Is there a flag?
            if (lines[i].Contains(IDFLAG))
            {
                //Add id
                tmpList.Add(new entry(lines[i].Substring(IDFLAG.Length)));
            }
            else if (lines[i].Contains(VALFLAG))
            {
                //Set value of latest seen entry
                tmpList[tmpList.Count - 1].value = lines[i].Substring(VALFLAG.Length);
                if (nextValBreak)
                {
                    break;
                }
            }
            else
            {
                Debug.LogWarning($"Unknown Line {lines[i]}");
            }

            if (lines[i].Contains(SEPERATOR))
            {
                nextValBreak = true;
            }
        }

        //Delete Everything From File And Create a new one
        File.Delete(saveFile);
        saveInfomation.Clear();

        //Populate the file with default values
        //Create saveFile if it doesn't exist and Open for writing
        StreamWriter writer = new StreamWriter(saveFile, false);

        //For each entry in our save infomation overwrite file
        for (int i = 0; i < tmpList.Count; i++)
        {
            writer.WriteLine(IDFLAG + tmpList[i].id);
            writer.WriteLine(VALFLAG + tmpList[i].value);
        }

        //Add some money into player save
        writer.WriteLine(IDFLAG + "MythTraces");
        writer.WriteLine(VALFLAG + "500");
        if (nameOfuser != "")
        {
            writer.WriteLine(IDFLAG + "Package_Name");
            writer.WriteLine(VALFLAG + nameOfuser);
        }

        //Close writer
        writer.Close();

        //Reload
        ioBusy = false;
        readyForProcessing = false;
        loadDataFromDisk();
        tmpList.Clear();

        //Create Hash
        updateHash();
        saveDataToDisk();

        //Verify
        if (!checkSaveValid())
        {
            Debug.LogError("Reset Save File UnSuccessfully, trying again...");
            Reset();
        }

        Debug.Log("Reset Save File Successfully");
    }

    //Loads data from savefile into saveInfomation
    public static void loadDataFromDisk() { loadDataFromDisk(saveFile); } 
    //Loads data from savefile into saveInfomation
    public static void loadDataFromDisk(string filePath)
    {
        if (!readyForProcessing && !ioBusy)
        {
            //Queue A Thread Task
            ThreadPool.QueueUserWorkItem(loadDataFromDiskThread, filePath);
        }
        else
        {
            Debug.LogWarning("loadDataFromDisk Invoked but data is already loaded or the IO is busy");
        }
    }



    //Load Data Thread
    static void loadDataFromDiskThread(System.Object stateInfo)
    {
        readyForProcessing = false;
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
        loadedValues = true;
        readyForProcessing = true;
    }


    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk() { saveDataToDisk(saveFile); }
    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk(string filePath)
    {
        //Update Hash With Our Current Changes
        updateHash();
        //Queue A Thread Task
        ThreadPool.QueueUserWorkItem(saveDataToDiskThread, filePath);
    }
    //Save Data Thread
    static void saveDataToDiskThread(System.Object stateInfo)
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


    //Removed value from saveInfomation
    public static void removeValue(string _id)
    {

        //wait till ready to process infomation
       while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                saveInfomation.RemoveAt(i);
                return;
            }
        }
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

        //wait till ready to process infomation
        while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

        //Find value
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                saveInfomation[i].value = _newValue;
                return;
            }
        }

        //save information is an empty list to start with so eveything will be appended

        //Value was not found make a new entry
        saveInfomation.Add(new entry(_id, _newValue));
    }

    //Get a value from our saveInfomation
    public static string getValue(string _id)
    {
        //wait till ready to process infomation
       while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

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
        //wait till ready to process infomation
       while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

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
        //wait till ready to process infomation
       while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

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
        //wait till ready to process infomation
        while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

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
