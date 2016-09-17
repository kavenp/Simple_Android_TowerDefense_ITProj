using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections;

public class MP_PlayerController : NetworkBehaviour
{
	// Builder movement variables
	public float turningSpeed;
	public float movingSpeed;
	public bool leftButtonClicked;

	// Current buildable tile
	private GameObject currentBuildableTile = null;

	// Towers
	public GameObject tower;

	// Direction
	int forward = 1;
	int backward = -1;

	// Buttons
	GameObject buttons;
	ViewController vc;

	void Start()
	{
		buttons = GameObject.FindGameObjectWithTag("Buttons");
		vc      = buttons.GetComponent<ViewController>();
	}

	void Update ()
	{
		// Check that is local player
		if (!isLocalPlayer)
		{
			return;
		}

        //DebugMove();
		ButtonActions();
	}

	[Command]
	public void CmdConstructTower ()
	{
		Vector3 down = transform.TransformDirection (Vector3.down);
		RaycastHit hit;
		Ray ray = new Ray (transform.position, down);

		// Raycast beneath builder
		if (Physics.Raycast (ray, out hit, 10))
		{
			// Hit a buildable surface
			if (hit.collider.tag == "BS")
			{
				currentBuildableTile = hit.collider.gameObject;

				Debug.Log (currentBuildableTile);

				// Get the script build tower and build tower
				MP_TileBuildTower build_script = currentBuildableTile.GetComponent<MP_TileBuildTower> ();
				build_script.BuildTower (this.gameObject.GetInstanceID (), 0);

			}
			else
			{
				currentBuildableTile = null;
			}


			//Debug.Log (hit.collider.ToString ());
		}
	}

	void DebugMove ()
	{
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

	public void ButtonActions()
	{
		if (vc.BuildButtonPressed())
		{
			CmdConstructTower ();
		}

		// Perform state analysis
		if (vc.RotateButtonPressed())
		{
			ButtonRotate (backward);
		}

		if (vc.UpButtonPressed())
		{
			ButtonTranslate (forward);
		}

		if (vc.DownButtonPressed())
		{
			ButtonTranslate (backward);
		}
	}

	public void ButtonTranslate (float verticalInput)
	{
		var z = verticalInput * Time.deltaTime * movingSpeed;
		gameObject.transform.Translate (0, 0, z);
	}

	public void ButtonRotate (float horizontalInput)
	{
		var x = horizontalInput * Time.deltaTime * turningSpeed;
		gameObject.transform.Rotate (0, x, 0);
	}

	public void ConstructTower ()
	{
		CmdConstructTower ();
	}

	public override void OnStartLocalPlayer ()
	{
		Color orange = new Color (178 / 255.0f, 115 / 255.0f, 0, 1);
		GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", orange);
	}
}
