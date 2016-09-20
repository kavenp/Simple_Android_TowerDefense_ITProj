using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour
{
	//initialize delegate
	public delegate void EnemyDelegate(GameObject enemy);
	public EnemyDelegate enemyDelegate;

	//tells all listeners upon destruction of enemy
	void OnDestroy() {
		if (enemyDelegate != null)
		{
			enemyDelegate (gameObject);
		}

		// Find all players and give them gold
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Ensure game has not ended
        if (players != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                MP_PlayerController pc =
                    players[i].GetComponent<MP_PlayerController>();

                pc.AddGold(10);
            }
        }
	}
}

