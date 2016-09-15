using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RestartScript : MonoBehaviour
{

    public void OnClick()
    {
        SceneManager.LoadScene("Multiplayer_W1", LoadSceneMode.Single);
    }
}
