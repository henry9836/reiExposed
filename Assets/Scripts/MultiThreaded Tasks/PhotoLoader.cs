using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class PhotoLoader
{
    public class ImageContainer
    {
        public Byte[] imgBytes;
        public int file = 0;
        public bool pending = true;
    }
    // This thread procedure performs the task.
    public static void ThreadProc(System.Object stateInfo)
    {
        //Import List
        ImageContainer img = stateInfo as ImageContainer;

        //Build path
        string name = img.file.ToString() + ".png";
        string foldername = "shhhhhSecretFolder";

        string path = "";

        #if UNITY_STANDALONE_LINUX
                path = Directory.GetCurrentDirectory() + "/" + foldername +"/";
        #endif

        #if UNITY_STANDALONE_WIN
                path = Directory.GetCurrentDirectory() + "\\" + foldername + "\\";
        #endif

        #if UNITY_EDITOR
                path = Directory.GetCurrentDirectory() + "\\" + foldername + "\\";
        #endif
        string fullFilename = path + name;

        //Read file into byte array in DXT1 format
        img.imgBytes = File.ReadAllBytes(fullFilename);

        //Mark as done
        img.pending = false;
        // No state object was passed to QueueUserWorkItem, so stateInfo is null.
        Debug.Log($"Created a byte array {img.imgBytes.Length }");
    }
}
