using UnityEngine;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Net;
using System.Text;

public class ChatBoxFunctions : MonoBehaviour {

	[SerializeField] Transform chatWindow;
	[SerializeField] GameObject newMessagePrefab;

	public string message = "";
	public string receivedString = "";
	private ClientConnection clientConnection = ClientConnection.GetInstance();

	//sets the chat message
	public void SetMessage (string message) {
		this.message = message;
	}

	public void ShowMessage (string msg) {
		//shows message in chat if sending non-empty string
		if (msg != "") {
			//instantiates new message
			GameObject newMsg = (GameObject)Instantiate (newMessagePrefab);
			newMsg.transform.SetParent (chatWindow);
			//sets sibling index so new message shows up under last message;
			newMsg.transform.SetSiblingIndex (chatWindow.childCount);
			//shows message on the new message gameobject
			newMsg.GetComponent<MessageFunctions> ().ShowMessage (msg);
		}
	}

	public void ShowSentMessage() {
		ShowMessage (message);
        message = "";
	}

	public void ShowReceivedMessage() {
		ShowMessage ("received: " + receivedString);
		//reset to empty after showing
		this.receivedString = "";
	}

	//sends message to server in JSON format
	public void SendMessage () {
		MessageInfo msgInfo = new MessageInfo ();
		msgInfo.senderID = SystemInfo.deviceUniqueIdentifier;
		msgInfo.message = this.message;
		string chatMsg = JsonConvert.SerializeObject (msgInfo);
		clientConnection.Send (chatMsg);
		clientConnection.BeginReceive(
			new AsyncCallback(ReceiveMessage));
	}

	public void ReceiveMessage (IAsyncResult asyncResult) {
		clientConnection.EndReceive (asyncResult);
		byte[] received = clientConnection.GetData ();
		string convertData = Encoding.UTF8.GetString (received);
		MessageInfo receivedMsg = JsonConvert.DeserializeObject<MessageInfo> (convertData);
		this.receivedString = receivedMsg.message;
		//start waiting for next message
		clientConnection.BeginReceive(
			new AsyncCallback(ReceiveMessage));
	}

	void Start () {
		this.message = "Initial connection check.";
		SendMessage ();
		this.message = "";
	}

	void Update () {
		if (receivedString != "") {
			ShowReceivedMessage ();
		}
	}

}
