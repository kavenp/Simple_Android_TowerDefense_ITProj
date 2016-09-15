using UnityEngine;
using UnityEngine.UI;


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
