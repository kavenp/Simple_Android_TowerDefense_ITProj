// Script copied from the DisplayScore script, but removed components that didn't work with replay

using UnityEngine;
using UnityEngine.UI;

// Displays the score.
// Score is the number of lives remaining, times 100.
public class replayScore : MonoBehaviour
{
    public Text scoreDisplay;

    void Start()
    {
        GameObject playerBase = GameObject.Find("PlayerBase");
        enemyReachBase livesScript =
            playerBase.GetComponent<enemyReachBase>();

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
