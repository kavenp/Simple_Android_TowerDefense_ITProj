using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MP_PlayerController : NetworkBehaviour
{
	// Builder movement variables
	public float turningSpeed;
	public float movingSpeed;

	// Current buildable tile
	private GameObject currentBuildableTile = null;

	// Towers
	public GameObject tower;

	void Update ()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * turningSpeed;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * movingSpeed;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		// Hit build button
		if (Input.GetKeyDown (KeyCode.Space))
		{
			ConstructTower ();
		}
	}

	void ConstructTower ()
	{
		Vector3 down = transform.TransformDirection (Vector3.down);
		RaycastHit hit;
		Ray ray = new Ray (transform.position, down);

		if (Physics.Raycast (ray, out hit, 5))
		{
			if (hit.collider.tag == "BS")
			{
				currentBuildableTile = hit.collider.gameObject;
				MP_TileBuildTower bt = currentBuildableTile.GetComponent<MP_TileBuildTower> ();
				Transform tt = hit.collider.transform;
				bt.BuildTower (tt);
			}
			else
			{
				currentBuildableTile = null;	
			}

			Debug.Log (hit.collider.ToString ());

		}
	}

	public override void OnStartLocalPlayer ()
	{
		Color orange = new Color (178 / 255.0f, 115 / 255.0f, 0, 1);
		GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", orange);
	}
}
