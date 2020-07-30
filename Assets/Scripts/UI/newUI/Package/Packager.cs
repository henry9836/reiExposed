using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Packager : MonoBehaviour
{
    public packagetosend sender;
    public Text message;
    public Text currency;
    public Button submitButton;
    public Button attachButton;
    public Image attachmentOneImage;
    public Image attachmentTwoImage;
    public Image attachmentThreeImage;

    public Items.AllItems item1 = Items.AllItems.NONE;
    public Items.AllItems item2 = Items.AllItems.NONE;
    public Items.AllItems item3 = Items.AllItems.NONE;

    Items items;
    Color originalMessageColor;
    Color originalCurrencyColor;

    private void Start()
    {
        originalMessageColor = message.color;
        originalCurrencyColor = currency.color;
        submitButton.interactable = false;
        items = transform.root.GetComponent<Items>();
    }

    //Attach an item
    public void attach(Items.AllItems item)
    {
        if (item1 == Items.AllItems.NONE)
        {
            item1 = item;
        }
        else if (item2 == Items.AllItems.NONE)
        {
            item2 = item;
        }
        else if (item3 == Items.AllItems.NONE)
        {
            item3 = item;
        }
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
        //Show current attachments
        attachmentOneImage.enabled = !(item1 == Items.AllItems.NONE);
        attachmentTwoImage.enabled = !(item2 == Items.AllItems.NONE);
        attachmentThreeImage.enabled = !(item3 == Items.AllItems.NONE);

        attachmentOneImage.sprite = items.images[(int)item1];
        attachmentTwoImage.sprite = items.images[(int)item2];
        attachmentThreeImage.sprite = items.images[(int)item3];

        //Give feedback on the bad ones
        if (message.text.Length <= 0)
        {
            message.color = Color.red;
        }
        else
        {
            message.color = originalMessageColor;
        }
        if (int.Parse(currency.text) < 10)
        {
            currency.color = Color.red;
        }
        else
        {
            currency.color = originalCurrencyColor;
        }

        if (message.text.Length > 0 && (int.Parse(currency.text) >= 10))
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }

    //Send package
    public void Submit()
    {
        //Build package
        sender.ddID = "STEAM_0:0:98612737"; //TODO replace with propper steamID
        sender.ddmessage = message.text;
        sender.ddcurr = int.Parse(currency.text);
        sender.dditem1 = (int)item1;
        sender.dditem2 = (int)item2;
        sender.dditem3 = (int)item3;

        //Send package
        //sender.send(1);
    }
}
