using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour 
{   
	//initialize delegate
	public delegate void EnemyDelegate(GameObject enemy);
	public EnemyDelegate enemyDelegate;

	//tells all listeners upon destruction of enemy
	void OnDestroy() {
		if (enemyDelegate != null) {
			enemyDelegate (gameObject);
		}
	}
}


