using UnityEngine;
using UnityEngine.Networking;

public class MP_HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;
    private string roomName;
    private NetworkManager nm;

    void Start()
    {
        // Get instance of network manager
        nm = NetworkManager.singleton;

        // Start matchmaking automatically
        if (nm.matchMaker == null)
        {
            nm.StartMatchMaker();
        }
    }


    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoome()
    {
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players");

            // Create room
            nm.matchMaker.CreateMatch(roomName, roomSize, true, "", null, null, 0, 1, nm.OnMatchCreate);
        }
    }

}
