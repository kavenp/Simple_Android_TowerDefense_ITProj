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

    private int creepHealthAdditive = 10;
    private const float creepDamageRed = 0.05f;


    // Number of players
    int concurrentPlayers;

    bool isSpawning = false;
    bool isWave = false;

    int playable = 2;

    void Update()
    {
        // Get number of concurrent players
        concurrentPlayers = GameObject.FindGameObjectsWithTag("Player").Length;

        // If 2 players haven't connected wait for connection
        if (concurrentPlayers < playable)
        {
            return;
        }

        // At this point, if the host is still playing and the client has disconnectede the game will still resume
        playable = 1;

        // Spawn wave management
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

        EnemyHealth creepHealth = enemy.GetComponent<EnemyHealth>();
        creepHealth.AddToMaXHealth(creepHealthAdditive);
        creepHealth.IncreaseDamageReduction(creepDamageRed);

        // This is terrible but can fix it later on
        numberOfEnemiesPerWave = 5;
    }
}
