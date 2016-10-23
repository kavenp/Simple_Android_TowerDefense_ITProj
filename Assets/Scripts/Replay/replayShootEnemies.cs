// Script copied from the ShootEnemies script, but removed components that didn't work with replay
// Please don't count this towards a total lines of code count

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

    private int additionalDamage = 0;
    private Vector3 previousTargetLoc;
	
    public int level;

    // Use this for initialization
    void Start()
    {
        inRange = new List<GameObject>();
        lastShotTime = Time.time;

        // Get the type of tower and modify its starting damage values upon creation
        if (this.tag == "Tower")
        {
            this.additionalDamage = 0;
        }

        if (this.tag == "Tower2")
        {
            this.additionalDamage = 12;
        }

        if (this.tag == "Tower3")
        {
            this.additionalDamage = 23;
        }

        level = 1;
    }

    void OnEnemyDestroy(GameObject enemy)
    {
        inRange.Remove(enemy);
        //once destroyed remove enemy from list
    }

    //Shoot the enemy
    void Shoot(GameObject target)
    {
        Vector3 startloc = gameObject.transform.position;
        Vector3 targetloc = target.transform.position;

        if (targetloc.Equals(previousTargetLoc))
        {
            inRange.Remove(target);
            return;
        }

        CmdSpawnBullet(startloc, targetloc, target);


        //get starting and target positions
    }


    void CmdSpawnBullet(Vector3 startloc, Vector3 targetloc, GameObject target)
    {
        GameObject newBullet = (GameObject)Instantiate(projectilePrefab, startloc, projectilePrefab.transform.rotation);



        //instantiate the new bullet
        replayBulletBehaviour behavior = (replayBulletBehaviour)newBullet.GetComponent("replayBulletBehaviour");
        //behavior.gc = gc;
        behavior.AddDamageToBullet(this.additionalDamage);
        //behavior.target = target;
        behavior.SetTarget(target);
        behavior.startpos = startloc;
        behavior.targetpos = targetloc;

        previousTargetLoc = targetloc;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            inRange.Add(other.gameObject);
            EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            dele.enemyDelegate += OnEnemyDestroy;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            inRange.Remove(other.gameObject);
            EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            dele.enemyDelegate -= OnEnemyDestroy;
        }
    }

    void Update()
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
                Shoot(target);
                lastShotTime = Time.time;
                //update time of last shot
            }
        }
    }

    public void setAdditionalDamage(int damage)
    {
        this.additionalDamage = damage;
    }
	
	public void setLastShotTime(float time){
		this.lastShotTime = time;
	}

	public int getAdditionalDamage()
	{
	    return this.additionalDamage;
	}
	
	public GameObject getTarget(){
		GameObject target = null;
        //initialize null target
        //float minDist = float.MaxValue;
        if (inRange.Count != 0)
        {
            target = inRange[0];
        }
		return target;
	}

    //	void OnTriggerStay (Collider other) {
    //		if (other.gameObject.tag.Equals ("Enemy")) {
    //			inRange.Add (other.gameObject);
    //			EnemyDestructionDelegate dele = other.gameObject.GetComponent<EnemyDestructionDelegate>();
    //			dele.enemyDelegate += OnEnemyDestroy;
    //		}
    //	}
}