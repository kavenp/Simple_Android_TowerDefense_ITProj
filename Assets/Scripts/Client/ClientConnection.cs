using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

// Provides an interface to the NeCTAR server.
public class ClientConnection
{

    private UdpClient udp;
    private IPEndPoint udpEndPoint;

    private int socketPort = 9876;
    private string nectarIP = "115.146.95.127";

    private static ClientConnection instance = new ClientConnection();

    // Singleton.
    private ClientConnection()
    {}

    // Get client connection object.
    public static ClientConnection GetInstance()
    {
        return instance;     
    }

    // Send message to the server.
    public void Send(string message)
    {
        udpEndPoint = new IPEndPoint(IPAddress.Any, socketPort);
        udp = new UdpClient(udpEndPoint);
        byte[] data = Encoding.ASCII.GetBytes(message);
        udp.Send(data, data.Length, nectarIP, socketPort);
    }

    // Wrapper for UdpClient's BeginReceive method.
    public void BeginReceive(AsyncCallback callback)
    {
        udp.BeginReceive(callback, udpEndPoint);
    }

    // Waits for a response asynchronously, then logs
    // the response for debugging purposes.
    public void DebugIncomingData(IAsyncResult asyncResult)
    {
        byte[] data = udp.EndReceive(asyncResult, ref udpEndPoint);

        string response = Encoding.UTF8.GetString(data);
        Debug.Log(response);

        udp.Close();
    }
}
