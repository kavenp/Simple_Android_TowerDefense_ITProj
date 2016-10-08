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
        nm.matchMaker.DestroyMatch(matchInfo.networkId, 0, OnMatchDestroy);

        // nm.StopHost();
        // nm.StopMatchMaker();


        //stop receiving chat
        clientConnection.End();

		// SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

     public static void OnMatchDestroy(bool success, string extendedInfo)
     {
        Debug.Log("Match Destroyed" + extendedInfo);
        NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Destroy(GameObject.Find("NetworkManager"));

        //  NetworkManager.singleton.StopHost();
        //  NetworkManager.singleton.StopMatchMaker();
        //  NetworkManager.Shutdown();

        //  NetworkTransport.Shutdown();
    }
}

