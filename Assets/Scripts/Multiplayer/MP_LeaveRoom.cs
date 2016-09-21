using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MP_LeaveRoom : MonoBehaviour
{
    private NetworkManager nm;

    void Start()
    {
        nm = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = nm.matchInfo;
        nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        nm.StopHost();

		SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
    }
}
