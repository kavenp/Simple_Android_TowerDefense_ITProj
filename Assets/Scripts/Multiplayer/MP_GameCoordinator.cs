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

    private int waveMultiplier = 2;

    private const int creepHealthAdditive = 30;
    private const float creepDamageRed = 0.1f;


    // Number of players
    int concurrentPlayers;

    bool isSpawning = false;
    bool isWave = false;

    void Update()
    {
        // Get number of concurrent players
        concurrentPlayers = GameObject.FindGameObjectsWithTag("Player").Length;

        // If 2 players haven't connected wait for connection
        if (concurrentPlayers < 2)
        {
            return;
        }

        Debug.Log("CP = " + concurrentPlayers);

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

        if(numberOfWaves % waveMultiplier == 0)
        {
            // Add more health / dmg red to creeps
            EnemyHealth creepHealth = createdEnemy.GetComponent<EnemyHealth>();
            creepHealth.AddToMaXHealth(creepHealthAdditive);
            creepHealth.IncreaseDamageReduction(creepDamageRed);
        }

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
