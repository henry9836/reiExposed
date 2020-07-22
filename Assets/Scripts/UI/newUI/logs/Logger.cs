using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float spacing = 200.0f;
    public static int nextID = 1;

    void removeMessage(int ID)
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
    }


    void AddNewMessage(LogContainer log)
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
        //Assign ID
        container.GetComponent<MessageInfo>().log = logs[logs.Count - 1];

        //Assign Obj Ref
        logs[logs.Count - 1].ui = container;

        //Move position to make sense
        container.GetComponent<RectTransform>().localPosition = pos;

    }

    private void Start()
    {
        //Testing
        for (int i = 0; i < 15; i++)
        {
            AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), false));
            AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), true));
        }

        StartCoroutine(testDel());

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
            else
            {
                break;
            }
        }
        
    }

}
