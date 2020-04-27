using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.HighDefinition;



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

        public string[] allData;

        public filedump(bool mexists, string mname, string mdata, int mdataline, string[] mallData)
        {
            this.exists = mexists;
            this.name = mname;
            this.data = mdata;
            this.dataline = mdataline;
            this.allData = mallData;
        }
    }

    public struct interpret
    {
        public int toint;
        public float tofloat;
        public string tostring;
    }

    public enum types
    { 
        INT,
        STRING,
        FLOAT,
    };

    void Start()
    {
        righter = new StreamWriter(saveFilePath, true);
        righter.Close();

        int no1 = 420;
        string no2 = "meme";
        float no3 = 69.9999f;

        saveitem("int", no1);
        saveitem("string", no2);
        saveitem("float", no3);

        //int test1 = safeItem("int", types.INT).toint; //returns int
        //string test2 = safeItem("string", types.STRING).tostring; //returns string
        //int test3 = safeItem("float", types.INT).toint; //error cannot covert float to int
    }


    public void saveitem(string name, int item)
    {
        saveitem(name, item.ToString());
    }

    public void saveitem(string name, float item)
    {
        saveitem(name, item.ToString());
    }


    public void saveitem(string name, string item)
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
            if (info.data != item)
            {
                righter = new StreamWriter(saveFilePath, false);

                Debug.Log(info.allData);

                string toinsert = "";

                for (int i = 0; i < info.allData.Length; i++)
                {
                    if (i == info.dataline)
                    {
                        toinsert = toinsert + input2 + "\n";
                    }
                    else
                    {
                        toinsert = toinsert + info.allData[i] + "\n";
                    }
                }

                righter.Write(toinsert);

                righter.Close();

                Debug.Log("overwritten");
            }
            else
            {
                Debug.Log("already exists");
            }
        }
    }

    public interpret safeItem(string name, types type)
    {
        interpret test = GetItem(name);

        if (type == types.INT)
        {
            if (test.toint == -999999)
            {
                Debug.LogError("Failed to save >" + name +  "< of type " + type);
            }
        }
        else if (type == types.FLOAT)
        {
            if (test.tofloat == -999999f)
            {
                Debug.LogError("Failed to save >" + name + "< of type " + type);
            }
        }

        return (test);
    }

    public interpret GetItem(string name)
    {
        filedump tmp = Readitem(name);

        interpret ans = new interpret();

        if (int.TryParse(tmp.data, out ans.toint) == false)
        {
            ans.toint = -999999;
        }
        if (float.TryParse(tmp.data, out ans.tofloat) == false)
        {
            ans.tofloat = -999999f;
        }

        ans.tostring = tmp.data;
        return (ans);
    }

    public filedump Readitem(string name)
    {
        string[] tmp = File.ReadAllLines(saveFilePath);
        int save = 999999;

        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] == "[" + name + "]")
            {
                save = i;
            }
        }

        if (save != 999999)
        {
            righter.Close();

            filedump exist = new filedump(true, name, tmp[save + 1], save + 1, tmp);

            return (exist);
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
