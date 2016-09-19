﻿using UnityEngine;
using UnityEngine.UI;

// Displays the score.
// Score is the number of lives remaining, times 100.
public class DisplayScore : MonoBehaviour
{
    public Text scoreDisplay;

    void Start()
    {
        GameObject playerBase = GameObject.Find("PlayerBase");
        LivesScript livesScript =
            playerBase.GetComponent<LivesScript>();

        if (livesScript.numLives > 0)
        {
            int score = livesScript.numLives * 100;
            scoreDisplay.text = "Score: " + score;
        }
        else
        {
            scoreDisplay.text = "Game Over";
        }
    }
}
