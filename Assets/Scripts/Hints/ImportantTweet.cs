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

    private float currency = 100.0f;

    private void Start()
    {

        currency = Random.Range(100.0f, 300.0f);

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
        dropControl
        logger.AddNewMessage(new Logger.LogContainer(hint, hintImportant));
    }

}
