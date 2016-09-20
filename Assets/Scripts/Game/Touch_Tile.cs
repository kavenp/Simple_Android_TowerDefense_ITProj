using UnityEngine;
using System.Collections;

public class Touch_Tile : MonoBehaviour
{
	public GameObject tower;

	private Object createdTower = null;

	// Game controller
	private GameController gc;

	void Awake ()
	{
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}

	void OnMouseDown ()
	{
		if (createdTower != null)
		{
			return;
		}

		// Only construct tower when game is not paused
		if (gc.isGamePaused () != true)
		{
			Vector3 towerPosition = new Vector3 (this.transform.position.x, tower.transform.position.y, this.transform.position.z);
			createdTower = Instantiate (tower, towerPosition, tower.transform.rotation);
		}
	}

}
