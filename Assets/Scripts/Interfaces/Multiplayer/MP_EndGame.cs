using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MP_EndGame : NetworkBehaviour
{
    GameObject quitObject;
    void Start ()
	{
		quitObject = Resources.Load("QUIT") as GameObject;
	}

	public void SpawnQuitObject()
	{
		// Spawn quit object
        var quitObj = (GameObject)Instantiate(quitObject, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(quitObj);
	}
}
