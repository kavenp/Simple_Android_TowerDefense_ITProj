using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

// Provides an interface to the server.
public class ClientConnection
{
    // Port and IP of the server
    private int socketPort = 9876;
    private string serverIP = "115.146.95.127";

	private IPEndPoint target;
	private UdpClient server;
	private UdpClient socket;

    // The data from the last response
    // received from the server
    private byte[] receivedData;

    private static ClientConnection instance = new ClientConnection();

    // Singleton.
    private ClientConnection()
	{
		//socket = new UdpClient (socketPort);
	}

    // Get the client connection object.
    public static ClientConnection GetInstance()
    {
        return instance;     
    }

	public void OpenSocket() {
		socket = new UdpClient (socketPort);
	}

    // Send message to the server.
    public void Send(string message)
    {
        if (udp != null)
        {
            udp.Close();
        }
		byte[] data = Encoding.ASCII.GetBytes(message);
		socket.Send(data, data.Length, nectarIP, socketPort);
    }

    // Wrapper for UdpClient's BeginReceive method.
    public void BeginReceiveWrapper(AsyncCallback callback)
    {
		socket.BeginReceive(callback, socket);
    }

    // Waits for a response asynchronously, then logs
    // the response for debugging purposes.
    public void DebugIncomingData(IAsyncResult asyncResult)
    {
		socket = new UdpClient (socketPort);
		target = new IPEndPoint(IPAddress.Parse(nectarIP),socketPort);
        byte[] data = socket.EndReceive(asyncResult, ref target);
        string response = Encoding.UTF8.GetString(data);
        string serverResponse = response;
        Debug.Log(response);
        socket.Close();
    }

	//EndReceive Wrapper
	public void EndReceiveWrapper(IAsyncResult asyncResult)
	{
		byte[] data = socket.EndReceive (asyncResult, ref target);
		this.receivedData = data;
		socket.Close ();
	}

	public void End() {
		socket.Close ();
	}

    // Call after calling the EndReceive method.
    // Gets the data from the last response received
    // from the server.
    public byte[] GetData()
	{
		return receivedData;
	}	
}
