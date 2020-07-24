using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class saveController
{
    /*
    [volume]
    0.69
    [MythTraces]
    99000
    [stign]
    w
    [groupEnabled]
    True
    [groupEnabled2]
    True
    [movespeed]
    3
    [testbounds.center.x]
    56.59856
    [testbounds.center.y]
    2.494967
    [testbounds.center.z]
    13.96222
    [testbounds.size.x]
    20.21215
    [testbounds.size.y]
    20.53987
    [testbounds.size.z]
    20.44885
    [stopnextto]
    False
    [recalc]
    True
    [recalcwhenidle]
    2
    [deets]
    2.5
    [rateofAnglechange]
    3
    [dynamicedgesize]
    10
    [themask]
    90625
    */



    public class entry
    {
        public string identifier = "ERROR";
        public string value = "0";

        public entry(string id, string val)
        {
            identifier = id;
            value = val;
        }
    }

    //Hold all save data in a list so that it is ready for quick access
    public static List<entry> saveContainer = new List<entry>();

    /// <summary>
    /// Updates a value in the save container to be used in a save file and ingame
    /// </summary>
    public static void updateVal(string id, string val) 
    {
        //Find and change value
        for (int i = 0; i < saveContainer.Count; i++)
        {
            if (saveContainer[i].identifier == id)
            {
                saveContainer[i].value = val;
                return;
            }
        }

        //Create container since we could not find a match ID
        saveContainer.Add(new entry(id, val));
    }

    /// <summary>
    /// Get a value from the save container
    /// </summary>
    public static string getVal(string id)
    {
        //Find and change value
        for (int i = 0; i < saveContainer.Count; i++)
        {
            if (saveContainer[i].identifier == id)
            {
                return saveContainer[i].value;
            }
        }

        return "";
    }




}
