using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    public int port = 6321;
    public TcpListener server;
    private bool serverStarted;
    public MainControl mainControlScript;
    public int byte_stored_pointer;

    private void Start()
    {
        byte_stored_pointer = 0;
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on port " + port.ToString());
        }
        catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
            return;
        foreach (ServerClient c in clients)
        {
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);

                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }
    }

    public void StopServer()
    {
        server.Stop();
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();

        Broadcast("%NAME", new List<ServerClient>() { clients[clients.Count - 1] });
    }

    private void OnIncomingData(ServerClient c, string data)
    {
        byte[] byteDATAS = new byte[data.Length / 2];
        StringToByteArray(data, byteDATAS);
        //
        for(int i = 0; i < byteDATAS.Length; i++)
        {
            mainControlScript.instructionsBytes[i + byte_stored_pointer] = byteDATAS[i];
        }
        byte_stored_pointer = byte_stored_pointer + byteDATAS.Length;

        Debug.Log(c.clientName + " has sent the following message :" + data);
    }

    //////////////////
    private void StringToByteArray(string data, byte[] byteDATAS)
    {
        byte[] tempDatas = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if(data[i] == 'A'|| data[i] == 'B'|| data[i] == 'C'|| data[i] == 'D'|| data[i] == 'E' || data[i] == 'F')
            {
                tempDatas[i] = (byte)(data[i] - 'A' + 10);
            }
            else
            {
                tempDatas[i] = (byte)(data[i] - '0');
            }
        }
        int j = 0;
        for (int i = 0; i < data.Length; i = i+2)
        {
            byteDATAS[j] = (byte)(tempDatas[i] * 16 + tempDatas[i + 1]);
            j++;
        }
    }



    //////////////////

    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach(ServerClient c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception e)
            {
                Debug.Log("Write error: " + e.Message + "to client" + c.clientName);
            }
        }
    }

}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;


    }
}
