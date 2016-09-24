using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class replayShootEnemies : MonoBehaviour
{
	public List<GameObject> inRange;
	//list of enemies in range
	public GameObject projectilePrefab;
	//for instantiating projectile
	private float lastShotTime;
	//time of last shot
	//private GameController gc;
	static private double fireRate = 1;

    private Vector3 previousTargetLoc;

	// Use this for initialization
	void Start ()
	{
		//gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		inRange = new List<GameObject> ();
		lastShotTime = Time.time;
	}

	void OnEnemyDestroy (GameObject enemy)
	{
		inRange.Remove (enemy);
		//once destroyed remove enemy from list
	}

	//Shoot the enemy
	//[Command]
	void Shoot (GameObject target)
	{
		Vector3 startloc = gameObject.transform.position;
		Vector3 targetloc = target.transform.position;

        if (targetloc.Equals(previousTargetLoc))
        {
            inRange.Remove(target);
            return;
        }

		//get starting and target positions
		GameObject newBullet = (GameObject) Instantiate (projectilePrefab, startloc, projectilePrefab.transform.rotation);

		//instantiate the new bullet
		replayBulletBehaviour behavior = (replayBulletBehaviour) newBullet.GetComponent ("replayBulletBehaviour");
		//behavior.gc = gc;
		behavior.target = target;
		behavior.startpos = startloc;
		behavior.targetpos = targetloc;
        previousTargetLoc = targetloc;
        //NetworkServer.Spawn(newBullet);
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag.Equals ("Enemy"))
		{
			inRange.Add (other.gameObject);
			EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate> ();
			dele.enemyDelegate += OnEnemyDestroy;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag.Equals ("Enemy"))
		{
			inRange.Remove (other.gameObject);
			EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate> ();
			dele.enemyDelegate -= OnEnemyDestroy;
		}
	}

	void Update ()
	{
		GameObject target = null;
		//initialize null target
		//float minDist = float.MaxValue;
		if (inRange.Count != 0)
		{
			target = inRange [0];
		}
		//choose first enemy in list to be target if list is non-empty
		if (target != null)
		{
			//start shooting if target exists
			if (Time.time - lastShotTime > fireRate)
			{
				//shoot if the time between now and last shot is larger than set fire rate
				Shoot (target);
				lastShotTime = Time.time;
				//update time of last shot
			}
		}
	}


	//	void OnTriggerStay (Collider other) {
	//		if (other.gameObject.tag.Equals ("Enemy")) {
	//			inRange.Add (other.gameObject);
	//			EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate>();
	//			dele.enemyDelegate += OnEnemyDestroy;
	//		}
	//	}
}
