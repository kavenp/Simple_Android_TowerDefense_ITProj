using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour
{
	// States
	private bool upButton;
	private bool downButton;
	private bool leftButton;
	private bool buildButton;
	private bool sellButton;

	void Start ()
	{
		this.upButton = false;
		this.downButton = false;
		this.leftButton = false;
		this.buildButton = false;
		this.sellButton = false;
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
		return (this.leftButton);
	}

	public bool BuildButtonPressed ()
	{
		return this.buildButton;
	}

	public bool SellButtonPressed ()
	{
		return this.sellButton;
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

	public void SellButtonOn ()
	{
		sellButton = true;
	}

	public void SellButtonOff ()
	{
		sellButton = false;
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
