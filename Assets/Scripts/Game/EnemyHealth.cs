using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {
	private int maxHealth;
	public int health;
	public Image healthBar;
	// Use this for initialization
	void Start () {
		health = 100;
		maxHealth = 100;
		healthBar = 
			transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	    healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}

	public void Hit (int damage) {
		health -= damage;
		healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}
}
