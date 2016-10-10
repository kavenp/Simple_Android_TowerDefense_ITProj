using UnityEngine;
using UnityEngine.UI;

// Displays the score or
// displays "Game Over" if the game was lost.
// Score is the number of lives remaining, times 100.
public class DisplayScore : MonoBehaviour
{
    // Text UI to display the score
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

        // Destroy the Player Base so it doesn't remain
        // in future scenes
        Destroy(playerBase);
    }
}
