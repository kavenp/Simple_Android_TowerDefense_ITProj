using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Retrieves from the server and displays
// the user's top scores.
public class RetrieveScores : MonoBehaviour
{
    // Text UI to display the score
    public Text score1Display;
    public Text score2Display;
    public Text score3Display;

    // The object that contains the deserialized
    // JSON response from the server
    private GetScoresResponse scores;
    
    // True if the server has responsed and
    // the user's scores have been received
    private bool scoresReceived = false;


    // If the scores menu is loaded,
    // retireve the scores of the user from the server.
    void Start()
    {
        // Store the infomation to be sent in an object,
        // which can then be serialized into JSON
        GetScoresMessage getScoresMessage = new GetScoresMessage();
        getScoresMessage.type = "GetScores";
        getScoresMessage.senderID = SystemInfo.deviceUniqueIdentifier;

        string message = JsonConvert.SerializeObject(getScoresMessage);

        ClientConnection clientConnection = ClientConnection.GetInstance();
        clientConnection.Send(message);

        // Wait for the response
        clientConnection.BeginReceive(
            new AsyncCallback(GetScoresResponse));
    }

    void Update()
    {
        // Can only change the text fields from the main thread
        if (scoresReceived)
        {
            score1Display.text = scores.Score1.ToString();
            score2Display.text = scores.Score2.ToString();
            score3Display.text = scores.Score3.ToString();

            // Stop updating the text
            scoresReceived = false;
        }
    }

    // Called when a message from the server is received
    // in response to the get scores request message.
    public void GetScoresResponse(IAsyncResult asyncResult)
    {
        ClientConnection clientConnection = ClientConnection.GetInstance();
        clientConnection.EndReceive(asyncResult);

        // Get the data, encode and deserialize
        byte[] data = clientConnection.GetData();
        string jsonData = Encoding.UTF8.GetString(data);

        scores = JsonConvert.DeserializeObject<GetScoresResponse>(jsonData);
        scoresReceived = true;
    }
}
