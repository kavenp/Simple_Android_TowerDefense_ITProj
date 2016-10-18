using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

/// Class that is responsible for starting and managing the game from start to finish
public class MP_GameCoordinator : NetworkBehaviour
{
    // Enemy object and position
    public GameObject enemy;
    public Vector3 spawnValues;

    // Wave variables to control how the enemies are spawned
    public int numberOfWaves;
    public int numberOfEnemiesPerWave;
    public float spawnWait;
    public float waveWait;
    private int waveMultiplier = 2;

    // Enemy scaling
    private int creepHealthAdditive = 10;
    private const float creepDamageRed = 0.05f;

    // Check to see if game is over
    public static bool IQUIT = false;

    // Number of players
    int concurrentPlayers;

    // Flags to see if there is a wave and if it is spawning
    bool isSpawning = false;
    bool isWave = false;

    // Number of players before the game is started
    int playable = 2;

    // The state of the game
    bool gameStarted = false;

    void Update()
    {
        // Get number of concurrent players
        concurrentPlayers = GameObject.FindGameObjectsWithTag("Player").Length;

        // If 2 players haven't connected wait for connection
        if (concurrentPlayers < playable)
        {
            return;
        }

        // Set status of game
        gameStarted = true;

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
		    NetworkManager nm = NetworkManager.singleton;
			nm.ServerChangeScene("GameOver");
            // This is only host side only
            //SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }

        // Check for disconected players - Alternative method
        //CheckDC(concurrentPlayers, playable);
    }

    // Spawn enemy wave
    IEnumerator SpawnEnemyWave(float seconds)
    {
        // Wait before spawning
        yield return new WaitForSeconds(seconds);

        // Create the enemy
        Vector3 spawnPosition = new Vector3(spawnValues.x, spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        var createdEnemy = (GameObject)Instantiate(enemy, spawnPosition, spawnRotation);

        // Server spawns the enemy in both clients
        NetworkServer.Spawn(createdEnemy);

        // Finished spawning wave, total waves is reduced by 1
        isSpawning = false;
        numberOfEnemiesPerWave -= 1;
    }

    // Spawn next wave
    IEnumerator SpawnNextWave(float seconds)
    {
        // Wait before spawning
        yield return new WaitForSeconds(seconds);

        // Set flags to false
        isWave = false;
        numberOfWaves -= 1;

        // Increase the enemy health per wave
        EnemyHealth creepHealth = enemy.GetComponent<EnemyHealth>();
        creepHealth.AddToMaXHealth(creepHealthAdditive);
        creepHealth.IncreaseDamageReduction(creepDamageRed);

        // This is terrible but can fix it later on
        numberOfEnemiesPerWave = 5;
    }

    // Method to check DC - Alternative method
    public void CheckDC(int concurrentPlayers, int playable)
    {
        // Check again if theres less than 2 players
        if(concurrentPlayers < playable)
        {
            Debug.Log("Someone left");
            //Debug.Log(Network.connections);
            MP_PlayerController lonePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<MP_PlayerController>();
            lonePlayer.CmdSetDisconnectCanvas();
        }
    }

    // Wait function
    IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(3);
    }

    // Check for unexpected disconnection
    public void Disconnect(ClientConnection clientConnection)
    {
        MP_PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<MP_PlayerController>();
        p.SetDisconnectCanvas();

        // Wait for cmd sync
        StartCoroutine(Waiting());

        Network.Disconnect();
        MasterServer.UnregisterHost();
        clientConnection.End();
        NetworkManager.singleton.StopHost();
        Destroy(GameObject.Find("NetworkManager"));
    }

    // Check for unexpected disconnection
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        GameObject[] notifyPlayers = GameObject.FindGameObjectsWithTag("Player");
        MP_PlayerController mpc;

        // Game is hosting and not using unity services
        if(!Network.isServer)
        {
            Debug.Log("Server connection disconnected");
            for(int i = 0; i < notifyPlayers.Length; i += 1)
            {
                mpc = notifyPlayers[i].GetComponent<MP_PlayerController>();
                mpc.CmdQuitObject();
            }

        }
        // Game has lost connection
        else if (info == NetworkDisconnection.LostConnection)
        {
            Debug.Log("Lost connection to server");
            for(int i = 0; i < notifyPlayers.Length; i += 1)
            {
                mpc = notifyPlayers[i].GetComponent<MP_PlayerController>();
                mpc.CmdQuitObject();
            }
        }
        else
        {
            Debug.Log("Disconnected from server");
            for(int i = 0; i < notifyPlayers.Length; i += 1)
            {
                mpc = notifyPlayers[i].GetComponent<MP_PlayerController>();
                mpc.CmdQuitObject();
            }
        }
    }

    // Get the state of the game
    public bool isGameStarted()
    {
        return this.gameStarted;
    }

    // Player has disconnected
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    // Get the quit status of the game coordinator
    public bool GetQuitStatus()
    {
        return MP_GameCoordinator.IQUIT;
    }
}
