using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

public class ExitMenu : MonoBehaviour {

    public void Start()
    {
        NetworkManager nm = NetworkManager.singleton;
        MatchInfo matchInfo = nm.matchInfo;
        nm.matchMaker.DestroyMatch(matchInfo.networkId, 0, nm.OnDestroyMatch);

        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopMatchMaker();
        nm.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, nm.OnDropConnection);
        NetworkManager.Shutdown();
        Destroy(GameObject.Find("NetworkManager"));
        NetworkTransport.Shutdown();
        foreach(GameObject i in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if(i.name != "Canvas" && i.name != "Message" && i.name != "ReturnMainMenu"
                && i.name != "ReturnText" && i.name != "EventSystem" && i.name != "Main Camera"
                && i.name != "EventCoordinator" && i.name != "GameObject")
            Destroy(i);
        }
    }
}
