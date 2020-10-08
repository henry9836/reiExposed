using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystemController
{

    private const string saveFile = "rei.sav";
    private const string IDFLAG = "#{ID}#";
    private const string VALFLAG = "#{VAL}#";
    private const string SEPERATOR = "toCensor";
    private const string HASHID = "MAGIC";


    private static System.Random rng = new System.Random();
    private static float lastCheckTime = 0.0f;

    public class entry
    {
        public enum TYPES
        {
            NONASSIGNED,
            INT,
            FLOAT,
            STRING
        }

        public string id = "Untitled";
        public string value = "-1";
        public TYPES type = TYPES.NONASSIGNED;

        private int offset = 95999; //Used to hide from memory searches
        private int tmpI = 0;
        private float tmpF = 0.0f;

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

            if (int.TryParse(hint, out tmpI))
            {
                //Apply Type
                type = TYPES.INT;

                //Apply Offset
                offset = rng.Next(-9999, 9999);
                value = (tmpI + offset).ToString();
            }
            else if (float.TryParse(hint, out tmpF))
            {
                //Apply Type
                type = TYPES.FLOAT;

                //Apply Offset
                offset = rng.Next(-9999, 9999);
                value = (tmpF + offset).ToString();
            }
            else
            {
                type = TYPES.STRING;
            }
        }

        public void tick()
        {
            updateValue(value);
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
                offset = rng.Next(-9999, 9999);

                //Apply new value with new offset
                value = (int.Parse(newVal) + offset).ToString();

            }
            else if (type == TYPES.FLOAT)
            {
                //Get a new offset
                offset = rng.Next(-9999, 9999);

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
                int tmpI = int.Parse(value);

                //Remove offset
                tmpI -= offset;

                //Get a new offset and apply it
                offset = rng.Next(-9999, 9999);
                value = (tmpI + offset).ToString();

                //Return the value without the offset
                return tmpI.ToString();
            }
            else if (type == TYPES.FLOAT)
            {
                //Get Value
                float tmpF = float.Parse(value);

                //Remove offset
                tmpF -= offset;

                //Get a new offset and apply it
                offset = rng.Next(-9999, 9999);
                value = (tmpF + offset).ToString();

                //Return the value without the offset
                return tmpF.ToString();
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
        updateValue(HASHID, calcCurrentHash().ToString(), true);
    }

    //Generates a hash for validation
    public static ulong calcCurrentHash() {

        string input = "";
        input += getValue("MythTraces");
        input += getValue("shotgunDamageLVL");
        input += getValue("shotgunRangeLVL");
        input += getValue("shotgunBulletSpreadADSLVL");
        input += getValue("shotgunBulletSpreadRunningLVL");
        input += getValue("meeleeDamageLVL");
        input += getValue("ammo");
        input += getValue("ammoTwo");
        input += getValue("PackagePending");
        input += getValue("Package_Name");
        input += getValue("Package_STEAM_ID");
        input += getValue("Package_Message");
        input += getValue("Package_Curr");
        input += getValue("Package_Item1");
        input += getValue("Package_Item2");
        input += getValue("Package_Item3");
        input += getValue("Package_Time");
        input += getValue("Package_MAGIC");

        return calcCurrentHash(input); 
    }
    public static ulong calcCurrentHash(string input)
    {
        ulong hash = 1;

        //Spread values out more evenly, mod is a prime number to avoid collisons
        ulong mod = 2147483647;
        //Make a big number
        ulong mul = 99643;

        //We were using a chunksize but that was letting too much modifiation through
        //int chunkSize = 4;
        int chunkSize = 1; 

        byte[] bytes = Encoding.Default.GetBytes(input);

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
        Debug.Log("RESET CALLED!!!");

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
        tmpList = null;
        tmpList = new List<entry>();

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
        saveInfomation = null;
        saveInfomation = new List<entry>();

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
        tmpList = null;

        //Reload
        ioBusy = false;
        readyForProcessing = false;
        loadDataFromDisk();

        //Create Hash
        saveDataToDisk();

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
                if (lines[i - 1].Contains(HASHID))
                {
                    saveInfomation[saveInfomation.Count - 1].type = entry.TYPES.STRING;
                }
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

    public static float getCurrentTime()
    {
        //Update Our Time
        float oldTime = getFloatValue("Package_Time");
        float newTime = Time.timeSinceLevelLoad;

        //Offset time to our current time since last time we saved
        if (lastCheckTime <= Time.timeSinceLevelLoad)
        {
            newTime = newTime - lastCheckTime;
        }
        lastCheckTime = Time.timeSinceLevelLoad;


        if (oldTime == -1.0f)
        {
            oldTime = 0.0f;
        }
        //Append value
        newTime += oldTime;
        updateValue("Package_Time", newTime);

        return newTime;
    }

    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk() { saveDataToDisk(saveFile, false); }
    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk(bool overrideTime) { saveDataToDisk(saveFile, overrideTime); }
    //Saves current state of saveInfomation to save file
    public static void saveDataToDisk(string filePath, bool overrideTime)
    {
        if (!overrideTime)
        {
            //Update our time
            getCurrentTime();
        }
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
            writer.WriteLine(VALFLAG + saveInfomation[i].getValue());
        }

        //Close writer
        writer.Close();
        writer = null;

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
    public static void updateValue(string _id, bool _newValue) { updateValue(_id, _newValue.ToString(), false, false); }
    //Update a value in our saveInfomation
    public static void updateValue(string _id, float _newValue) { updateValue(_id, _newValue.ToString(), false, false); }
    //Update a value in our saveInfomation
    public static void updateValue(string _id, int _newValue) { updateValue(_id, _newValue.ToString(), false, false); }

    //Update a value in our saveInfomation
    public static void updateValue(string _id, string _newValue, bool overrideToString) { updateValue(_id, _newValue, overrideToString, false); }
    public static void updateValue(string _id, string _newValue, bool overrideToString, bool overrideWait)
    {

        //wait till ready to process infomation
        while (!readyForProcessing && !overrideWait) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

        bool valFound = false;

        //Find value
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                if (overrideToString)
                {
                    Debug.Log($"CREATED A OVERRIDE OBJECT: {saveInfomation[i].id}");
                    saveInfomation[i].type = entry.TYPES.STRING;
                }
                saveInfomation[i].updateValue(_newValue);
                valFound = true;
            }
            else
            {
                //Crashes game to crash?
                //saveInfomation[i].tick();
            }
        }

        if (valFound)
        {
            return;
        }

        //save information is an empty list to start with so eveything will be appended

        //Value was not found make a new entry
        saveInfomation.Add(new entry(_id, _newValue));
    }

    //Get a value from our saveInfomation
    public static string getValue(string _id) { return getValue(_id, false); }
    public static string getValue(string _id, bool overrideWait)
    {
        //wait till ready to process infomation
       while (!readyForProcessing && !overrideWait) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

        string valFound = "-1";

        //Find value
        for (int i = 0; i < saveInfomation.Count; i++)
        {
            if (saveInfomation[i].id == _id)
            {
                valFound = saveInfomation[i].getValue();
            }
            else
            {
                //Crashes game to crash?
                //saveInfomation[i].tick();
            }
        }

        //Exit
        return valFound;
    }

    //Load data as int type
    public static int getIntValue(string _id)
    {
        //wait till ready to process infomation
       while (!readyForProcessing) { Debug.LogError("Waiting on save system to be ready for processing, have you loaded data from disk?"); }

        int result = -1;
        if (int.TryParse(getValue(_id, false), out result))
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
        if (float.TryParse(getValue(_id, false), out result))
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
        if (bool.TryParse(getValue(_id, false), out result))
        {
            return result;
        }
        else
        {
            return false;
        }
    }

}
