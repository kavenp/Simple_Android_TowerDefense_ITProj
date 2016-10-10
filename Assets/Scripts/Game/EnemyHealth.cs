using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyHealth : MonoBehaviour
{

	[SerializeField]
	private static int startingMaxHealth = 100;

    [SerializeField]
    private static float startingDmgRed = 1f;

    [SerializeField]
	public int health = 100;

	[SerializeField]
	public int maxHealth = 100;


    private float dmg_red = 1;
    public Image healthBar;
	// Use this for initialization
	void Start () {
        dmg_red = startingDmgRed;
		health    = startingMaxHealth;
		maxHealth = startingMaxHealth;
		healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update ()
	{
	    healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}

	public void AddToMaXHealth(int health)
	{
        startingMaxHealth += health;
    }

	public void IncreaseDamageReduction(float red)
	{
        startingDmgRed -= red;

		// Limit damage reduction
		if(startingDmgRed < 0.3)
		{
            startingDmgRed = 0.3f;
        }
    }

	public void Hit (int damage) {
		health = (int) (health - (dmg_red * damage));
		healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}
}
