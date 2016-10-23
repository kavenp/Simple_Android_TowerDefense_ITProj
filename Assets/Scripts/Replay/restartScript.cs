// Script copied from elsewhere, but altered to work with replay
// Please don't count this towards a total lines of code count

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Reloads the replay scene.
public class restartScript : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("Replay", LoadSceneMode.Single);
    }
}
