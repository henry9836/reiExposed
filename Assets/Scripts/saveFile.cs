using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveFile : MonoBehaviour
{
    private string saveFilePath = "test.cfg";
    private StreamWriter righter;

    public struct filedump
    {
        public bool exists;
        public string name;
        public string data;
        public int dataline;

        public filedump(bool mexists, string mname, string mdata, int mdataline)
        {
            this.exists = mexists;
            this.name = mname;
            this.data = mdata;
            this.dataline = mdataline;
        }
    }

    void Start()
    {
        Debug.Log(saveFilePath);


        righter = new StreamWriter(saveFilePath, true);
        righter.Close();

        saveitem("test", "zoop");
        Readitem("test");

    }


    public bool saveitem(string name, string item)
    {
        string input1 = "[" + name + "]" + "\n" + item;
        string input2 = item;

        filedump info = Readitem(name);

        if (info.exists == false)
        {
            righter = new StreamWriter(saveFilePath, true);
            righter.WriteLine(input1);
            righter.Close();
            Debug.Log("created");
        }
        else
        {
            //righter.write
            Debug.Log("already exist");
        }

        return true;
    }

    public filedump Readitem(string name)
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

            filedump exist = new filedump(true, name, tmp[save + 1], save + 1);


            return exist;
        }


        righter.Close();

        filedump nonexistant = new filedump();
        nonexistant.exists = false;
        return (nonexistant);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }
}
