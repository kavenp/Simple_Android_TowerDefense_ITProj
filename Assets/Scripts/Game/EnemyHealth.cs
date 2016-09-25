using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	[SerializeField]
	private int startingMaxHealth = 100;

	[SerializeField]
	public int health = 100;

	[SerializeField]
	public int maxHealth = 100;


    private float dmg_red = 1;
    public Image healthBar;
	// Use this for initialization
	void Start () {
		health    = this.startingMaxHealth;
		maxHealth = this.startingMaxHealth;
		healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void AddToMaXHealth(int health)
	{
        this.startingMaxHealth += health;
    }

	public void IncreaseDamageReduction(float red)
	{
        this.dmg_red -= red;

		// Limit damage reduction
		if(dmg_red < 0.3)
		{
            dmg_red = 0.3f;
        }
    }

	public void Hit (int damage) {
		health = (int) (health - (dmg_red * damage));
		healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}
}
