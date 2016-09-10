using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MP_TileBuildTower :NetworkBehaviour
{
	private bool isFree = true;
	private GameObject createdTower = null;
	private int owner;

	// Need to make this a list of towers
	private GameObject tower;

	void Awake ()
	{
		// Load tower resources
		tower = Resources.Load ("Tower") as GameObject;

		// Owner
		setOwner (-1);
	}

	public void BuildTower (Transform tt, int playerID, int towerType)
	{
		if (isTileFree ())
		{
			// Set the owner of the tile to the player
			setOwner (playerID);

			// Create the tower and get the server to spawn it
			Vector3 towerPosition = new Vector3 (this.transform.position.x, tower.transform.position.y, this.transform.position.z);
			createdTower = (GameObject) Instantiate (tower, towerPosition, tower.transform.rotation);
			NetworkServer.Spawn (createdTower);

			// Set the tile to occupied
			setTileFree (false);
		}
		else
		{
			// Put UI text saying its currently unbuildable
			Debug.Log ("Theres currently a tower on this tile");
		}
	}

	private void setOwner (int newOwner)
	{
		this.owner = newOwner;
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
