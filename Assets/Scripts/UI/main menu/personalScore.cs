using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class personalScore : MonoBehaviour
{
    public GameObject canvas;
    public Button submitButton;
    public bool serverRecord = true;
    public static List<leader> PersonallistofLeaderboard = new List<leader>() { };

    private packagetosend sender;

    void Start()
    {
        sender = canvas.GetComponent<packagetosend>();
        if (serverRecord)
        {
            sender.send(packagetosend.sendpackettypes.REQUESTUSERRANK);
        }
        else
        {
            if (SaveSystemController.getBoolValue("PackagePending")) {
                //Load from save
                this.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unranked";
                this.transform.GetChild(1).gameObject.GetComponent<Text>().text = SaveSystemController.getValue("Package_Name");
                this.transform.GetChild(2).gameObject.GetComponent<Text>().text = NetworkUtility.convertToTime(SaveSystemController.getFloatValue("Package_Time"));
            }
            else
            {
                this.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You haven't beaten the boss yet";
                this.transform.GetChild(1).gameObject.GetComponent<Text>().text = "";
                if (SaveSystemController.getFloatValue("Package_Time") == -1.0f) {
                    this.transform.GetChild(2).gameObject.GetComponent<Text>().text = "";
                }
                else
                {
                    this.transform.GetChild(2).gameObject.GetComponent<Text>().text = NetworkUtility.convertToTime(SaveSystemController.getFloatValue("Package_Time"));
                }
            }
        }

        submitButton.interactable = SaveSystemController.getBoolValue("PackagePending");
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

    public void submitScore()
    {
        if (SaveSystemController.getBoolValue("PackagePending")) {

            submitButton.interactable = false;

            //Build package
            sender.ddID = "STEAM_0:0:98612737"; //TODO replace with propper steamID
            sender.ddmessage = SaveSystemController.getValue("Package_Message");
            sender.ddcurr = SaveSystemController.getIntValue("Package_Curr");
            sender.dditem1 = SaveSystemController.getIntValue("Package_Item1");
            sender.dditem2 = SaveSystemController.getIntValue("Package_Item2");
            sender.dditem3 = SaveSystemController.getIntValue("Package_Item3");
            sender.ddname = SaveSystemController.getValue("Package_Name");
            sender.ddtime = SaveSystemController.getValue("Package_Time");
            sender.ddhash = SaveSystemController.getValue("Package_MAGIC");

            //Ship it to the server
            sender.send(packagetosend.sendpackettypes.PACKAGESEND);

            //Reset Save
            SaveSystemController.Reset();

            //Reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
