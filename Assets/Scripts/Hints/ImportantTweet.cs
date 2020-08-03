using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantTweet : MonoBehaviour
{

    public Logger logger;
    public enemydrop dropControl;

    public string hint = "Important Hint";
    public Items.AllItems item1 = Items.AllItems.NONE;
    public Items.AllItems item2 = Items.AllItems.NONE;
    public Items.AllItems item3 = Items.AllItems.NONE;
    public bool hintImportant = true;

    private int currency = 100;

    private void Start()
    {

        currency = Random.Range(100, 300);

        if (!logger)
        {
            logger = GameObject.Find("MessageLog").GetComponent<Logger>();
        }
        if (!dropControl)
        {
            dropControl = GameObject.Find("Canvas").GetComponent<enemydrop>();
        }
    }

    public void triggerTweet()
    {
        dropControl.manualMessage(hint, currency, (int)item1, (int)item2, (int)item3);
        logger.AddNewMessage(new Logger.LogContainer(hint, hintImportant));
    }

}
