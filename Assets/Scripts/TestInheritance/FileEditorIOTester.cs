using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadTester
{

    public static string file = "test.txt";

    public static void writeTest()
    {
        StreamWriter writer = new StreamWriter(file, true);
        
        for (int i = 0; i < 100; i++)
        {
            writer.WriteLine("Test");
        }

        writer.Close();

        Debug.Log($"Thread done: Wrote a lot of test lines");
    }

    public static void readTest()
    {
        //string[] a = File.ReadAllLines(file);
        List<string> lines = new List<string>();

        for (int i = 0; i < 100; i++)
        {
            lines.Clear();
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
                //foreach (var line in File.ReadAllLines(file))
                //{
                //    lines.Add(line);
                //}
                //a = File.ReadAllLines(file);
            }

        Debug.Log($"Thread done, {lines.Count} lines");
    }

    public static void Test()
    {
        Thread w = new Thread(new ThreadStart(writeTest));
        Thread r = new Thread(new ThreadStart(readTest));

        w.Start();
        r.Start();
    }
}

public class FileEditorIOTester : MonoBehaviour
{
    private void Start()
    {
        ThreadTester.Test();
    }
}
