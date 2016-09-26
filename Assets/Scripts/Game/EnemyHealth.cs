using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyHealth : NetworkBehaviour
{
	public const int startingMaxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	private int currentHealth = startingMaxHealth;

    [SerializeField]
	public int health;

	[SerializeField]
	public int maxHealth;


    private float dmg_red = 1;
    public Image healthBar;
	// Use this for initialization
	void Start () {
		currentHealth = startingMaxHealth;
        health 		  = currentHealth;
        maxHealth     = startingMaxHealth;
        //healthBar     = transform.FindChild("EnemyCanvas").GetComponent<Image>();
        healthBar = GameObject.FindGameObjectWithTag("EnemyCanvas").GetComponent<Image>();
    }

	// Update is called once per frame
	void Update ()
	{

	}

	public void AddToMaXHealth(int health)
	{
        //startingMaxHealth += health;
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

	public void Hit (int damage)
	{
		if(!isServer)
		{
            return;
        }

        currentHealth = (int) (currentHealth - (dmg_red * damage));
		if (currentHealth <= 0)
		{
            RpcDestroy();
        }


        //health = (int) (health - (dmg_red * damage));
		//healthBar.fillAmount = (float)(health)/(float)(maxHealth);
	}

	void OnChangeHealth(int currentHealth)
	{
		healthBar.fillAmount = (float)(currentHealth)/(float)(maxHealth);
	}

	[ClientRpc]
	void RpcDestroy()
	{
        Debug.Log("I get here");
        MP_PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<MP_PlayerController>();
        pc.AddGold(10);
        Destroy(gameObject);
    }
}
