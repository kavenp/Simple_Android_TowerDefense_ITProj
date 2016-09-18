using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System;

public class TCPClient1 : MonoBehaviour
{

    private TcpClient tcpClient = null;

    public void ConnectToServer()
    {
        //this.tcpClient = tcpClient;
        //tcpClient = new TcpClient(AddressFamily.InterNetwork);
        IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Any, 9876);
        tcpClient = new TcpClient(tcpEndPoint);

        //IPAddress[] remoteHost = Dns.GetHostAddresses("hostaddress");

        //Start the async connect operation           

        tcpClient.BeginConnect(IPAddress.Any, 9876, new
                          AsyncCallback(ConnectCallback), tcpClient);

    }


    private void ConnectCallback(IAsyncResult result)
    {

        //We are connected successfully.

        try
        {
            NetworkStream networkStream = tcpClient.GetStream();

            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];

            //Now we are connected start asyn read operation.

            networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
        }
        catch
        {
            Debug.Log("Network stream exception");
        }




    }



    /// Callback for Read operation
    private void ReadCallback(IAsyncResult result)
    {

        NetworkStream networkStream;


            networkStream = tcpClient.GetStream();




        byte[] buffer = result.AsyncState as byte[];

        string data = ASCIIEncoding.ASCII.GetString(buffer, 0, buffer.Length);

        //Do something with the data object here.
        Debug.Log(data);

        //Then start reading from the network again.

        networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);

    }
}
