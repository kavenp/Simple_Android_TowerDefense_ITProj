using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScoresMenu : MonoBehaviour
{
    // Load the scores menu when clicked
    public void OnClick()
    {
        SceneManager.LoadScene("ScoresMenu", LoadSceneMode.Single);
    }
}
