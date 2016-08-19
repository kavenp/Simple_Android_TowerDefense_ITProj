using UnityEngine;
using System.Collections;

public class PauseTouched : MonoBehaviour
{
    // State
    public bool isPaused = false;

    // Sprites
    public Sprite pauseSprite;
    public Sprite resumeSprite;

    // Sprite renderer
    private SpriteRenderer rend;

    // Game controller
    private GameController gc;

    void Start ()
    {
        rend = GetComponent<SpriteRenderer> ();
        rend.enabled = true;
        gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
    }

    void OnMouseDown ()
    {
        // Game is not paused
        if (isPaused == false)
        {
            Debug.Log ("Game paused");

            Time.timeScale = 0;
            rend.sprite = resumeSprite;
            this.isPaused = true;
            gc.setPauseFlag (true);
        }
        // Resume game
        else
        {
            Debug.Log ("Game resumed");

            Time.timeScale = 1;
            rend.sprite = pauseSprite;
            this.isPaused = false;
            gc.setPauseFlag (false);
        }
    }
}
