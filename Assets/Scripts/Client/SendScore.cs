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
            // Game was lost, no score
            return;
        }

        int score = livesScript.numLives * 100;

        // Store the infomation to be sent in an object,
        // which can then be serialized into JSON
        NewScoreMessage newScoreMessage = new NewScoreMessage();
        newScoreMessage.type = "NewScore";
        newScoreMessage.senderID = SystemInfo.deviceUniqueIdentifier;
        newScoreMessage.score = score;

        string message = JsonConvert.SerializeObject(newScoreMessage);

        ClientConnection clientConnection = ClientConnection.GetInstance();
        clientConnection.OpenSocket();
        clientConnection.Send(message);
    }
}