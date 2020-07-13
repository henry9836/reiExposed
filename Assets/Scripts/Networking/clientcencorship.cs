using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

public class clientcencorship : MonoBehaviour
#if UNITY_EDITOR
, IPostprocessBuildWithReport
#endif
{
    private string saveFilePath = "sjw.txt";
    private StreamWriter righter;

    public List<string> dawords = new List<string>();

    void Awake()
    {
        if (FileExists(saveFilePath) == false)
        {
            StreamWriter tmp = new StreamWriter(saveFilePath, true);
            tmp.WriteLine();
            tmp.Close();
        }
        string[] graballlines = File.ReadAllLines(saveFilePath);
        for (int i = 0; i < graballlines.Length; i++)
        {
            Debug.Log($"{i}");
            dawords.Add(graballlines[i]);
        }
    }




    public string watchYourProfanity(string dump)
    {
        ThreadPool.QueueUserWorkItem(ThreadProc, tmp);

        Debug.Log("WATCH YOUR P{RRPRPRRIEOKWGJERGIJGREIEPJR LAG TMEI!");

        for (int j = 0; j < dawords.Count; j++)
        {
            string check = dawords[j];
            string newcheck = "";
            string checkto = "";



            for (int i = 0; i < check.Length; i++)
            {
                checkto += "▇";
            }

            for (int i = 0; i < check.Length - 1; i++)
            {
                newcheck += check[i] + "\\s*";
            }

            newcheck += check[check.Length - 1];

            string rxp = @"(?i)(" + newcheck + ")(.*?)";


            Regex rx = new Regex(rxp);
            MatchCollection ans = rx.Matches(dump);

            for (int i = ans.Count - 1; i > -1; i--)
            {
                dump = dump.Remove(ans[i].Index, ans[i].Length);
                dump = dump.Insert(ans[i].Index, checkto);
            }
        }
        return (dump);
    }

    static void ThreadProc(System.Object stateInfo)
    {

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
