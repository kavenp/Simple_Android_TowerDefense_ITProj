using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class TCPClient : MonoBehaviour
{

    private TcpClient tcpClient;
    private IPEndPoint tcpEndPoint;

    StreamReader clientStreamReader;
    StreamWriter clientStreamWriter;

    private int socketPort = 9876;
    private string nectarIP = "115.146.95.127";

    void Start()
    {
        TCPClient1 tcp1 = new TCPClient1();
        tcp1.ConnectToServer();
        Debug.Log("1");

        tcpEndPoint = new IPEndPoint(IPAddress.Any, socketPort);
        tcpClient = new TcpClient(tcpEndPoint);

        Debug.Log("2");
        tcpClient.Connect(nectarIP, socketPort);
        

        NetworkStream networkStream = tcpClient.GetStream();
        //clientStreamReader = new StreamReader(networkStream);
        clientStreamWriter = new StreamWriter(networkStream);


        //Debug.Log(clientStreamReader.ReadLine());

        //tcpClient.BeginConnect(IPAddress.Any, socketPort,
        //    new AsyncCallback(IncomingData), tcpEndPoint);
        Debug.Log("3");

        clientStreamWriter.WriteLine("hi");
        clientStreamWriter.Flush();


        Debug.Log("4");
        clientStreamWriter.Close();
        tcpClient.Close();

        
        
    }

    void IncomingData(IAsyncResult asyncResult)
    {
        Debug.Log(clientStreamReader.ReadLine());

        //byte[] data = tcpClient.EndReceive(asyncResult, ref tcpEndPoint);

        //string response = Encoding.UTF8.GetString(data);
        //Debug.Log(response);

        //tcpClient.Close();
    }
}
