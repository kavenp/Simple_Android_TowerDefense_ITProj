using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivesScript : MonoBehaviour
{
    
    public int numLives;
    public Text livesDisplay;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        livesDisplay.text = "Lives: " + numLives;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && numLives > 0)
        {
            numLives--;
            livesDisplay.text = "Lives: " + numLives;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);

        if (numLives <= 0)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
