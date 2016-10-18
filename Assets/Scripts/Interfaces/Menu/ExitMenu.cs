using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;
using System;

/// Class to disconnect all matchmaking and network features
public class ExitMenu : MonoBehaviour {

    // Disconnection variables
    int m_HostId = -1;
    List<int> m_ConnectionIds = new List<int>();
    MatchInfo m_MatchInfo;
    string m_MatchName = "NewRoom";
    NetworkMatch m_NetworkMatch;
    bool m_MatchCreated;
    bool m_MatchJoined;
    bool m_ConnectionEstablished;

    public void Start()
    {
        // Get the network manager and match information
        NetworkManager nm = NetworkManager.singleton;
        MatchInfo matchInfo = nm.matchInfo;
        NetworkMatch matchMaker = nm.matchMaker;

        /// Alternative way of handling connections
        //nm.matchMaker.DestroyMatch(matchInfo.networkId, 0, nm.OnDestroyMatch);
        //NetworkManager.singleton.StopHost();
        //NetworkManager.singleton.StopClient();
        //NetworkManager.singleton.StopMatchMaker();
        //nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        //NetworkManager.Shutdown();

        // Destroy all active connections
        while(Network.connections.Length > 0)
        {
            Network.CloseConnection(Network.connections[0], true);
        }

        // Find the Network Manager and destroy it
        Destroy(GameObject.Find("NetworkManager"));

        //NetworkTransport.Shutdown();

        // Destroy all irrelevant game objects in the same
        foreach(GameObject i in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if(i.name != "Canvas" && i.name != "Message" && i.name != "ReturnMainMenu"
                && i.name != "ReturnText" && i.name != "EventSystem" && i.name != "Main Camera"
                && i.name != "EventCoordinator" && i.name != "GameObject")
            Destroy(i);
        }
    }
}
