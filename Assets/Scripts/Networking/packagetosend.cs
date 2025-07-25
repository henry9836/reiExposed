﻿using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class datadump
{
    public datadump()
    {

    }

    public datadump(int pack, string ID, string msg, int curr, int itm1, int itm2, int itm3, string nam, string tim, string has)
    {
        tpacketType = pack;
        tID = ID;
        tmessage = msg;
        tcurr = curr;
        titem1 = itm1;
        titem2 = itm2;
        titem3 = itm3;
        titem3 = itm3;
        titem3 = itm3;
        tName = nam;
        tTime = tim;
        tHash = has;
    }

    public datadump(int pack, string msg)
    {
        tpacketType = pack;
        tmessage = msg;
    }

    public datadump(int pack)
    {
        tpacketType = pack;
    }

    public int tpacketType;
    public string tID;
    public string tmessage;
    public int tcurr;
    public int titem1;
    public int titem2;
    public int titem3;
    public string tName;
    public string tTime;
    public string tHash;
}

public class multipass
{
    public multipass()
    {

    }

    public multipass(datadump package, int mpport, string mpip, TcpClient mpclient, byte[] mpdata, NetworkStream mpstream)
    {
        mpdatadump = package;
        port = mpport;
        IP = mpip;
        client = mpclient;
        data = mpdata;
        stream = mpstream;

        ddpackettype = mpdatadump.tpacketType;
        ddID = mpdatadump.tID;
        ddmessage = mpdatadump.tmessage;
        ddcurr = mpdatadump.tcurr;
        dditem1 = mpdatadump.titem1;
        dditem2 = mpdatadump.titem2;
        dditem3 = mpdatadump.titem3;
        ddName = mpdatadump.tName;
        ddTime = mpdatadump.tTime;
        ddHash = mpdatadump.tHash;
    }

    public datadump mpdatadump;

    public int port;
    public string IP;
    public TcpClient client;
    public byte[] data;
    public NetworkStream stream;

    public int ddpackettype;
    public string ddID;
    public string ddmessage;
    public int ddcurr;
    public int dditem1;
    public int dditem2;
    public int dditem3;
    public string ddName;
    public string ddTime;
    public string ddHash;

}

public class packagetosend : MonoBehaviour
{
    const int BUFFERSIZE = 8192; //4*2048 (UTF-8 = 4 bytes each char)

    public enum sendpackettypes
    {
        ACK,
        PACKAGESEND,
        PACKAGERECIVE,
        REQUESTLEADERBOARD,
        REQUESTUSERRANK
    };

    public bool toPackage;

    private int port = 27100;
    private string IP = "reiexposed.ddns.net";
    private TcpClient client;
    byte[] data;
    private NetworkStream stream;

    public sendpackettypes ddpackettype;
    public string ddID;
    public string ddmessage;
    public int ddcurr;
    public int dditem1;
    public int dditem2;
    public int dditem3;
    public string ddname;
    public string ddtime; //String in seconds
    public string ddhash; //Score Hash

    public GameObject usertextbox;
    public GameObject usercurr;
    public GameObject useritem1;
    public GameObject useritem2;
    public GameObject useritem3;
    public GameObject TwitterInput;

    public Text debugText;

    public static List<datadump> enemieDrops = new List<datadump>() { };

    [Header("Network Messages Debug")]
    public bool debugMessages = true;
    private float timerThing = 0.0f;
    private int thingy = 0;

    public GameObject leaderboard;

    void Start()
    {
		LoadConfig();

        GameObject[] enemylist = GameObject.FindGameObjectsWithTag("Myth");

        for (int i = 0; i < enemylist.Length; i++)
        {
            send(sendpackettypes.PACKAGERECIVE);
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            send(sendpackettypes.REQUESTLEADERBOARD);
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            send(sendpackettypes.REQUESTUSERRANK);
        }
#endif

        if (toPackage == true)
        {
            toPackage = false;
            send(ddpackettype);
        }

        timerThing += Time.deltaTime;
        //FOR SOME REASON WE CANNOT HAVE QR CODE WORKING UNLESS WE DO THIS CHECK
        if (debugMessages)
        {
            debugText.enabled = true;
            if (timerThing > 3.0f)
            {
                timerThing = 0.0f;
                if (enemieDrops.Count > 0)
                {
                    if (enemieDrops[thingy] != null)
                    {
                        debugText.text = enemieDrops.Count.ToString() + ":" + enemieDrops[thingy].tmessage + ":" + thingy.ToString();
                        //debugText.text = ""; //Things are going well
                    }
                    else
                    {
                        debugText.text = enemieDrops.Count.ToString() + ":NULL:" + thingy.ToString();
                        //debugText.text = "";
                    }
                }
                else
                {
                    debugText.text = enemieDrops.Count.ToString() + ":EMPTY:" + thingy.ToString();
                    //debugText.text = "";
                }

                thingy++;
                if (thingy >= enemieDrops.Count)
                {
                    thingy = 0;
                }
            }
        }
        else
        {
            debugText.enabled = false;
        }
    }

