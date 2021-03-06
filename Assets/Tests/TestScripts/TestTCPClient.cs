﻿using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System.IO;

public class TestTCPClient : MonoBehaviour
{
    public Text display;

    private TcpClient tcpClient;
    private IPEndPoint tcpEndPoint;

    private int socketPort = 9876;
    private string nectarIP = "115.146.95.127";

    void Start()
    {
        tcpEndPoint = new IPEndPoint(IPAddress.Any, socketPort);
        tcpClient = new TcpClient(tcpEndPoint);
        tcpClient.Connect(nectarIP, socketPort);

        NetworkStream networkStream = tcpClient.GetStream();
        StreamReader clientStreamReader = new StreamReader(networkStream);
        StreamWriter clientStreamWriter = new StreamWriter(networkStream);

        clientStreamWriter.WriteLine("hi");
        clientStreamWriter.Flush();
        string serverReply = clientStreamReader.ReadLine();
        Debug.Log(serverReply);
        display.text = serverReply;
    }
}
