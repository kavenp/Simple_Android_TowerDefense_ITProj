using UnityEngine;

/// Class that updates the status of all the buttons on screen
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
    private bool disconnectButton;
    private bool leaveButton;

	// Set button states to false
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
        this.disconnectButton = false;
        this.leaveButton = false;
    }

	/// When a button is pressed down, set it to true. When up, set it to false

	// Leave button
    public bool LeaveButtonPressed ()
    {
        return this.leaveButton;
    }

	public void LeaveButtonOn ()
    {
        leaveButton = true;
    }

    public void LeaveButtonOff ()
    {
        leaveButton = false;
    }

	// Up button
	public bool UpButtonPressed ()
	{
		return this.upButton;
	}

	public void UpButtonOn ()
	{
		upButton = true;
	}

	public void UpButtonOff ()
	{
		upButton = false;
	}

	// Down button
	public bool DownButtonPressed ()
	{
		return this.downButton;
	}

	public void DownButtonOn ()
	{
		downButton = true;
	}

	public void DownButtonOff ()
	{
		downButton = false;
	}

	// Rotate button
	public bool RotateButtonPressed ()
	{
		return (this.leftButton);
	}

	public void LeftButtonOn ()
	{
		leftButton = true;
	}

	public void LeftButtonOff ()
	{
		leftButton = false;
	}

	// Build button
	public bool BuildButtonPressed ()
	{
		return this.buildButton;
	}

	public void BuildButtonOn ()
	{
		buildButton = true;
	}

	public void BuildButtonOff ()
	{
		buildButton = false;
	}

	// Build 2 button
	public bool BuildButton2Pressed ()
	{
		return this.buildButton2;
	}

	public void BuildButton2On ()
	{
		buildButton2 = true;
	}

	public void BuildButton2Off ()
	{
		buildButton2 = false;
	}

	// Build 3  button
	public bool BuildButton3Pressed ()
	{
		return this.buildButton3;
	}

	public void BuildButton3On ()
	{
		buildButton3 = true;
	}

	public void BuildButton3Off ()
	{
		buildButton3 = false;
	}

	// Upgrade button
	public bool UpgradeButtonPressed ()
	{
		return this.upgradeButton;
	}

	public void UpgradeButtonOn ()
	{
		upgradeButton = true;
	}

	public void UpgradeButtonOff ()
	{
		upgradeButton = false;
	}

	// Sell button
	public bool SellButtonPressed ()
	{
		return this.sellButton;
	}

	public void SellButtonOn ()
	{
		sellButton = true;
	}

	public void SellButtonOff ()
	{
		sellButton = false;
	}

	// Disconnect button
	public bool DisconnectButtonPressed ()
	{
        return this.disconnectButton;
    }

	public void DisconnectButtonOn ()
	{
        disconnectButton = true;
    }

	public void DisconnectButtonOff ()
	{
        disconnectButton = false;
    }
}
