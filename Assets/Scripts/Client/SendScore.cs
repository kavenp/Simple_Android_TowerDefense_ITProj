using Newtonsoft.Json;
using System;
using UnityEngine;

public class SendScore : MonoBehaviour
{
    // If the game was won,
    // sends the score and user ID to the server.
    void Start()
    {
        GameObject playerBase = GameObject.Find("PlayerBase");
        LivesScript livesScript =
            playerBase.GetComponent<LivesScript>();

        if (livesScript.numLives <= 0)
        {
            return;
        }

        int score = livesScript.numLives * 100;

        // Store the infomation to be sent in an object,
        // which can then be serialized into JSON
        ScoreInfo scoreInfo = new ScoreInfo();
        scoreInfo.senderID = SystemInfo.deviceUniqueIdentifier;
        scoreInfo.score = score;

        string scoreMessage = JsonConvert.SerializeObject(scoreInfo);

        ClientConnection clientConnection = ClientConnection.GetInstance();
        clientConnection.Send(scoreMessage);

        // Test purposes only
        clientConnection.BeginReceiveWrapper(
            new AsyncCallback(clientConnection.DebugIncomingData));
    }
}
