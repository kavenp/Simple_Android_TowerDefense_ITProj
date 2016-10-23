// Copied from BulletBehaviour script, but got rid of enemy destruction and networking components
// Please don't count this towards a total lines of code count

using UnityEngine;
using System.Collections;

public class replayBulletBehaviour : MonoBehaviour
{
	private int damage = 3;
	//bullet damage;
	public float speed = 5;
	//bullets speed
	public GameObject target = null;
	//bullets target (Enemy)
	public Vector3 startpos;
	//bullets start position
	public Vector3 targetpos;
	//bullets target position

	//public GameController gc;
	//game controller, interfaces do not appear in Unity inspector
	private EnemyHealth enemyHP;
	//enemy HP
	private float distance;
	//distance from target
	private float startTime;
    //when bullet first shot

    private Vector3 previousPosition;
    //private float existTime = 0;
    //private int maxTime = 5;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;
		distance = Vector3.Distance (startpos, targetpos);
		//EnemyDestructionDelegate dele = target.gameObject.GetComponent<EnemyDestructionDelegate>();
		//dele.enemyDelegate += OnEnemyDestroy;
		enemyHP = (EnemyHealth) target.GetComponent ("EnemyHealth");
		//GameObject gcObject = GameObject.FindGameObjectWithTag ("GameController");

		//gc = gcObject.GetComponent<GameController> ();

	}

	// Self destructs once enemy is detected to be destroyed
	void OnEnemyDestroy (GameObject target)
	{
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update ()
	{
		float timeInt = Time.time - startTime;
		//time interval from start to current frame

		gameObject.transform.position = Vector3.Lerp (startpos, targetpos, timeInt * speed / distance);
		//linearly interpolating from starting position to target considering speed
		startpos = gameObject.transform.position;

        if (startpos.Equals(previousPosition))
        {
            Destroy(gameObject);
        }

        previousPosition = startpos;
        if (target != null)
		{
			targetpos = target.transform.position;
		}
		else
		{
			Destroy (gameObject);
			//target is null so destroy bullet;
		}


//		if (gameObject.transform.position.Equals (targetpos)) {
//			//if position reaches target
//			if (target != null) {
//				//check if target still exists
//				enemyHP.Hit(damage);
//				if(enemyHP.health <= 0) {
//					Destroy (target);
//					//destroyed if no health left
//				}
//			}
//			//otherwise target is already destroyed so just destroy bullet
//			Destroy (gameObject);
//		}
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == target)
		{
			enemyHP.Hit (damage);
			Destroy (gameObject);
		}
	}
	
	public void SetTarget (GameObject _target)
    {
        this.target = _target;
    }



    public void AddDamageToBullet (int damage)
    {
        this.damage += damage;
    }

    public void AddSpeedToBullet (int speed)
    {
        this.speed += speed;
    }
	
	public void setDamage(int damage){
	    this.damage = damage;
	}
	
	public void setSpeed(float speed){
	    this.speed = speed;
	}

}