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

public static class filterInfo {
    [HideInInspector]
    public static bool threadProcessing = false;
}

public class passthrough
{
    public List<string> dawords;
    public string dump;
}

public class clientcencorship : MonoBehaviour
#if UNITY_EDITOR
, IPostprocessBuildWithReport
#endif
{
    private string saveFilePath = "sjw.txt";
    private StreamWriter righter;

    public static List<string> dawords = new List<string>();
    public static List<string> messages = new List<string>();

    private passthrough currDump = null;
    private bool processLock = false;

    public int getMessageCount()
    {
        return messages.Count;
    }

    public string getMessageAndRemove(int i)
    {
        Debug.Log($"Message List Size: {messages.Count}");
        string msg = messages[i];
        messages.RemoveAt(i);
        return msg;
    }

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
            dawords.Add(graballlines[i]);
        }
    }

    public IEnumerator watchYourProfanity(string dump)
    {
        //Wait for another IEnumator
        while (processLock)
        {
            yield return null;
        }

        //Lock this IEnumator
        processLock = true;

        //Wait till other thread is done
        while (filterInfo.threadProcessing)
        {
            yield return null;
        }

        //spawn thread
        currDump = new passthrough();
        currDump.dawords = dawords;
        currDump.dump = dump;
        ThreadPool.QueueUserWorkItem(ThreadProc, currDump);

        //Unlock IEnumator
        processLock = false;
        yield return null;
    }

    static void ThreadProc(System.Object stateInfo)
    {
        //Lock Thread
        filterInfo.threadProcessing = true;

        passthrough PT = stateInfo as passthrough;


        for (int j = 0; j < PT.dawords.Count; j++)
        {
            string check = PT.dawords[j];
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
            MatchCollection ans = rx.Matches(PT.dump);

            for (int i = ans.Count - 1; i > -1; i--)
            {
                PT.dump = PT.dump.Remove(ans[i].Index, ans[i].Length);
                PT.dump = PT.dump.Insert(ans[i].Index, checkto);
            }
        }

        //Append onto list
        messages.Add(PT.dump);

        //Unlock Thread
        filterInfo.threadProcessing = false;
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
