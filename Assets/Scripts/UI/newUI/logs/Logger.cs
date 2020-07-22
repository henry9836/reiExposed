using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    public class LogContainer
    {
        public string msg = "ERROR";
        public bool important = false;
        public int ID = -1;
        public GameObject ui;

        public LogContainer(string m)
        {
            msg = m;
            important = false;
            ID = nextID;
            nextID++;
        }
        public LogContainer(string m, bool i)
        {
            msg = m;
            important = i; 
            ID = nextID;
            nextID++;
        }
    }

    public List<LogContainer> logs = new List<LogContainer>();
    public GameObject contrainerPrefab;
    public RectTransform messageAnchor;
    public LogScrollController logScrollCtrl;
    public float spacing = 200.0f;
    public static int nextID = 1;

    Vector3 initalMsgPos = Vector3.zero;
    Image backdrop;
    Color backdropColor;

    public void showMsgs()
    {
        for (int i = 0; i < logs.Count; i++)
        {
            logs[i].ui.GetComponent<MessageInfo>().showUI();
        }
        backdrop.color = new Color(backdrop.color.r, backdrop.color.g, backdrop.color.b, backdropColor.a);
    }

    public void hideMsgs()
    {
        for (int i = 0; i < logs.Count; i++)
        {
            logs[i].ui.GetComponent<MessageInfo>().hideUI();
        }
        backdrop.color = new Color(backdrop.color.r, backdrop.color.g, backdrop.color.b, 0.0f);
    }

    public void resetMsgs()
    {
        messageAnchor.position = initalMsgPos;
    }

    void reorderMsgs()
    {
        if (logs.Count > 0)
        {
            List<LogContainer> reorderedLogs = new List<LogContainer>();

            //Append all important logs
            for (int i = 0; i < logs.Count; i++)
            {
                if (logs[i].important)
                {
                    reorderedLogs.Add(logs[i]);
                    //Remove from logs
                    logs.RemoveAt(i);
                }
            }

            //Add the rest onto the end of the list
            for (int i = 0; i < logs.Count; i++)
            {
                reorderedLogs.Add(logs[i]);
            }

            logs.Clear();

            //Move back onto logs
            for (int i = 0; i < reorderedLogs.Count; i++)
            {
                logs.Add(reorderedLogs[i]);
            }
            //logs = reorderedLogs;
            reorderedLogs.Clear();


            //Reorder UI
            for (int i = 0; i < logs.Count; i++)
            {
                if (i == 0)
                {
                    logs[i].ui.GetComponent<RectTransform>().localPosition = -(Vector3.up * spacing);
                }
                else
                {
                    logs[i].ui.GetComponent<RectTransform>().localPosition = logs[i - 1].ui.GetComponent<RectTransform>().localPosition - (Vector3.up * spacing);
                }
            }

            //Update Controller
            logScrollCtrl.updateInfo(logs[0].ui.GetComponent<RectTransform>(), logs[logs.Count - 1].ui.GetComponent<RectTransform>());
        } 
    }

    public void removeMessage(int ID)
    {
        for (int i = 0; i < logs.Count; i++)
        {
            if (logs[i].ID == ID)
            {
                Destroy(logs[i].ui);
                logs.RemoveAt(i);
                break;
            }
        }

        reorderMsgs();
    }


    public void AddNewMessage(LogContainer log)
    {
        logs.Add(log);
        //Get Postition to move to
        Vector3 pos = Vector3.zero;
        if (messageAnchor.childCount > 0) {
            pos = messageAnchor.GetChild(messageAnchor.childCount - 1).GetComponent<RectTransform>().localPosition - (Vector3.up * spacing);
        }
        else
        {
            pos = -(Vector3.up * spacing);
        }
        //Create a UI Element and parent to logger
        GameObject container = Instantiate(contrainerPrefab, Vector3.zero, Quaternion.identity, messageAnchor);

        //Assign ID and setup
        container.GetComponent<MessageInfo>().log = logs[logs.Count - 1];
        container.GetComponent<MessageInfo>().setup();

        //Assign Obj Ref
        logs[logs.Count - 1].ui = container;

        //Move position to make sense
        container.GetComponent<RectTransform>().localPosition = pos;

        reorderMsgs();
    }

    private void Start()
    {

        if (!logScrollCtrl)
        {
            logScrollCtrl = GetComponent<LogScrollController>();
        }

        initalMsgPos = messageAnchor.position;
        backdrop = GetComponent<Image>();
        backdropColor = backdrop.color;

        //Testing
        //for (int i = 0; i < 15; i++)
        //{
        //    AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), false));
        //    AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), true));
        //}

        //StartCoroutine(testDel());

    }

    IEnumerator testDel()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (logs.Count > 0)
            {
                removeMessage(logs[Random.Range(0, logs.Count)].ID);
            }
        }
        
    }

}
