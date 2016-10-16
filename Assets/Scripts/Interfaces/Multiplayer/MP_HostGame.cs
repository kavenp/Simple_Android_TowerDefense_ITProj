using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using System.Text;

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

	private ClientConnection clientConnection = ClientConnection.GetInstance();
	public GameObject chatbox;
	public GameObject canvas;
	private string receivedRoom = "";

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
    }
	
	void Update()
	{
		//instantiate a chatbox once we have received room info
		//from ack packet and set roomID for chatBox
		if (this.receivedRoom != "") {
			GameObject newChat =
				(GameObject)Instantiate (chatbox, chatbox.transform.position, chatbox.transform.rotation);
			newChat.transform.SetParent (canvas.transform,false);
			ChatBoxFunctions chatFuncs = newChat.GetComponent<ChatBoxFunctions> ();
			chatFuncs.SetRoom(this.receivedRoom);
			//reset to null once ack is over
			//so we don't keep instantiating new chatboxes
			this.receivedRoom = "";
		}
		if(Application.loadedLevelName == "GameOver"){
				Destroy(gameObject);
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

            // Set network message to nothing
            nwMessage.text = "";

            // Start match
            nm.matchMaker.CreateMatch(room, roomSize, true, "", "", "", 0, 0, nm.OnMatchCreate);

			RoomInfo roomInfo = new RoomInfo ();

			roomInfo.senderID = SystemInfo.deviceUniqueIdentifier;
			roomInfo.type = "roomInfo";
			roomInfo.roomName = room;

			//serialize and send room information to chat server
			string initConMsg = JsonConvert.SerializeObject(roomInfo);
			clientConnection.OpenSocket ();
			clientConnection.Send (initConMsg);
			//After initial send start waiting for return ack from server
			clientConnection.BeginReceiveWrapper(
				new AsyncCallback(ReceiveAck));

            // Disable networking overlay
            mp_background.SetActive(false);
            hostgame_ui.SetActive(false);
            joingame_ui.SetActive(false);
        }
    }

	public void ReceiveAck (IAsyncResult asyncResult) {
		clientConnection.EndReceiveWrapper (asyncResult);
		byte[] received = clientConnection.GetData ();
		string convertData = Encoding.UTF8.GetString (received);
		MessageInfo receivedMsg = JsonConvert.DeserializeObject<MessageInfo> (convertData);
		this.receivedRoom = receivedMsg.roomID;
	}
}
