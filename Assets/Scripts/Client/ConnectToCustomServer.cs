using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

// Derived from
// http://www.robotmonkeybrain.com/good-enough-guide-to-unitys-unet-transport-layer-llapi/
public class ConnectToCustomServer : MonoBehaviour
{

    private int channelId;
    private int socketId;
    private int socketPort = 6002;

    private int connectionId;

    // Starts a host (opens a socket).
    void Start()
    {
        /*
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        channelId = config.AddChannel(QosType.Reliable);

        int maxConnections = 10;
        HostTopology topology = new HostTopology(config, maxConnections);

        socketId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket Open. SocketId is: " + socketId);
        */

        IPEndPoint RemoteEndPoint = new IPEndPoint(
                    IPAddress.Parse("10.12.209.75"), 6002);
        Socket server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);
        string welcome = "Hello, are you there?";
        byte[] data = Encoding.ASCII.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);

      // while (valid message) get valid message
        
     
    }

    // Connects to socket.S
    public void Connect()
    {
        byte error;
        // Nectar server IP = 115.146.95.127
        connectionId = NetworkTransport.Connect(socketId, "10.12.209.75",
            socketPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionId);
    }

    // Sends a message to the server.
    public void SendSocketMessage(string message)
    {
        byte error;
        byte[] buffer = new byte[1024];

        //buffer = Encoding.ASCII.GetBytes(message);

        
        Stream stream = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, message);
        



        int bufferSize = 1024; 
        //int m = System.BitConverter.ToInt32(buffer, 0);
          
    
        NetworkTransport.Send(socketId, connectionId, channelId, buffer, bufferSize, out error);
    }

    void CheckMessages()
    {

    }

    //void Update()
    //{
    //    int recHostId;
    //    int recConnectionId;
    //    int recChannelId;
    //    byte[] recBuffer = new byte[1024];
    //    int bufferSize = 1024;
    //    int dataSize;
    //    byte error;
    //    NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostId, out
    //        recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);

    //    switch (recNetworkEvent)
    //    {
    //        case NetworkEventType.Nothing:
    //            break;
    //        case NetworkEventType.ConnectEvent:
    //            Debug.Log("incoming connection event received");
    //            break;
    //        case NetworkEventType.DataEvent:
    //            Stream stream = new MemoryStream(recBuffer);
    //            BinaryFormatter formatter = new BinaryFormatter();
    //            string message = formatter.Deserialize(stream) as string;
    //            Debug.Log("incoming message event received: " + message);
    //            break;
    //        case NetworkEventType.DisconnectEvent:
    //            Debug.Log("remote client event disconnected");
    //            break;
    //    }
    //}
}
