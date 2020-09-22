using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class personalScore : MonoBehaviour
{
    public GameObject canvas;
    public static List<leader> PersonallistofLeaderboard = new List<leader>() { };


    void Start()
    {
        canvas.GetComponent<packagetosend>().send(packagetosend.sendpackettypes.REQUESTUSERRANK);
    }

    void Update()
    {
        if (PersonallistofLeaderboard.Count > 0)
        {
            create(PersonallistofLeaderboard[0]);
            PersonallistofLeaderboard.RemoveAt(0);
        }
    }

    public void create(leader lead)
    {
        this.transform.GetChild(0).gameObject.GetComponent<Text>().text = lead.position;
        this.transform.GetChild(1).gameObject.GetComponent<Text>().text = lead.name;
        this.transform.GetChild(2).gameObject.GetComponent<Text>().text = lead.time;
    }
}
