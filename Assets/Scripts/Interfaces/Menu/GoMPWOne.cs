using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

/// Class that goes to the multiplayer scene when clicked
public class GoMPWOne : MonoBehaviour {

	// Go to multiplayer scene when clicked
	public void OnClick()
	{
        // Get list of not destroyed objects
		GameObject[] replayManagers = GameObject.FindGameObjectsWithTag("RM");
        GameObject[] playerBases = GameObject.FindGameObjectsWithTag("PB");
        GameObject[] networkMan = GameObject.FindGameObjectsWithTag("NetworkManager");

        // Destroy these not destroyed objects
        foreach (GameObject r in replayManagers)
		{
            Destroy(r);
        }

		foreach(GameObject p in playerBases)
		{
            Destroy(p);
        }

        // Purge all active connections - alternative method
        //Network.Disconnect();
        //MasterServer.UnregisterHost();
        //MasterServer.ClearHostList();
        //NetworkManager nm = NetworkManager.singleton;

        foreach (GameObject n in networkMan)
        {
            Destroy(n);
        }

        // Load the main menu scene
		SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
		//if(GameObject.FindGameObjectWithTag("NetworkManager") == null)
		//{
		//	Instantiate(GameObject.FindGameObjectWithTag("NetworkManager"));
		//}
	}
}
