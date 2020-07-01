using System.Collections;
using System.Collections.Generic;
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

public class packagetosend : MonoBehaviour
{
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

    public int ddpackettype;
    public string ddID;
    public string ddmessage;
    public int ddcurr;
    public int dditem1;
    public int dditem2;
    public int dditem3;


    private void Start()
    {
        ddpackettype = 2;
    }

    public void Update()
    {
        if (toPackage == true)
        {
            toPackage = false;
            datadump tmp = new datadump(ddpackettype, ddID, ddmessage, ddcurr, dditem1, dditem2, dditem3);

            send(tmp);
        }
    }


    public void send(datadump package)
    {
        client = new TcpClient(IP, port);
        string pack = encoder(package);
        data = System.Text.Encoding.ASCII.GetBytes(pack);
        stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        Debug.Log("sent: " + pack);
        data = new byte[256];
        string responcedata = string.Empty;
        int bytes = stream.Read(data, 0, data.Length);
        responcedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        datadump tmp = decoder(responcedata);
        Debug.Log("type:" + tmp.tpacketType + " ID:" + tmp.tID + " msg:" + tmp.tmessage + " curr:" + tmp.tcurr + " itm1:" + tmp.titem1 + " itm2:" + tmp.titem2 + " itm3:" + tmp.titem3);

        stream.Close();
        client.Close();
    }

    public datadump decoder(string responce)
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
                    thedata.tmessage = resp.Substring(0, resp.IndexOf("--"));
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

    public string encoder(datadump dump)
    {
        string thestring = "";

        switch ((sendpackettypes)dump.tpacketType)
        {
            case sendpackettypes.ACK:
                {
                    thestring += "0" + "--";

                    //placeholder
                    thestring += "1";
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
