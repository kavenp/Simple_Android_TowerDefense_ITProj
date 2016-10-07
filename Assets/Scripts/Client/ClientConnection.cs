using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

// Provides an interface to the NeCTAR server.
public class ClientConnection
{
	private int socketPort = 9876;
	private string nectarIP = "115.146.95.127";

	private IPEndPoint target;
	private UdpClient server;
	private UdpClient socket;

    private static ClientConnection instance = new ClientConnection();

	private string serverResponse;
	private byte[] receivedData;

    // Singleton.
    private ClientConnection()
	{
		//socket = new UdpClient (socketPort);
	}

    // Get client connection object.
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
		this.serverResponse = response;
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

	public byte[] GetData()
	{
		return receivedData;
	}

	public string GetResponse() 
	{
		return serverResponse;	
	}
		
}
