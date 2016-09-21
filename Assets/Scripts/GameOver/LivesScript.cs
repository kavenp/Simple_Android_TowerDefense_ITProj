using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Updates and displays the number of lives
// remaining for the players.
public class LivesScript : MonoBehaviour
{

    public int numLives;
    public Text livesDisplay;

    void Awake()
    {
        // Need the number of lives remaining
        // for calculating and displaying the score
        // in the GameOver scene
        DontDestroyOnLoad(transform.gameObject);
        livesDisplay = GameObject.FindGameObjectWithTag("LivesDisplay").GetComponent<Text>();
    }

    void Start()
    {
        livesDisplay.text = "Lives: " + numLives;
    }

    // Deduct a life if the player has lives
    // and the object that has entered the
    // player's base is an enemy.
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && numLives > 0)
        {
            numLives--;
            livesDisplay.text = "Lives: " + numLives;
        }
    }

    // Wait for trigger to exit so that lives
    // is updated on all clients.
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);

        if (numLives <= 0)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
