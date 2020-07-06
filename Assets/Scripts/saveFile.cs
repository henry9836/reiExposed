using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Rendering.HighDefinition;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif
public class saveFile : MonoBehaviour
#if UNITY_EDITOR
, IPostprocessBuildWithReport
#endif

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

    void Awake()
    {
        if (FileExists(saveFilePath) == false)
        {
            StreamWriter tmp = new StreamWriter(saveFilePath, true);
            tmp.WriteLine();
            tmp.Close();
        }
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
            //Debug.Log("created");
        }
        else
        {
            if (info.data != item)
            {
                righter = new StreamWriter(saveFilePath, false);

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

                //Debug.Log("overwritten");
            }
            else
            {
                //Debug.Log("already exists");
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
                //Debug.LogError("Failed to save >" + name +  "< of type " + type);
            }
        }
        else if (type == types.FLOAT)
        {
            if (test.tofloat == -999999f)
            {
                //Debug.LogError("Failed to save >" + name + "< of type " + type);
            }
        }

        return (test);
    }

    private interpret GetItem(string name)
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

    private filedump Readitem(string name)
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
            filedump exist = new filedump(true, name, tmp[save + 1], save + 1, tmp);
            return (exist);
        }

        filedump nonexistant = new filedump();
        nonexistant.exists = false;
        return (nonexistant);
    }

    public bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public int callbackOrder { get { return 0; } }


#if UNITY_EDITOR

    public void OnPostprocessBuild(BuildReport report)
    {
        string path = report.summary.outputPath;
        int cutpos = 0;

        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] == '/')
            {
                cutpos = i;
            }
        }

        path = path.Substring(0, cutpos + 1);

        if (FileExists(path + saveFilePath))
        {
            File.Delete(path + saveFilePath);
        }

        File.Copy(saveFilePath, path + saveFilePath);
    }
#endif

}
