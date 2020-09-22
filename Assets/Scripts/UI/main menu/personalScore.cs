using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class personalScore : MonoBehaviour
{
    public GameObject canvas;
    public bool serverRecord = true;
    public static List<leader> PersonallistofLeaderboard = new List<leader>() { };


    void Start()
    {
        if (serverRecord)
        {
            canvas.GetComponent<packagetosend>().send(packagetosend.sendpackettypes.REQUESTUSERRANK);
        }
        else
        {
            if (SaveSystemController.getBoolValue("PackagePending")) {
                //Load from save
                this.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unranked";
                this.transform.GetChild(1).gameObject.GetComponent<Text>().text = SaveSystemController.getValue("Package_Name");
                this.transform.GetChild(2).gameObject.GetComponent<Text>().text = SaveSystemController.getValue("Package_Time");
            }
            else
            {
                this.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You haven't beaten the boss yet";
                this.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
                this.transform.GetChild(2).gameObject.GetComponent<Text>().text = "";
            }
        }
    }

    void Update()
    {
        if (serverRecord)
        {
            if (PersonallistofLeaderboard.Count > 0)
            {
                create(PersonallistofLeaderboard[0]);
                PersonallistofLeaderboard.RemoveAt(0);
            }
        }
    }

    public void create(leader lead)
    {
        if (serverRecord)
        {
            this.transform.GetChild(0).gameObject.GetComponent<Text>().text = lead.position;
            this.transform.GetChild(1).gameObject.GetComponent<Text>().text = lead.name;
            this.transform.GetChild(2).gameObject.GetComponent<Text>().text = lead.time;
        }
    }
}
