using UnityEngine;
using UnityEngine.UI;


public class DisplayScore : MonoBehaviour
{
    public Text scoreDisplay;

    void Start()
    {
        GameObject scoreInfo = GameObject.Find("ScoreInfo");
        ScoreInfoScript scoreInfoScript =
            scoreInfo.GetComponent<ScoreInfoScript>();
        scoreDisplay.text = "Score: " + scoreInfoScript.score;
    }
}
