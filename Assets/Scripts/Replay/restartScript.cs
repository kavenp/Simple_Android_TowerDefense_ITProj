using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Reloads the multiplayer scene.
public class restartScript : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("Replay", LoadSceneMode.Single);
    }
}
