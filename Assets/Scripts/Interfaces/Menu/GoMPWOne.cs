using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoMPWOne : MonoBehaviour {

	// Go to multiplayer scene when clicked
	public void OnClick()
	{
		SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
		if(GameObject.FindGameObjectWithTag("NetworkManager") == null)
		{
			Instantiate(GameObject.FindGameObjectWithTag("NetworkManager"));
		}

		if(GameObject.FindGameObjectWithTag("RM") != null)
		{
            Destroy(GameObject.FindGameObjectWithTag("RM"));
        }

		if(GameObject.FindGameObjectWithTag("PB") != null)
		{
            Destroy(GameObject.FindGameObjectWithTag("PB"));
        }
	}
}
