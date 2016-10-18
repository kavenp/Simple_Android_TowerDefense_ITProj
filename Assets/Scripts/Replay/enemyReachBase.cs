// Script copied from the LivesScript script, but removed components that didn't work with replay

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Updates and displays the number of lives
// remaining for the players.
public class enemyReachBase : MonoBehaviour
{
    public List<GameObject> reached;
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
		reached = new List<GameObject>();
		if (FindObjectsOfType(GetType()).Length > 1)
         {
             Destroy(gameObject);
         }
    }
	
	void Update()
	{
	    if(Application.loadedLevelName == "Multiplayer_W1"){
				Destroy(gameObject);
			}
	}

    // Deduct a life if the player has lives
    // and the object that has entered the
    // player's base is an enemy.
    void OnTriggerEnter(Collider other)
    {
	    
        if (other.tag == "Enemy" && numLives > 0 && !reached.Contains(other.gameObject))
        {
            numLives--;
            livesDisplay.text = "Lives: " + numLives;
        }
		
		reached.Add(other.gameObject);
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
