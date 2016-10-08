using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

public class MP_JoinGame : MonoBehaviour
{
	public GameObject chatbox;
	public GameObject canvas;

	private NetworkManager nm;

	private ClientConnection clientConnection = ClientConnection.GetInstance();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;

    List<GameObject> roomList = new List<GameObject>();
    private GameObject mp_background;
    private GameObject hostgame_ui;
    private GameObject joingame_ui;

	private string receivedRoom = "";

    private MatchInfoSnapshot theCurrentMatchInfo = null;
    void Start()
    {
        nm = NetworkManager.singleton;

        // Check
        if (nm.matchMaker == null)
        {
            nm.StartMatchMaker();
        }

        // Refresh room list
        RefreshRoomList();

        // Get UI components
        mp_background = GameObject.FindGameObjectWithTag("Background");
        hostgame_ui = GameObject.FindGameObjectWithTag("HostGameCanvas");
        joingame_ui = GameObject.FindGameObjectWithTag("JoinGameCanvas");
    }

	void Update() {
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
	}

    public void RefreshRoomList()
    {
        ClearRoomList();
        nm.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";

        // Check for match list
        if (!success || matchList == null)
        {
            status.text = "Couldn't get room list.";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            // Instantiate the rooms
            GameObject _roomListItemGo = Instantiate(roomListItemPrefab);
            _roomListItemGo.transform.SetParent(roomListParent);

            MP_RoomListItem _roomListItem = _roomListItemGo.GetComponent<MP_RoomListItem>();
            // Check its not null
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }

            // Add room to list
            roomList.Add(_roomListItemGo);
        }

        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment.";
        }
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i += 1)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        // Try to join match
        nm.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    // Join match was successful
    public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        ClearRoomList();
        status.text = "JOINING...";

        MatchInfoSnapshot cm = getCurrentMatchInfo();

        RoomInfo roomInfo = new RoomInfo ();
		roomInfo.senderID = SystemInfo.deviceUniqueIdentifier;
		roomInfo.roomName = cm.name;

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

	public void ReceiveAck (IAsyncResult asyncResult) {
		clientConnection.EndReceiveWrapper (asyncResult);
		byte[] received = clientConnection.GetData ();
		string convertData = Encoding.UTF8.GetString (received);
		MessageInfo receivedMsg = JsonConvert.DeserializeObject<MessageInfo> (convertData);
		this.receivedRoom = receivedMsg.roomID;
	}

    public void setCurrentMatchInfo(MatchInfoSnapshot match)
    {
        this.theCurrentMatchInfo = match;
    }

     public MatchInfoSnapshot getCurrentMatchInfo()
    {
        return this.theCurrentMatchInfo;
    }
}
