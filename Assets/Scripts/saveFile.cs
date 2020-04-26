using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveFile : MonoBehaviour
{
    private string saveFilePath = "test.cfg";
    private StreamWriter righter;

    void Start()
    {
        Debug.Log(saveFilePath);


        righter = new StreamWriter(saveFilePath, true);
        righter.Close();

        createitem("test", "zoop");
        readitem("test");

    }


    public bool createitem(string name, string item)
    {
        string input = "[" + name + "]" + "\n" + item;

        if (readitem(name) == "")
        {
            righter = new StreamWriter(saveFilePath, true);
            righter.WriteLine(input);
            righter.Close();
            Debug.Log("created");

        }
        else
        {
            Debug.Log("already exist");
        }



        return true;
    }

    public string readitem(string name)
    {
        string[] tmp = File.ReadAllLines(saveFilePath);
        int save = 9999;

        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] == "[" + name + "]")
            {
                save = i;
            }
        }

        if (save != 9999)
        {
            righter.Close();

            return tmp[save + 1];
        }


        righter.Close();

        return "";
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }
}
