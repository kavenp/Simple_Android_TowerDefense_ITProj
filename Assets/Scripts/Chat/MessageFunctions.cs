using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageFunctions : MonoBehaviour {

	[SerializeField] Text text;

	void Start() {
		//Destroys the message after 3 seconds if it's not already destroyed
		Destroy (gameObject, 3.0f);
	}

	public void ShowMessage (string message) {
		text.text = message;
	}

	public void HideMessage () {
		Destroy (gameObject);
	}
}
