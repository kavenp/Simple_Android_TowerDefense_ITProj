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
			CmdConstructTower ();
		}
	}

	[Command]
	void CmdConstructTower ()
	{
		Vector3 down = transform.TransformDirection (Vector3.down);
		RaycastHit hit;
		Ray ray = new Ray (transform.position, down);

		// Raycast beneath builder
		if (Physics.Raycast (ray, out hit, 5))
		{
			// Hit a buildable surface
			if (hit.collider.tag == "BS")
			{
				currentBuildableTile = hit.collider.gameObject;

				// Get the script build tower and build tower
				MP_TileBuildTower build_script = currentBuildableTile.GetComponent<MP_TileBuildTower> ();
				Transform tower_transform = hit.collider.transform;
				build_script.BuildTower (tower_transform, this.gameObject.GetInstanceID (), 0);
			}
			else
			{
				currentBuildableTile = null;	
			}

			//Debug.Log (hit.collider.ToString ());

		}
	}

	public override void OnStartLocalPlayer ()
	{
		Color orange = new Color (178 / 255.0f, 115 / 255.0f, 0, 1);
		GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", orange);
	}
}
