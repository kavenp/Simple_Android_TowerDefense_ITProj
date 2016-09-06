using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        channelId = config.AddChannel(QosType.Reliable);

        int maxConnections = 10;
        HostTopology topology = new HostTopology(config, maxConnections);

        socketId = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket Open. SocketId is: " + socketId);

        Connect();
        SendSocketMessage("HelloServer");
    }

    // Connects to socket.
    public void Connect()
    {
        byte error;
        connectionId = NetworkTransport.Connect(socketId, "115.146.95.127",
            socketPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionId);
    }

    // Sends a message to the server.
    public void SendSocketMessage(string message)
    {
        byte error;
        byte[] buffer = new byte[1024];
        Stream stream = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, message);

        int bufferSize = 1024;

        NetworkTransport.Send(socketId, connectionId, channelId, buffer, bufferSize, out error);
    }
}
