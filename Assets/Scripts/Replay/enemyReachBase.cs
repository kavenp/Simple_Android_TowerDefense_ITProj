using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Updates and displays the number of lives
// remaining for the players.
public class enemyReachBase : MonoBehaviour
{

    // Wait for trigger to exit so that lives
    // is updated on all clients.
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
