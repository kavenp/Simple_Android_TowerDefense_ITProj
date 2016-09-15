using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    
    public int numLives;
    public Text scoreDisplay;

    void Start()
    {
        int size = (int)(Screen.dpi / 8.0f);
        if (size > 0)
        {
            // Screen.dpi returns 0 if dpi cannot be determined
            scoreDisplay.fontSize = size;
         }
        scoreDisplay.text = "Lives: " + numLives;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            numLives--;
            scoreDisplay.text = "Lives: " + numLives;
        }
    }
}
