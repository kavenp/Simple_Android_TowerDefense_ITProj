using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Checks if the game has been won.
// Broken, does not load game over scene properly
// and sends messages to the server repeatedly.
public class CheckVictory : MonoBehaviour
{
    private float time;
    private float lastWaveTime = 25;

    void Start()
    {
        time = 0;
    }

    // If the last wave has passed, and no enemies are present,
    // load the game over scene.
    void Update()
    {
        //if (time > 3)
        //{
        //    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        //}

        if (time < lastWaveTime)
        {
            time += Time.deltaTime;
            return;
        }

        if (GameObject.FindWithTag("Enemy") == null)
        {
            // The game has been won
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
