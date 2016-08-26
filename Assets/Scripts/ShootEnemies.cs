using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootEnemies : MonoBehaviour
{
    public List<GameObject> inRange;
    //list of enemies in range
    public GameObject projectilePrefab;
    //for instantiating projectile
    private float lastShotTime;
    //time of last shot
	private GameController gc;
    static private double fireRate = 1;

    // Use this for initialization
    void Start ()
    {
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		inRange = new List<GameObject> ();
        lastShotTime = Time.time;
    }

    void OnEnemyDestroy (GameObject enemy)
    {
        inRange.Remove (enemy);
        //once destroyed remove enemy from list
    }

    //Shoot the enemy
    void Shoot (GameObject target)
    {
        Vector3 startloc = gameObject.transform.position;
        Vector3 targetloc = target.transform.position;
        //get starting and target positions
        GameObject newBullet = (GameObject) Instantiate (projectilePrefab, startloc, projectilePrefab.transform.rotation);
        //instantiate the new bullet
        BulletBehaviour behavior = (BulletBehaviour) newBullet.GetComponent ("BulletBehaviour");
		behavior.gc = gc;
		behavior.target = target;
        behavior.startpos = startloc;
        behavior.targetpos = targetloc;
        //set bullet behavior to target
    }

    //	void OnTriggerStay (Collider other) {
    //		if (other.gameObject.tag.Equals ("Enemy")) {
    //			inRange.Add (other.gameObject);
    //			EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate>();
    //			dele.enemyDelegate += OnEnemyDestroy;
    //		}
    //	}

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
            target = inRange[0];
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
}
