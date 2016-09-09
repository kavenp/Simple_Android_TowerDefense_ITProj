﻿using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

// Derived from
// http://pcrelated.net/index.php/csharp-send-and-receive-data-using-udpclient/
public class ConnectToCustomServer : MonoBehaviour
{

    static UdpClient udp;
    static IPEndPoint udpEndPoint;

    private int socketPort = 9876;
    private string nectarIP = "115.146.95.127";

    void Start()
    {
        string welcome = "Hello2";
        byte[] data = Encoding.ASCII.GetBytes(welcome);

        udpEndPoint = new IPEndPoint(IPAddress.Any, socketPort);
        udp = new UdpClient(udpEndPoint);

        // Send the data
        udp.Send(data, data.Length, nectarIP, socketPort);

        // Begin waiting for a response asynchronously
        udp.BeginReceive(new AsyncCallback(UDP_IncomingData), udpEndPoint);
    }

    static void UDP_IncomingData(IAsyncResult asyncResult)
    {
        byte[] data = udp.EndReceive(asyncResult, ref udpEndPoint);

        string response = Encoding.UTF8.GetString(data);
        Debug.Log(response);

        udp.Close();
    }
}
