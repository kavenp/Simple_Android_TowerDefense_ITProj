using UnityEngine;
using UnityEngine.UI;
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
	private string roomID = "";
	private ClientConnection clientConnection = ClientConnection.GetInstance();

	//sets the chat message
	public void SetMessage (string message) {
		this.message = message;
	}

	void Start () {
		clientConnection.OpenSocket();
		clientConnection.BeginReceiveWrapper(new AsyncCallback(ReceiveMessage));
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

	public void ShowReceivedMessage() {
		ShowMessage ("received: " + receivedString);
		//reset to empty after showing
		this.receivedString = "";
	}

	//sends message to server in JSON format
	public void SendMessage () {
		//Stop BeginReceive so we can send a message
		clientConnection.End ();
		MessageInfo msgInfo = new MessageInfo ();
		msgInfo.senderID = SystemInfo.deviceUniqueIdentifier;
		msgInfo.message = this.message;
		msgInfo.roomID = this.roomID;
		string chatMsg = JsonConvert.SerializeObject (msgInfo);
		//Reopen socket and send message
		clientConnection.OpenSocket ();
		clientConnection.Send (chatMsg);
		//Restart BeginReceive
		clientConnection.BeginReceiveWrapper (
			new AsyncCallback (ReceiveMessage));
    }

	public void ReceiveMessage (IAsyncResult asyncResult) {
		clientConnection.EndReceiveWrapper (asyncResult);
		byte[] received = clientConnection.GetData ();
		string convertData = Encoding.UTF8.GetString (received);
		MessageInfo receivedMsg = JsonConvert.DeserializeObject<MessageInfo> (convertData);
		this.receivedString = receivedMsg.message;
		this.roomID = receivedMsg.roomID;
		//start waiting for next message, restart BeginReceive
		clientConnection.OpenSocket();
		clientConnection.BeginReceiveWrapper (
			new AsyncCallback(ReceiveMessage));
	}

	public void SetRoom(string room) {
		this.roomID = room;
	}

	void Update () {
		if (receivedString != "") {
			ShowReceivedMessage ();
		}
	}
}
