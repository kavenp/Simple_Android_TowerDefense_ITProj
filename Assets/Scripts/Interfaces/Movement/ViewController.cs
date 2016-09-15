using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{
	// States
	private bool upButton;
	private bool downButton;
	private bool leftButton;
	private bool rightButton;
	private bool buildButton;

	void Start ()
	{
		this.upButton = false;
		this.downButton = false;
		this.leftButton = false;
		this.rightButton = false;
		this.buildButton = false;
	}

	public bool UpButtonPressed ()
	{
		return this.upButton;
	}

	public bool DownButtonPressed ()
	{
		return this.downButton;
	}

	public bool RotateButtonPressed ()
	{
		return (this.leftButton || this.rightButton);
	}

	public bool BuildButtonPressed ()
	{
		return this.buildButton;
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
