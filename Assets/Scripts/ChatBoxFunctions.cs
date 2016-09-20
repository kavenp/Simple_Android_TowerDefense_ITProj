using UnityEngine;
using System.Collections;

public class ChatBoxFunctions : MonoBehaviour {

	[SerializeField] Transform chatWindow;
	[SerializeField] GameObject newMessagePrefab;

	public string message = "";

	public void SetMessage (string message) {
		this.message = message;
	}

	public void ShowMessage () {
		if (message != "") {
			GameObject newMsg = (GameObject)Instantiate (newMessagePrefab);
			newMsg.transform.SetParent (chatWindow);
			newMsg.transform.SetSiblingIndex (chatWindow.childCount);
			newMsg.GetComponent<MessageFunctions> ().ShowMessage (message);
		}
	}
}
