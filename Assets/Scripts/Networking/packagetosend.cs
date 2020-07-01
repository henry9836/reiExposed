using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class packagetosend : MonoBehaviour
{

    public bool toPackage;
    public string thePackage;

    public string division = "##";
    public string message;
    public string item; //replace properly in future


    private int port = 27100;
    private string IP = "45.32.245.198";
    private TcpClient client;
    byte[] data;
    private NetworkStream stream;

    public void Update()
    {
        if (toPackage == true)
        {
            toPackage = false;
            thePackage = "";
            thePackage += message + division;
            thePackage += item;
            send();
        }
    }


    public void send()
    {
        client = new TcpClient(IP, port);
        data = System.Text.Encoding.ASCII.GetBytes(thePackage);
        stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        Debug.Log("sent: " + thePackage);
        data = new byte[256];
        string responcedata = string.Empty;
        int bytes = stream.Read(data, 0, data.Length);
        responcedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        Debug.Log("recived: " + responcedata);

        stream.Close();
        client.Close();
    }

    public void recive()
    {
        client = new TcpClient(IP, port);
        data = System.Text.Encoding.ASCII.GetBytes("call");
        stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        Debug.Log("sent: " + "call");   
        data = new byte[256];
        string responcedata = string.Empty;
        int bytes = stream.Read(data, 0, data.Length);
        responcedata = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        Debug.Log("recived: " + responcedata);

        stream.Close();
        client.Close();
    }

}
