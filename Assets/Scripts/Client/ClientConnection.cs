using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

// Provides an interface to the server.
public class ClientConnection
{
    // For sending and recieving UDP packets
    private UdpClient udp = null;
    private IPEndPoint udpEndPoint;

    // Port and IP of the server
    private int socketPort = 9876;
    private string serverIP = "115.146.95.127";

    // The data from the last response
    // received from the server
    private byte[] receivedData;

    private static ClientConnection instance = new ClientConnection();

    // Singleton.
    private ClientConnection()
    {}

    // Get the client connection object.
    public static ClientConnection GetInstance()
    {
        return instance;     
    }

    // Send a message to the server.
    public void Send(string message)
    {
        if (udp != null)
        {
            udp.Close();
        }
        udpEndPoint = new IPEndPoint(IPAddress.Any, socketPort);
        udp = new UdpClient(udpEndPoint);
        byte[] data = Encoding.ASCII.GetBytes(message);
        udp.Send(data, data.Length, serverIP, socketPort);
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
        string serverResponse = response;
        Debug.Log(response);
        udp.Close();
    }

	// EndReceive Wrapper.
	public void EndReceive(IAsyncResult asyncResult)
	{
		byte[] data = udp.EndReceive (asyncResult, ref udpEndPoint);
		this.receivedData = data;
		udp.Close ();
	}

    // Call after calling the EndReceive method.
    // Gets the data from the last response received
    // from the server.
    public byte[] GetData()
	{
		return receivedData;
	}	
}
