﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

/// Class that allows the player to build/sell/upgrade a tower
public class MP_TileBuildTower :NetworkBehaviour
{
	// Tile states
	private bool isFree = true;
	private GameObject createdTower = null;

	// Tower states
	private string towerType;
	private int owner;
    private const int damageIncrease = 5;

    // Need to make this a list of towers
    Dictionary<string, GameObject> towerBuildDict = new Dictionary<string, GameObject>();

	// Tower limits
    private const int upgradeLimit = 5;
    private int currentUpgradeLimit = 0;

    void Awake ()
	{
		// Load tower resources
		towerBuildDict.Add("Tower", Resources.Load ("Tower") as GameObject);
		towerBuildDict.Add("Tower2", Resources.Load ("Tower2") as GameObject);
		towerBuildDict.Add("Tower3", Resources.Load ("Tower3") as GameObject);

		// Owner
		setOwner (-1);
		setTowerType (null);
	}

	/// Build a tower
	public void BuildTower (int playerID, string _towerType, Dictionary<string,int> towerDict, int playerGold)
	{
		// Get cost of tower
		int towerCost;
		towerDict.TryGetValue(_towerType, out towerCost);

		// Calculate gold
		if(playerGold >= towerCost)
		{
			if (isTileFree ())
			{
                GameObject theTower;
				towerBuildDict.TryGetValue(_towerType, out theTower);

                // Set the owner of the tile to the player
                setOwner (playerID);

				// Set the tower type build on this tile
				setTowerType(_towerType);

				// Create the tower and get the server to spawn it
				Vector3 towerPosition = new Vector3 (this.transform.position.x, theTower.transform.position.y, this.transform.position.z);
				createdTower = (GameObject) Instantiate (theTower, towerPosition, theTower.transform.rotation);
				NetworkServer.Spawn (createdTower);

                // Subtract gold - Client side only
                //mpc.AddGold(-towerCost);

                // Set the tile to occupied
                setTileFree (false);
            }
			else
			{
				// Put UI text saying its currently unbuildable
				Debug.Log ("Theres currently a tower on this tile");
			}
		}
		else
		{
			Debug.Log("Not enough gold");
		}
    }

	/// Sell a tower
	public void SellTower (int playerID, Dictionary<string,int> towerDict)
	{
		if(this.towerType != null)
		{
			if(isTileFree() == false)
			{
				// If owner of the tower
				if(playerID == this.owner)
				{
					// Get cost of tower - Client only
					int refundCost;
					towerDict.TryGetValue(this.towerType, out refundCost);

					// Destroy tower and refund it
					Destroy(this.createdTower);

                    // Add gold - flat rate - client side only
                    //playerGold += (int) (0.9 * refundCost);

                    // Set the tile to occupied
                    setTileFree (true);

					// Set the tower type to null
					setTowerType(null);

                    // Set current upgrade limit for this tile to `0`
                    this.currentUpgradeLimit = 0;
				}
			}
		}
	}

	/// Upgrade a tower
	public void UpgradeTower (int playerID, ref bool upgradedTower)
	{
		if(this.towerType != null)
		{
			if(isTileFree() == false)
			{
				// If owner of the tower
				if(playerID == this.owner)
				{
                    if (currentUpgradeLimit < upgradeLimit)
                    {
                        // Modify tower's shooting script
                        ShootEnemies towerShooting = createdTower.GetComponent<ShootEnemies>();
                        towerShooting.AddAdditionalDamage(damageIncrease);
                        towerShooting.level++;

                        // Increase upgrade limit and tell playe
                        // tower has been upgraded
                        currentUpgradeLimit += 1;
                    }
                    else
                    {
                        Debug.Log("Current upgrade limit reached for this tower");
                    }
                }
			}
		}
	}

	// Set owner of tower
	private void setOwner (int newOwner)
	{
		this.owner = newOwner;
	}

	// Set type of tower
	private void setTowerType (string towerType)
	{
		this.towerType = towerType;
	}

	// Set status of tile
	private void setTileFree (bool flag)
	{
		this.isFree = flag;
	}

	// Check status of tile
	private bool isTileFree ()
	{
		return this.isFree;
	}
}