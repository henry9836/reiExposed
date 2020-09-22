using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Packager : MonoBehaviour
{
    public packagetosend sender;
    public Text message;
    public Text currency;
    public Text nameField;
    public Button submitButton;
    public Button attachButton;
    public Image attachmentOneImage;
    public Image attachmentTwoImage;
    public Image attachmentThreeImage;

    public Items.AllItems item1 = Items.AllItems.NONE;
    public Items.AllItems item2 = Items.AllItems.NONE;
    public Items.AllItems item3 = Items.AllItems.NONE;

    //SQLi filter, it is also serverside
    private string[] blacklist = {"'",
                                "\"",
                                "--",
                                "#",
                                "/*",
                                "*/",
                                "/*!",
                                ";",
                                "UNION",
                                "EXEC",
                                "0x",
                                "\\" };

    float levelTime = 3600.0f;
    Items items;
    Color originalMessageColor;
    Color originalCurrencyColor;

    private void Start()
    {
        levelTime = Time.timeSinceLevelLoad;
        Cursor.visible = true;
        originalMessageColor = message.color;
        originalCurrencyColor = currency.color;
        submitButton.interactable = false;
        items = transform.root.GetComponent<Items>();
    }

    //Attach an item
    public bool attach(Items.AllItems item)
    {
        if (item1 == Items.AllItems.NONE)
        {
            item1 = item;
            return true;
        }
        else if (item2 == Items.AllItems.NONE)
        {
            item2 = item;
            return true;
        }
        else if (item3 == Items.AllItems.NONE)
        {
            item3 = item;
            return true;
        }
        return false;
    }

    //Deattach an item
    public void remove(Items.AllItems item)
    {
        if (item == item1)
        {
            item1 = Items.AllItems.NONE;
        }
        else if (item == item2)
        {
            item2 = Items.AllItems.NONE;
        }
        else if (item == item3)
        {
            item3 = Items.AllItems.NONE;
        }
    }

    private void FixedUpdate()
    {

        if (Cursor.lockState != CursorLockMode.Confined || !Cursor.visible)
        {
            //Confine mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        //Show current attachments
        attachmentOneImage.enabled = !(item1 == Items.AllItems.NONE);
        attachmentTwoImage.enabled = !(item2 == Items.AllItems.NONE);
        attachmentThreeImage.enabled = !(item3 == Items.AllItems.NONE);

        attachmentOneImage.sprite = items.images[(int)item1];
        attachmentTwoImage.sprite = items.images[(int)item2];
        attachmentThreeImage.sprite = items.images[(int)item3];

        //Give feedback on the bad ones
        if (message.text.Length <= 0 || message.text.Length > 230)
        {
            message.color = Color.red;
        }
        else
        {
            message.color = originalMessageColor;
        }

        //Check for SQLi
        bool blacklisted = false;
        bool blacklistedName = false;
        for (int i = 0; i < blacklist.Length; i++)
        {
            if (message.text.Contains(blacklist[i]))
            {
                blacklisted = true;
                break;
            }

            if (nameField.text.Contains(blacklist[i]))
            {
                blacklistedName = true;
                break;
            }
        }

        //If passed SQLi checks
        if (!blacklisted && !blacklistedName)
        {
            nameField.color = originalCurrencyColor;

            //Check for valid input currenecy
            int userInputCurrency = 0;
            if (int.TryParse(currency.text, out userInputCurrency))
            {
                if (userInputCurrency < 0 || userInputCurrency > SaveSystemController.getIntValue("MythTraces"))
                {
                    currency.color = Color.red;
                    return;
                }
                else
                {
                    currency.color = originalCurrencyColor;
                }
            }

            if (message.text.Length > 230)
            {
                message.color = Color.red;
            }
            if (nameField.text.Length > 30)
            {
                nameField.color = Color.red;
            }


            if ((nameField.text.Length > 0 && nameField.text.Length <= 60) && (message.text.Length > 0 && message.text.Length <= 230) && (int.Parse(currency.text) >= 0) && (int.Parse(currency.text) <= SaveSystemController.getIntValue("MythTraces")))
            {
                submitButton.interactable = true;
            }
            else
            {
                submitButton.interactable = false;
            }
        }
        //Has a blacklisted character
        else
        {
            if (blacklistedName)
            {
                nameField.color = Color.red;
            }
            else
            {
                message.color = Color.red;
            }
            submitButton.interactable = false;

        }
    }

    //Send package
    public void Submit()
    {
        //Build package
        //sender.ddID = "STEAM_0:0:98612737"; //TODO replace with propper steamID
        //sender.ddmessage = message.text;
        int curr = int.Parse(currency.text);
        if (curr < 100)
        {
            curr += 100;
        }
        //sender.ddcurr = curr;
        //sender.dditem1 = (int)item1;
        //sender.dditem2 = (int)item2;
        //sender.dditem3 = (int)item3;
        //sender.ddname = nameField.text;
        //sender.ddtime = NetworkUtility.convertToTime(levelTime);

        ////Remove Items
        //items.removeitemequipped(item1, false);
        //items.removeitemequipped(item2, false);
        //items.removeitemequipped(item3, false);

        //Send package
        //sender.send(1);

        //Save To File
        SaveSystemController.updateValue("PackagePending", true);
        SaveSystemController.updateValue("Package_STEAM_ID", "STEAM_0:0:98612737");
        SaveSystemController.updateValue("Package_Message", message.text);
        SaveSystemController.updateValue("Package_Curr", curr);
        SaveSystemController.updateValue("Package_Item1", (int)item1);
        SaveSystemController.updateValue("Package_Item2", (int)item2);
        SaveSystemController.updateValue("Package_Item3", (int)item3);
        SaveSystemController.updateValue("Package_Name", nameField.text);
        SaveSystemController.updateValue("Package_Time", NetworkUtility.convertToTime(levelTime));

        //Remove MythTraces
        SaveSystemController.updateValue("MythTraces", SaveSystemController.getIntValue("MythTraces") - int.Parse(currency.text));
        SaveSystemController.saveDataToDisk();

        //Lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Load into main menu
        while (SaveSystemController.ioBusy)
        {
            Debug.Log("Waiting On Save System IO");
        }
        SceneManager.LoadScene(0);

        //Close packager
        gameObject.SetActive(false);
    }
}
