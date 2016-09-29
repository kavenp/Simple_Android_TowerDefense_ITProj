using UnityEngine;

public class ViewController : MonoBehaviour
{
	// States
	private bool upButton;
	private bool downButton;
	private bool leftButton;
	private bool buildButton;
	private bool buildButton2;
    private bool buildButton3;
    private bool sellButton;

    private bool upgradeButton;
    void Start ()
	{
		this.upButton = false;
		this.downButton = false;
		this.leftButton = false;
		this.buildButton = false;
		this.buildButton2 = false;
		this.buildButton3 = false;
		this.sellButton = false;
        this.upgradeButton = false;
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

	public bool BuildButton2Pressed ()
	{
		return this.buildButton2;
	}

	public bool BuildButton3Pressed ()
	{
		return this.buildButton3;
	}

	public bool UpgradeButtonPressed ()
	{
		return this.upgradeButton;
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

	public void BuildButton2On ()
	{
		buildButton2 = true;
	}

	public void BuildButton2Off ()
	{
		buildButton2 = false;
	}

	public void BuildButton3On ()
	{
		buildButton3 = true;
	}

	public void BuildButton3Off ()
	{
		buildButton3 = false;
	}

	public void UpgradeButtonOn ()
	{
		upgradeButton = true;
	}

	public void UpgradeButtonOff ()
	{
		upgradeButton = false;
	}
}
