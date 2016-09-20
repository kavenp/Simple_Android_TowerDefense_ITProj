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
    private GameObject hostgame_ui;

    private GameObject joingame_ui;

    private GameObject goldDisplay;
    private GameObject livesDisplay;

    void Start()
    {
        // Get instance of network manager
        nm = NetworkManager.singleton;

        // Start matchmaking automatically
        if (nm.matchMaker == null)
        {
            nm.StartMatchMaker();
        }

        // Get ui components
        theRoomName = GameObject.FindGameObjectWithTag("RoomName").GetComponent<InputField>();
        nwMessage = GameObject.FindGameObjectWithTag("nwMessage").GetComponent<Text>();
        mp_background = GameObject.FindGameObjectWithTag("Background");
        hostgame_ui = GameObject.FindGameObjectWithTag("HostGameCanvas");
        joingame_ui = GameObject.FindGameObjectWithTag("JoinGameCanvas");
        goldDisplay = GameObject.FindGameObjectWithTag("GoldDisplay");
        livesDisplay = GameObject.FindGameObjectWithTag("LivesDisplay");

        livesDisplay.SetActive(false);
        goldDisplay.SetActive(false);
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
            hostgame_ui.SetActive(false);
            joingame_ui.SetActive(false);

            // Allow lives/gold overlay
            livesDisplay.SetActive(true);
            goldDisplay.SetActive(true);
        }
    }

}
