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

    Color originalMessageColor;
    Color originalCurrencyColor;

    private void Start()
    {
        originalMessageColor = message.color;
        originalCurrencyColor = currency.color;
        submitButton.interactable = false;
    }

    private void FixedUpdate()
    {
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
        sender.dditem1 = 0;
        sender.dditem2 = 0;
        sender.dditem3 = 0;

        //Send package
        //sender.send(1);
    }
}
