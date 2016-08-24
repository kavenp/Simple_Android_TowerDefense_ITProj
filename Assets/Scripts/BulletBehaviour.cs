﻿using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {
	private int damage = 25;
	//bullet damage;
	public float speed = 10;
	//bullets speed
	public GameObject target = null;
	//bullets target (Enemy)
	public Vector3 startpos;
	//bullets start position
	public Vector3 targetpos;
	//bullets target position

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
	}

	// Self destructs once enemy is detected to be destroyed
	void OnEnemyDestroy(GameObject target) {
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		float timeInt = Time.time - startTime;
		//time interval from start to current frame
		gameObject.transform.position = Vector3.Lerp (startpos, targetpos, timeInt * speed / distance);
		//linearly interpolating from starting position to target considering speed
		if (gameObject.transform.position.Equals (targetpos)) {
			//if position reaches target
			if (target != null) {
				//check if target still exists
				enemyHP.Hit(damage);
				if(enemyHP.health <= 0) {
					Destroy (target);
					//destroyed if no health left
				}
			}
			//otherwise target is already destroyed so just destroy bullet
			Destroy (gameObject);
		}
	}
}