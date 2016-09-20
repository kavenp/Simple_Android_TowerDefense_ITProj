using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

public class MP_JoinGame : MonoBehaviour
{
    private NetworkManager nm;

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
        nm.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, nm.OnMatchJoined);
		ClearRoomList();
        status.text = "JOINING...";

        // Disable networking overlay
        mp_background.SetActive(false);
        hostgame_ui.SetActive(false);
        joingame_ui.SetActive(false);
    }
}
