using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using System.Collections;

/// Class that allows the player to leave a game
public class MP_LeaveRoom : MonoBehaviour
{
    // Networking fields
    private NetworkManager nm;
	private ClientConnection clientConnection = ClientConnection.GetInstance();

    void Start()
    {
        nm = NetworkManager.singleton;
    }

    // Wait period
    IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(3);
    }

    // Leave room calls this function
    public void LeaveRoom()
    {
        // Get the player object and call it to change the scene
        MatchInfo matchInfo = nm.matchInfo;
        MP_PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<MP_PlayerController>();
        pc.CmdServerChangeScene();

        /// Alternative way of handling leaving the game

        //pc.CmdQuitObject();
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

    // Callback when match is destroyed
    public static void OnMatchDestroy(bool success, string extendedInfo)
    {
        //Debug.Log("Match Destroyed" + extendedInfo);
        NetworkManager.singleton.StopHost();
        //Destroy(GameObject.Find("NetworkManager"));
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

