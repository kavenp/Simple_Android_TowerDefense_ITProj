using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoMPWOne : MonoBehaviour {

	// Go to multiplayer scene when clicked
	public void OnClick()
	{
		SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
		if(GameObject.Find("NetworkManager") == null)
		{
			Instantiate(GameObject.Find("NetworkManager"));
		}
	}
}
