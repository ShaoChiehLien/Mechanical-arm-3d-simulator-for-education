using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;

public class Client : MonoBehaviour
{
    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private String clientName = "LiensPC";

    public void ConnectToServer()
    {
        //If already connected, ignore this function
        if (socketReady)
            return;

        string host = "127.0.0.1";
        int port = 6321;

        //Overwrite default host/port values, if there is something in those boxes
        string h;
        int p;

        h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (h != "")
            host = h;

        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
            port = p;

        //Create the socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch(Exception e)
        {
            Debug.Log("Socket error:" + e.Message);
        } 

    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }

        }
    }

    public void OnSendButton()
    {
        string message = GameObject.Find("SendInput").GetComponent<InputField>().text;
        Send(message);
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;
        writer.WriteLine(data);
        writer.Flush();
    }

    private void OnIncomingData(string data)
    {
        if(data == "%NAME")
        {
            Send("&Name|" + clientName);
            return;
        }

        Debug.Log("Server: " + data);
    }

}
