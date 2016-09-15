using UnityEngine;

public class ScoreInfoScript : MonoBehaviour
{
    
    public int score;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
