using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class convertDeals : MonoBehaviour
{
    public Text followers;
    public Text mythtraces;


    void Start()
    {
        updateui();
    }

    public void deal(string curr) 
    {
        string[] xycurr = curr.Split(' ');

        int xcurr = int.Parse(xycurr[0]);
        int ycurr = int.Parse(xycurr[1]);

        Debug.Log(":"+ xcurr + ":" + " " + ":" + ycurr + ":");


        if (currency.MythTraces >= xcurr)
        {
            currency.MythTraces -= xcurr;
            currency.Followers += ycurr;
            updateui();

        }
        else
        { 
            //angery shake
        }

    }

    public void updateui()
    {
        followers.text = currency.Followers.ToString();
        mythtraces.text = currency.MythTraces.ToString();

    }
}
