using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Reloads the multiplayer scene.
public class RestartScript : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
    }
}
