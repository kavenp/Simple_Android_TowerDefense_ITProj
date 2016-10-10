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

    public static bool IQUIT = false;

    // Number of players
    int concurrentPlayers;

    bool isSpawning = false;
    bool isWave = false;

    int playable = 2;

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
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }

        CheckDC(concurrentPlayers, playable);
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

    IEnumerator Waiting()
    {
        yield return new WaitForSecondsRealtime(3);
    }

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

    public bool isGameStarted()
    {
        return this.gameStarted;
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }
    public bool GetQuitStatus()
    {
        return MP_GameCoordinator.IQUIT;
    }
}