	void LoadConfig()
	{
		string configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.txt");

		if (File.Exists(configPath))
        {
            try
            {
                string[] lines = File.ReadAllLines(configPath);
                foreach (string line in lines)
                {
                    if (line.StartsWith("IP="))
                    {
                        IP = line.Substring(3);
                    }
                    else if (line.StartsWith("PORT="))
                    {
                        if (int.TryParse(line.Substring(5), out int parsedPort))
                        {
                            port = parsedPort;
                        }
                    }
                }
                Debug.Log($"Config loaded - IP: {IP}, Port: {port}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading config: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"Config file not found at {configPath}, using defaults");
        }
	}

    public void submitScore(string name, string time)
    {

    }

    public void send(int type) { send((sendpackettypes)type); }

    public void send(sendpackettypes type) { send(type, "NA"); }
    public void send(sendpackettypes type, string sendoffset)
    {
        datadump package = new datadump();
        ddpackettype = type;

        switch (ddpackettype)
        {
            case sendpackettypes.ACK:
                {
                    if (ddmessage == "")
                    {
                        ddmessage = "ping";
                    }

                    package = new datadump((int)ddpackettype, ddmessage);
                    break;
                }
            case sendpackettypes.PACKAGESEND:
                {
                    package = new datadump((int)ddpackettype, ddID, ddmessage, ddcurr, dditem1, dditem2, dditem3, ddname, ddtime, ddhash);
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    package = new datadump((int)ddpackettype);
                    break;
                }
            case sendpackettypes.REQUESTLEADERBOARD:
                {
                    string ddchunkSize = "10";
                    string offsetFromStart = sendoffset;
                    package = new datadump((int)ddpackettype, ddchunkSize + "--" + offsetFromStart);
                    break;
                }
            case sendpackettypes.REQUESTUSERRANK:
                {
                    string name = SaveSystemController.getValue("Package_Name");
                    if (name == "-1")
                    {
                        name = "Anon";
                    }
                    string dduserName = name;
                    package = new datadump((int)ddpackettype, dduserName);
                    break;
                }
            default:
                {
                    Debug.Log($"invalid packet type {type.ToString()}");
                    break;
                }
        }

        multipass tmp = new multipass(package, port, IP, client, data, stream);
        ThreadPool.QueueUserWorkItem(ThreadProc, tmp);

        //if (tmp.ddpackettype == 1) {
        //    StartCoroutine(returnToMainTmp());
        //    TwitterInput.SetActive(false);
        //}
    }
    

    static void ThreadProc(System.Object stateInfo)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        multipass mp = stateInfo as multipass;
        string pack = encoder(mp.mpdatadump);
        mp.client = new TcpClient(mp.IP, mp.port);
        mp.data = utf8.GetBytes(pack);
        mp.stream = mp.client.GetStream();
        mp.stream.Write(mp.data, 0, mp.data.Length);
        Debug.Log("sent: " + pack);
        mp.data = new byte[BUFFERSIZE];
        string responcedata = string.Empty;
        int bytes = mp.stream.Read(mp.data, 0, mp.data.Length);
        responcedata = utf8.GetString(mp.data);
        datadump tmp = decoder(responcedata);

        switch ((sendpackettypes)tmp.tpacketType)
        {
            case sendpackettypes.ACK:
                {
                    Debug.Log("Recieved: " + ((sendpackettypes)tmp.tpacketType).ToString() + " msg:" + tmp.tmessage);
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    Debug.Log("Recieved:" + ((sendpackettypes)tmp.tpacketType).ToString() + " ID:" + tmp.tID + " msg:" + tmp.tmessage + " curr:" + tmp.tcurr + " itm1:" + tmp.titem1 + " itm2:" + tmp.titem2 + " itm3:" + tmp.titem3);
                    enemieDrops.Add(tmp);
                    break;
                }
            case sendpackettypes.REQUESTLEADERBOARD:
                {
                    List<string> decoding = new List<string>() { };
                    List<string> decodingsub = new List<string>() { };

                    tmp.tmessage += "--";

                    for (int i = 0; i < 10; i++)
                    {

                        if (tmp.tmessage.IndexOf("--") == -1)
                        {
                            decoding.Add(tmp.tmessage);
                            break;
                        }
                        else
                        {
                            decoding.Add(tmp.tmessage.Substring(0, tmp.tmessage.IndexOf("--")));
                            tmp.tmessage = tmp.tmessage.Substring(tmp.tmessage.IndexOf("--") + 2);
                        }
                    }

                    for (int j = 0; j < 10; j++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (decoding[j].IndexOf("#") == -1)
                            {
                                decodingsub.Add(decoding[j]);
                                break;
                            }
                            else
                            {
                                decodingsub.Add(decoding[j].Substring(0, decoding[j].IndexOf("#")));
                                decoding[j] = decoding[j].Substring(decoding[j].IndexOf("#") + 1);
                            }
                        }

                        resizeHolder.listofLeaderboard.Add(new leader(decodingsub[0], decodingsub[2], decodingsub[1]));
                        decodingsub = new List<string>() { };
                    }
                    break;
                }
            case sendpackettypes.REQUESTUSERRANK:
                {
                    List<string> decoding = new List<string>() { };
                    tmp.tmessage += "--";

                    for (int i = 0; i < 3; i++)
                    {
                        if (tmp.tmessage.IndexOf("--") == -1)
                        {
                            decoding.Add(tmp.tmessage);
                            break;
                        }
                        else
                        {
                            decoding.Add(tmp.tmessage.Substring(0, tmp.tmessage.IndexOf("--")));
                            tmp.tmessage = tmp.tmessage.Substring(tmp.tmessage.IndexOf("--") + 2);
                        }
                    }

                    personalScore.PersonallistofLeaderboard.Add(new leader(decoding[0], decoding[2], decoding[1]));
                    break;
                }
            default:
                {
                    Debug.Log($"Invalid packet type {tmp.tpacketType} | {tmp.tmessage}");
                    break;
                }
        }

        mp.stream.Close();
        mp.client.Close();
    }

