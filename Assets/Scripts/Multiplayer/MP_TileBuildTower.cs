using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MP_TileBuildTower :NetworkBehaviour
{
	private bool isFree = true;
	private GameObject createdTower = null;

	private int towerType;
	private int owner;

	// Tower types and cost
	Dictionary<int, int> towerDict = new Dictionary<int, int>();

	// Need to make this a list of towers
	private GameObject tower;

	void Awake ()
	{
		// Load tower resources
		tower = Resources.Load ("Tower") as GameObject;

		// Initialise Dictionary
		towerDict.Add(0, 20);

		// Owner
		setOwner (-1);
		setTowerType (-1);
	}

	public void BuildTower (int playerID, int _towerType, ref int playerGold)
	{
		// Get cost of tower
		int towerCost;
		towerDict.TryGetValue(_towerType, out towerCost);

		// Calculate gold
		if(playerGold >= towerCost)
		{
			if (isTileFree ())
			{
				// Set the owner of the tile to the player
				setOwner (playerID);

				// Set the tower type build on this tile
				setTowerType(_towerType);

				// Create the tower and get the server to spawn it
				Vector3 towerPosition = new Vector3 (this.transform.position.x, tower.transform.position.y, this.transform.position.z);
				createdTower = (GameObject) Instantiate (tower, towerPosition, tower.transform.rotation);
				NetworkServer.Spawn (createdTower);

				// Subtract gold
				//playerGold -= towerCost;

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

	public void SellTower (int playerID, ref int playerGold)
	{
		if(this.towerType != -1)
		{
			if(isTileFree() == false)
			{
				// If owner of the tower
				if(playerID == this.owner)
				{
					// Get cost of tower
					int refundCost;
					towerDict.TryGetValue(this.towerType, out refundCost);

					// Destroy tower and refund it
					Destroy(this.createdTower);

					// Add gold - flat rate
					//playerGold += (int) (0.9 * refundCost);

					// Set the tile to occupied
					setTileFree (true);

					// Set the tower type to null
					setTowerType(-1);
				}
			}
		}
	}

	private void setOwner (int newOwner)
	{
		this.owner = newOwner;
	}

	private void setTowerType (int towerType)
	{
		this.towerType = towerType;
	}

	private void setTileFree (bool flag)
	{
		this.isFree = flag;
	}

	private bool isTileFree ()
	{
		return this.isFree;
	}

	public void SellTower ()
	{

	}
}
