using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
	// Prefab
	public GameObject enemy;
	public Vector3 spawnValues;

	// Need to add spawn values
	public int numberOfWaves;
	public int numberOfEnemiesPerWave;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	void Update ()
	{
		InvokeRepeating ("SpawnEnemy", 6, 10f);
	}

	void SpawnEnemy ()
	{
		Vector3 spawnPosition = new Vector3 (spawnValues.x, spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity;

		var createdEnemy = (GameObject) Instantiate (enemy, spawnPosition, spawnRotation);
		NetworkServer.Spawn (createdEnemy);

		CancelInvoke ();
	}

	public bool isGamePaused ()
	{
		return false;
	}

	public void setPauseFlag (bool flag)
	{
		
	}
}
