using UnityEngine;
using System.Collections;

// Logs the enemy health and the time since health
// was first lost.
public class TestEnemyHealthScript : MonoBehaviour
{
    private GameObject enemy = null;
    private EnemyHealth enemyHealth = null;
    private int previousHealth = 100;
    private float startTime = -1;

    void Update()
    {
        if (enemy == null)
        {
            enemy = GameObject.FindWithTag("Enemy");
            
        }
        else if (enemyHealth == null)
        {
            enemyHealth = enemy.GetComponent<EnemyHealth>();
        }
        else if (enemyHealth.health != previousHealth)
        {
            if (startTime < 0)
            {
                startTime = Time.time;
            }
            float timePassed = Time.time - startTime;
            Debug.Log("Time: " + timePassed +
                ", Health: " + enemyHealth.health);

            previousHealth = enemyHealth.health;
        }
    }
}
