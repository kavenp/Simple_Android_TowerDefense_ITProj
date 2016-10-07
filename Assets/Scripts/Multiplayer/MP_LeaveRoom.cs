using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using System;

public class MP_LeaveRoom : MonoBehaviour
{
    private NetworkManager nm;
	private ClientConnection clientConnection = ClientConnection.GetInstance();

    void Start()
    {
        nm = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = nm.matchInfo;
        nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        nm.StopHost();

		//stop receiving chat
		clientConnection.End();

		SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
