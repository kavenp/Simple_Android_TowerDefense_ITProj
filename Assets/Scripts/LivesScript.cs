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
        int size = (int)(Screen.dpi / 8.0f);
        if (size > 0)
        {
            // Screen.dpi returns 0 if dpi cannot be determined
            livesDisplay.fontSize = size;
         }
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
