using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;

public class datadump
{
    public datadump()
    {

    }

    public datadump(int pack, string ID, string msg, int curr, int itm1, int itm2, int itm3)
    {
        tpacketType = pack;
        tID = ID;
        tmessage = msg;
        tcurr = curr;
        titem1 = itm1;
        titem2 = itm2;
        titem3 = itm3;
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
}

public class packagetosend : MonoBehaviour
{
    const int BUFFERSIZE = 2048;

    public enum sendpackettypes
    {
        ACK,
        PACKAGESEND,
        PACKAGERECIVE,
    };

    public bool toPackage;

    private int port = 27100;
    private string IP = "45.32.245.198";
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

    public GameObject usertextbox;
    public GameObject usercurr;
    public GameObject useritem1;
    public GameObject useritem2;
    public GameObject useritem3;

    public static List<datadump> enemieDrops = new List<datadump>() { };

    void Start()
    {
        //GameObject[] enemylist = GameObject.FindGameObjectsWithTag("Enemy");

        //for (int i = 0; i < enemylist.Length; i++)
        //{
        //    send(2);
        //}

        send(sendpackettypes.PACKAGERECIVE);
    }

    void Update()
    {
        if (toPackage == true)
        {
            toPackage = false;
            send(ddpackettype);
        }
    }

    public void send(sendpackettypes type)
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
                    ddID = "STEAM_0:0:98612737"; //TODO replace with propper steamID
                    ddmessage = usertextbox.GetComponent<Text>().text;
                    ddcurr = convert(usercurr.GetComponent<Text>().text);
                    dditem1 = convert(useritem1.GetComponent<Text>().text);
                    dditem2 = convert(useritem2.GetComponent<Text>().text);
                    dditem3 = convert(useritem3.GetComponent<Text>().text);

                    package = new datadump((int)ddpackettype, ddID, ddmessage, ddcurr, dditem1, dditem2, dditem3);
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    package = new datadump((int)ddpackettype);
                    break;
                }
            default:
                {
                    Debug.Log("invalid packet type");
                    break;
                }
        }

        multipass tmp = new multipass(package, port, IP, client, data, stream);
        ThreadPool.QueueUserWorkItem(ThreadProc, tmp);
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
            default:
                {
                    Debug.Log("invalid packet type");
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
            default:
                {
                    Debug.Log("invalid packet type");
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
                    thestring += dump.titem3;
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    thestring += dump.tpacketType + "--";
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
}
