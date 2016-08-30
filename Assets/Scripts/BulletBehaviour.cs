using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {
	private int damage = 10;
	//bullet damage;
	public float speed = 5;
	//bullets speed
	public GameObject target = null;
	//bullets target (Enemy)
	public Vector3 startpos;
	//bullets start position
	public Vector3 targetpos;
    //bullets target position

    public IGameController gc;
	//game controller, interfaces do not appear in Unity inspector
	private EnemyHealth enemyHP;
	//enemy HP
	private float distance;
	//distance from target
	private float startTime;
	//when bullet first shot

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		distance = Vector3.Distance (startpos, targetpos);
		//EnemyDestructionDelegate dele = target.gameObject.GetComponent<EnemyDestructionDelegate>();
		//dele.enemyDelegate += OnEnemyDestroy;
		enemyHP = (EnemyHealth)target.GetComponent("EnemyHealth");
        GameObject gcObject =
            GameObject.FindGameObjectWithTag("GameController");

        gc = gcObject.GetComponent<IGameController>();

    }

	// Self destructs once enemy is detected to be destroyed
	void OnEnemyDestroy(GameObject target) {
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		float timeInt = Time.time - startTime;
		//time interval from start to current frame
		if (!gc.isGamePaused ()) {
			gameObject.transform.position = Vector3.Lerp (startpos, targetpos, timeInt * speed / distance);
			//linearly interpolating from starting position to target considering speed
			startpos = gameObject.transform.position;
			if (target != null) {
				targetpos = target.transform.position;
			} else {
				Destroy (gameObject);
				//target is null so destroy bullet;
			}
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

	void OnTriggerEnter (Collider other) {
		if (other.gameObject == target) 
		{
			enemyHP.Hit (damage);
			if(enemyHP.health <= 0)
			{		
				Destroy (other.gameObject);
			}
			Destroy (gameObject);
		}
	}

}