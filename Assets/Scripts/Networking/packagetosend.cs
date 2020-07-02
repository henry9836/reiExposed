using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Net.Sockets;

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

    public multipass(datadump package, int mpport, string mpip, TcpClient mpclient, byte[] mpdata, NetworkStream mpstream, int mppackettype, string mpID, string mpmessage, int mpcurr, int mpitem1, int mpitem2, int mpitem3)
    {
        mpdatadump = package;
        port = mpport;
        IP = mpip;
        client = mpclient;
        data = mpdata;
        stream = mpstream;

        ddpackettype = mppackettype;
        ddID = mpID;
        ddmessage = mpmessage;
        ddcurr = mpcurr;
        dditem1 = mpitem1;
        dditem2 = mpitem2;
        dditem3 = mpitem3;
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

    public void Update()
    {
        if (toPackage == true)
        {
            toPackage = false;
            datadump tmp = new datadump((int)ddpackettype, ddID, ddmessage, ddcurr, dditem1, dditem2, dditem3);
            send(tmp);
        }
    }

    public void send(datadump package)
    {
        multipass tmp = new multipass(package, port, IP, client, data, stream, (int)ddpackettype, ddID, ddmessage, ddcurr, dditem1, dditem2, dditem3);
        ThreadPool.QueueUserWorkItem(ThreadProc, tmp);
    }
    

    static void ThreadProc(System.Object stateInfo)
    {
        multipass mp = stateInfo as multipass;
        string pack = encoder(mp.mpdatadump);
        mp.client = new TcpClient(mp.IP, mp.port);
        mp.data = System.Text.Encoding.ASCII.GetBytes(pack);
        mp.stream = mp.client.GetStream();
        mp.stream.Write(mp.data, 0, mp.data.Length);
        Debug.Log("sent: " + pack);
        mp.data = new byte[BUFFERSIZE];
        string responcedata = string.Empty;
        int bytes = mp.stream.Read(mp.data, 0, mp.data.Length);
        responcedata = System.Text.Encoding.ASCII.GetString(mp.data, 0, bytes);
        datadump tmp = decoder(responcedata);

        switch ((sendpackettypes)tmp.tpacketType)
        {
            case sendpackettypes.ACK:
                {
                    Debug.Log("type: " + tmp.tpacketType + " msg:" + tmp.tmessage);
                    break;
                }
            case sendpackettypes.PACKAGERECIVE:
                {
                    Debug.Log("type:" + tmp.tpacketType + " ID:" + tmp.tID + " msg:" + tmp.tmessage + " curr:" + tmp.tcurr + " itm1:" + tmp.titem1 + " itm2:" + tmp.titem2 + " itm3:" + tmp.titem3);
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
                        decoding.Add(resp.Substring(0, resp.IndexOf("--")));
                        resp = resp.Substring(resp.IndexOf("--") + 2);
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

                    //placeholder
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
}