    public static datadump decoder(string responce)
    {
        List<string> decoding = new List<string>() { };
        string resp = responce;
        datadump thedata = new datadump();
        //Debug.Log("Decoding: " + resp);
        thedata.tpacketType = int.Parse(resp.Substring(0, 1));
        resp = resp.Substring(3);

        switch ((sendpackettypes) thedata.tpacketType)
        {
            case sendpackettypes.ACK:
                {
                    thedata.tmessage = resp;
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {

                    for (int i = 0; i < 5; i++)
                    {
                        //Error Handling
                        if (resp.IndexOf("--") == -1)
                        {
                            decoding.Add(resp);
                            break;
                        }
                        else
                        {
                            decoding.Add(resp.Substring(0, resp.IndexOf("--")));
                            resp = resp.Substring(resp.IndexOf("--") + 2);
                        }
                    }

                    thedata.tID = decoding[0];
                    thedata.tmessage = decoding[1];
                    thedata.tcurr = int.Parse(decoding[2]);
                    thedata.titem1 = int.Parse(decoding[3]);
                    thedata.titem2 = int.Parse(decoding[4]);
                    thedata.titem3 = int.Parse(resp);

                    break;
                }
            case sendpackettypes.REQUESTLEADERBOARD:
                {
                    thedata.tmessage = resp;
                    break;
                }
            case sendpackettypes.REQUESTUSERRANK:
                {
                    thedata.tmessage = resp;
                    break;
                }
            default:
                {
                    Debug.Log($"Invalid packet type {thedata.tpacketType} | {resp}");
                    break;
                }
        }

        return (thedata);
    }

    public static string encoder(datadump dump)
    {
        string thestring = "";

        switch ((sendpackettypes)dump.tpacketType)
        {
            case sendpackettypes.ACK:
                {
                    thestring += "0" + "--";
                    thestring += dump.tmessage;
                    break;
                }
            case sendpackettypes.PACKAGESEND:
                {
                    thestring += dump.tpacketType + "--";
                    thestring += dump.tID + "--";
                    thestring += dump.tmessage + "--";
                    thestring += dump.tcurr + "--";
                    thestring += dump.titem1 + "--";
                    thestring += dump.titem2 + "--";
                    thestring += dump.titem3 + "--";
                    thestring += dump.tName + "--";
                    thestring += dump.tTime + "--";
                    thestring += dump.tHash;
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    thestring += dump.tpacketType + "--";
                    break;
                }
            case sendpackettypes.REQUESTUSERRANK:
            case sendpackettypes.REQUESTLEADERBOARD:
                {
                    thestring += dump.tpacketType + "--" + dump.tmessage;
                    break;
                }
            default:
                {
                    Debug.Log("error type");
                    break;
                }
        }

        return (thestring);
    }

    public int convert(string input)
    {
        int number;
        bool test = int.TryParse(input, out number);

        if (test)
        {
            return (number);
        }
        else
        {
            return (0);
        }
    }

    IEnumerator returnToMainTmp()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }


}
