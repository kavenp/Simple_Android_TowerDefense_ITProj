using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using System.Collections;

public class MP_LeaveRoom : MonoBehaviour
{
    private NetworkManager nm;
	private ClientConnection clientConnection = ClientConnection.GetInstance();

    void Start()
    {
        nm = NetworkManager.singleton;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(3);
    }
    public void LeaveRoom()
    {
        MatchInfo matchInfo = nm.matchInfo;
        MP_PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<MP_PlayerController>();
        pc.CmdQuitObject();
        //MP_GameCoordinator mgc = GameObject.FindGameObjectWithTag("GameController").GetComponent<MP_GameCoordinator>();
        //mgc.QUIT();
        //MP_EndGame eg = GameObject.FindGameObjectWithTag("GameController").GetComponent<MP_EndGame>();
        //eg.SpawnQuitObject();

        //MP_GameCoordinator mgc = GameObject.FindGameObjectWithTag("GameController").GetComponent<MP_GameCoordinator>();
        //mgc.Disconnect(clientConnection);

        // Wait for cmd sync
        //StartCoroutine(Waiting());

        //nm.matchMaker.DestroyMatch(matchInfo.networkId, 0, OnMatchDestroy);
        //Network.Disconnect();
        //MasterServer.UnregisterHost();
        //clientConnection.End();
    }
     public static void OnMatchDestroy(bool success, string extendedInfo)
     {
        //Debug.Log("Match Destroyed" + extendedInfo);
        NetworkManager.singleton.StopHost();
        //Destroy(GameObject.Find("NetworkManager"));
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

