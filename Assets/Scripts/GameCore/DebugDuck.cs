using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugDuck : MonoBehaviour
{
    public Text output;
    public Text input;
    public Text inputPlaceholder;

    //This script is only to be used for developers and dev builds never for release builds
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private void print(string line)
    {
        output.text += line + "\n";
    }

    private void execute()
    {
        if (input.text == "clear")
        {
            output.text = "";
        }
        else if (input.text == "help")
        {
            print(" [HELP]\n" +
                "clear - clears the screen\n" +
                "getInfo - displays useful info\n" +
                "getInfoFull - displays more useful info\n" +
                "getHash - displays calculated hash\n" +
                "setName - sets name\n" +
                "setTime - sets time\n" +
                "setCurr - sets currency\n" +
                "setMsg - sets message\n" +
                "setMoney - sets amount of money\n" +
                "setItem1 - sets item1\n" +
                "setItem2 - sets item2\n" +
                "setItem3 - sets item3\n" +
                "updatePackageH - updates the package hash\n" + 
                "save - saves to disk\n" +
                "test - emulate a anti-cheat check and get return\n" +
                "exit - soft relaunch\n" +
                "quit - close game"
                );
        }
        else if (input.text == "getInfoFull")
        {

            print($"CURR: {SaveSystemController.getValue("MythTraces")}\n" +
                $"SHOTD: {SaveSystemController.getValue("shotgunDamageLVL")}\n" +
                $"SHOTR: {SaveSystemController.getValue("shotgunRangeLVL")}\n" +
                $"SHOTSA: {SaveSystemController.getValue("shotgunBulletSpreadADSLVL")}\n" +
                $"SHOTSR: {SaveSystemController.getValue("shotgunBulletSpreadRunningLVL")}\n" +
                $"DMG: {SaveSystemController.getValue("meeleeDamageLVL")}\n" +
                $"A: {SaveSystemController.getValue("ammo")}\n" +
                $"A2: {SaveSystemController.getValue("ammoTwo")}\n" +
                $"pP: {SaveSystemController.getValue("PackagePending")}\n" +
                $"pN: {SaveSystemController.getValue("Package_Name")}\n" +
                $"pID: {SaveSystemController.getValue("Package_STEAM_ID")}\n" +
                $"pMSG: {SaveSystemController.getValue("Package_Message")}\n" +
                $"pITM1: {SaveSystemController.getValue("Package_Item1")}\n" +
                $"pITM2: {SaveSystemController.getValue("Package_Item2")}\n" +
                $"pITM3: {SaveSystemController.getValue("Package_Item3")}\n" +
                $"pTIME: {SaveSystemController.getValue("Package_Time")}\n" +
                $"pH: {SaveSystemController.getValue("Package_MAGIC")}" +
                $"");
        }
        else if (input.text == "getInfo")
        {
            print($"H: {SaveSystemController.getValue("THEBIGONE")}\ncH: {SaveSystemController.calcCurrentHash()}\nTime: {SaveSystemController.getValue("MythTraces")}\nCURR: {SaveSystemController.getValue("MythTraces")}\npNAME: {SaveSystemController.getValue("Package_Name")}\npTIME: {SaveSystemController.getValue("Package_Time")}\npCURR: {SaveSystemController.getValue("Package_Curr")}\npH: {SaveSystemController.getValue("Package_MAGIC")}\npI1: {SaveSystemController.getValue("Package_Item1")}\npI2: {SaveSystemController.getValue("Package_Item2")}\npI3: {SaveSystemController.getValue("Package_Item3")}");
        }
        else if(input.text == "getHash")
        {
            print($"cH: {SaveSystemController.calcCurrentHash()}");
        }
        else if (input.text == "updatePackageH")
        {
            SaveSystemController.updateValue("Package_MAGIC", (SaveSystemController.calcCurrentHash(SaveSystemController.getValue("Package_Name") + SaveSystemController.getValue("Package_Time") + SaveSystemController.getValue("Package_Curr") + SaveSystemController.getValue("Package_Message") + SaveSystemController.getValue("Package_Item1") + SaveSystemController.getValue("Package_Item2") + SaveSystemController.getValue("Package_Item3")).ToString()), true);
        }
        else if (input.text == "quit")
        {
            Application.Quit();
        }
        else if (input.text == "test")
        {
            print($"H: {SaveSystemController.getValue("THEBIGONE")}");
            print($"cH: {SaveSystemController.calcCurrentHash()}");

            if (!SaveSystemController.checkSaveValid())
            {
                //CHEATS!!!!
                print("CHEATER DETECTED!!!");
            }
            else
            {
                print("Save File Is Unmodified");
            }
        }
        else if (input.text == "exit")
        {
            SceneManager.LoadScene(0);
        }
        else if (input.text == "save")
        {
            SaveSystemController.saveDataToDisk(true);
        }
        else if (input.text.Contains("set"))
        {
            SaveSystemController.updateValue("PackagePending", true);
            SaveSystemController.updateValue("Package_STEAM_ID", "EX_STEAM_0:0:98612737", true);
            if (input.text.Contains("setName"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Name", tmp, true);
                print($"Set Package_Name to {SaveSystemController.getValue("Package_Name")}");
            }
            else if (input.text.Contains("setTime"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Time", float.Parse(tmp));
                print($"Set Package_Time to {SaveSystemController.getValue("Package_Time")}");
            }
            else if (input.text.Contains("setMoney"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("MythTraces", int.Parse(tmp));
                print($"Set MythTraces to {SaveSystemController.getValue("MythTraces")}");
            }
            else if (input.text.Contains("setCurr"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Curr", int.Parse(tmp));
                print($"Set Package_Curr to {SaveSystemController.getValue("Package_Curr")}");
            }
            else if (input.text.Contains("setMsg"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Message", tmp, true);
                print($"Set Package_Message to {SaveSystemController.getValue("Package_Message")}");
            }
            else if (input.text.Contains("setItem1"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Item1", int.Parse(tmp));
                print($"Set Package_Item1 to {SaveSystemController.getValue("Package_Item1")}");
            }
            else if (input.text.Contains("setItem2"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Item2", int.Parse(tmp));
                print($"Set Package_Item2 to {SaveSystemController.getValue("Package_Item2")}");
            }
            else if (input.text.Contains("setItem3"))
            {
                string tmp = input.text.Substring(input.text.IndexOf(" ") + 1);
                SaveSystemController.updateValue("Package_Item3", int.Parse(tmp));
                print($"Set Package_Item3 to {SaveSystemController.getValue("Package_Item3")}");
            }
            else
            {
                print("Unknown command");
            }
        }
        else
        {
            print("Unknown command");
        }


        //Clear input
        input.text = "";
        inputPlaceholder.text = "";
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        print("Ready");
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input.text.Length > 0)
            {
                execute();
            }
        }
    }

#else
    void Start(){
        print("Not developer build, exitting...");
        StartCoroutine(leave());
    }

    IEnumerator leave()
    {
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }

#endif
}
