using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameController : NetworkBehaviour
{
	public GameObject enemy;
	public Vector3 spawnValues;

	public int numberOfWaves;
	public int numberOfEnemiesPerWave;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	void Awake ()
	{
		this.enemy = GameObject.FindGameObjectWithTag ("Enemy");
	}

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
