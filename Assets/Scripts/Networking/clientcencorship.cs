using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

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

    void Awake()
    {
        if (FileExists(saveFilePath) == false)
        {
            StreamWriter tmp = new StreamWriter(saveFilePath, true);
            tmp.WriteLine();
            tmp.Close();
        }
    }

    void Start()
    {
        string tmp = watchYourProfanity("The the fu Ckquick brown fox fox jumps ̀ ́ ̂ ̃ ̄ ̅ ̆ ̇ ̈ ̉ ̊Блять вы, сука! ̣ ̤ ̥ ̦ ̧ ̨ ̫ ̬ ̭ ̮ ̯ ̰ ̱ ̲ ̳ ̴ ̵ ̶ ̷ ̸ ̹ ̺ ̻ ̼ ̽ ̾ ̿ ̀ ́ ͂ ̓ ̈́ ͅ ͆ ͇ ͈ ͉ ͊ ͋ ͌ ͍ ͎ ͏ ͠ ͡ ͢ ͣ ͤ ͥ ͦ ͧ ͨ ͩ ͪ ͫ ͬ ͭ ͮ ͯ the lazy fu ck Fuck dog dog.");
        Debug.Log(tmp);
    }

    public string watchYourProfanity(string dump)
    {

        for (int j = 0; j < 2; j++)
        {
            string check = "fuck";
            string newcheck = "";
            string checkto = "";

            if (j == 1)
            {
                check = "the";
            }

            for (int i = 0; i < check.Length; i++)
            {
                checkto += "*";
            }

            for (int i = 0; i < check.Length - 1; i++)
            {
                newcheck += check[i] + "\\s*";
            }

            newcheck += check[check.Length - 1];

            string rxp = @"(?i)(" + newcheck + ")(.*?)";

            Debug.Log(rxp);

            Regex rx = new Regex(rxp);
            MatchCollection ans = rx.Matches(dump);

            for (int i = ans.Count - 1; i > -1; i--)
            {
                //Debug.Log(ans[i].Index + " " + ans[i].Length + " " + ans[i].Value);
                dump = dump.Remove(ans[i].Index, ans[i].Length);
                dump = dump.Insert(ans[i].Index, checkto);
            }
        }
        return (dump);
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
