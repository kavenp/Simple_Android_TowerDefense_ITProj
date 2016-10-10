using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoMPWOne : MonoBehaviour {

	// Go to multiplayer scene when clicked
	public void OnClick()
	{
		GameObject[] replayManagers = GameObject.FindGameObjectsWithTag("RM");
        GameObject[] playerBases = GameObject.FindGameObjectsWithTag("PB");

		foreach(GameObject r in replayManagers)
		{
            Destroy(r);
        }

		foreach(GameObject p in playerBases)
		{
            Destroy(p);
        }

		// Purge all active connections
		Network.Disconnect();
        MasterServer.UnregisterHost();
		MasterServer.ClearHostList();


		SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
		if(GameObject.FindGameObjectWithTag("NetworkManager") == null)
		{
			Instantiate(GameObject.FindGameObjectWithTag("NetworkManager"));
		}
	}
}
