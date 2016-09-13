using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{
	// States
	bool upButton;
	bool downButton;
	bool leftButton;
	bool rightButton;
	bool buildButton;

	// Direction
	int forward = 1;
	int backward = -1;

	void Start ()
	{
		this.upButton = false;
		this.downButton = false;
		this.leftButton = false;
		this.rightButton = false;
		this.buildButton = false;
	}

	void FixedUpdate ()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		MP_PlayerController mpc = player.GetComponent<MP_PlayerController> ();

		if (this.buildButton)
		{
			mpc.CmdConstructTower ();
		}

		// Perform state analysis
		if (this.leftButton)
		{
			mpc.ButtonRotate (backward);
		}

		if (this.rightButton)
		{
			mpc.ButtonRotate (forward);
		}

		if (this.upButton)
		{
			mpc.ButtonTranslate (forward);
		}

		if (this.downButton)
		{
			mpc.ButtonTranslate (backward);
		}


	}

	public void UpButtonOn ()
	{
		upButton = true;
	}

	public void UpButtonOff ()
	{
		upButton = false;
	}

	public void DownButtonOn ()
	{
		downButton = true;
	}

	public void DownButtonOff ()
	{
		downButton = false;
	}

	public void LeftButtonOn ()
	{
		leftButton = true;
	}

	public void LeftButtonOff ()
	{
		leftButton = false;
	}

	public void RightButtonOn ()
	{
		rightButton = true;
	}

	public void RightButtonOff ()
	{
		rightButton = false;
	}

	public void BuildButtonOn ()
	{
		buildButton = true;
	}

	public void BuildButtonOff ()
	{
		buildButton = false;
	}



}
