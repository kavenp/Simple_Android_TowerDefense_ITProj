using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public Vector3 spawnValues;

    public float spawnWait;
    public float startWait;
    public float waveWait;

    void Start ()
    {
        StartCoroutine (SpawnWaves ());
    }

    IEnumerator SpawnWaves ()
    {
        yield return new WaitForSeconds (startWait);

        while (true)
        {
            Vector3 spawnPosition = new Vector3 (Random.Range (0, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate (enemy, spawnPosition, spawnRotation);
            yield return new WaitForSeconds (spawnWait);
        }

        yield return new WaitForSeconds (waveWait);
    }
    
}
