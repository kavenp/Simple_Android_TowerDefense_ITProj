using UnityEngine;
using System.Collections;

public class MP_TileBuildTower : MonoBehaviour
{
	private bool isFree = true;
	private Object createdTower = null;

	// Need to make this a list of towers
	private GameObject tower;

	void Awake ()
	{
		tower = Resources.Load ("Tower") as GameObject;
	}

	public void BuildTower (Transform tt)
	{
		if (isFree == true)
		{
			// Gold stuff

			Vector3 towerPosition = new Vector3 (this.transform.position.x, tower.transform.position.y, this.transform.position.z);
			createdTower = Instantiate (tower, towerPosition, tower.transform.rotation);
			isFree = false;
		}
		else
		{
			Debug.Log ("Theres currently a tower on this tile");
		}
	}

	public void SellTower ()
	{

	}
}
