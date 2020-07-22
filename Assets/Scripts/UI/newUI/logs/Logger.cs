using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public class LogContainer
    {
        public string msg = "ERROR";
        public bool important = false;

        public LogContainer(string m)
        {
            msg = m;
            important = false;
        }
        public LogContainer(string m, bool i)
        {
            msg = m;
            important = i;
        }
    }

    public List<LogContainer> logs = new List<LogContainer>();

    void AddNewMessage(LogContainer log)
    {
        logs.Add(log);
    }




    private void Start()
    {
        //Testing
        for (int i = 0; i < 15; i++)
        {
            AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), false));
            AddNewMessage(new LogContainer(Random.Range(0, 9999).ToString(), true));
        }

    }

}
