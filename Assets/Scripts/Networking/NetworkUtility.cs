using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkUtility
{
    public static string convertToTime(float time)
    {
        int mins = Mathf.FloorToInt(time / 60.0f);
        int hours = Mathf.FloorToInt(mins / 60.0f);
        float secs = (((time / 60.0f) - mins) * 60.0f); //((mins non rounded) - (mins rounded)) * 60.0f
        //readjust mins
        mins = Mathf.FloorToInt((((mins / 60.0f) - hours) * 60.0f));

        //SQL does not allow 60 mins
        if (mins >= 60)
        {
            mins = 0;
        }

        //Construct string
        string strHours = hours.ToString();
        string strMins = mins.ToString();
        string strSecs = secs.ToString();

        string formatSec = strSecs.Substring(0, strSecs.IndexOf("."));

        //Nicer formatting
        if (strHours.Length < 2)
        {
            strHours = "0" + strHours;
        }
        if (strMins.Length < 2)
        {
            strMins = "0" + strMins;
        }
        if (formatSec.Length < 2)
        {
            strSecs = "0" + strSecs;
        }

        return strHours + ":" + strMins + ":" + strSecs;
    }
}
