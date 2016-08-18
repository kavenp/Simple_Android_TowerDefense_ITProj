using UnityEngine;
using System.Collections;

public class PauseTouched : MonoBehaviour
{
    // State
    bool isPaused = false;

    void OnMouseDown ()
    {
        // Game is not paused
        if (isPaused == false)
        {
            Time.timeScale = 0;
            Debug.Log ("Game paused");
            this.isPaused = true;
        }
        // Resume game
        else
        {
            Time.timeScale = 1;
            Debug.Log ("Game resumed");
            this.isPaused = false;
        }
    }
}
