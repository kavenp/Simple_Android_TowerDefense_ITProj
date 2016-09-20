using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class MP_GameCoordinator : NetworkBehaviour
{
    public GameObject enemy;
    public Vector3 spawnValues;

    public int numberOfWaves;
    public int numberOfEnemiesPerWave;
    public float spawnWait;
    public float waveWait;

    bool isSpawning = false;
    bool isWave = false;

    void Update()
    {
        if (isSpawning == false && numberOfEnemiesPerWave > 0)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemyWave(spawnWait));
        }

        if (isWave == false && numberOfWaves > 0)
        {
            isWave = true;
            StartCoroutine(SpawnNextWave(waveWait));
        }
        else if (numberOfWaves == 0 && isWave == false &&
            numberOfEnemiesPerWave == 0 &&
            GameObject.FindWithTag("Enemy") == null)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    IEnumerator SpawnEnemyWave(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Vector3 spawnPosition = new Vector3(spawnValues.x, spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        var createdEnemy = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);
        NetworkServer.Spawn(createdEnemy);

        isSpawning = false;
        numberOfEnemiesPerWave -= 1;
    }

    IEnumerator SpawnNextWave(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isWave = false;
        numberOfWaves -= 1;

        // This is terrible but can fix it later on
        numberOfEnemiesPerWave = 5;
    }
}
