using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MP_HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 2;
    private NetworkManager nm;
    private InputField theRoomName;
    private Text nwMessage;
    private GameObject mp_background;
    private GameObject networking_ui;

    void Start()
    {
        // Get instance of network manager
        nm = NetworkManager.singleton;

        // Get ui components
        theRoomName = GameObject.FindGameObjectWithTag("RoomName").GetComponent<InputField>();
        nwMessage = GameObject.FindGameObjectWithTag("nwMessage").GetComponent<Text>();
        mp_background = GameObject.FindGameObjectWithTag("Background");
        networking_ui = GameObject.FindGameObjectWithTag("HostGameCanvas");

        // Start matchmaking automatically
        if (nm.matchMaker == null)
        {
            nm.StartMatchMaker();
        }
    }
    public void CreateRoom()
    {
        string room = theRoomName.text;

        if(room == "" || room == null)
        {
            nwMessage.text = "The room name cannot be empty";
        }

        if (room != "" && room != null)
        {
            Debug.Log("Creating Room: " + room + " with room for " + roomSize + " players");

            // Start match
            nm.matchMaker.CreateMatch(room, roomSize, true, "", "", "", 0, 0, nm.OnMatchCreate);

            // Disable networking overlay
            mp_background.SetActive(false);
            networking_ui.SetActive(false);
        }
    }

}
