using UnityEngine;

public class replayViewController : MonoBehaviour
{
	// States
	private bool nextButton;
	private bool prevButton;
	
    void Start ()
	{
		this.nextButton = false;
		this.prevButton = false;
    }

	public bool nextButtonPressed ()
	{
		return this.nextButton;
	}

	public bool prevButtonPressed ()
	{
		return this.prevButton;
	}
	
	public void nextButtonOn ()
	{
	    Debug.Log("Test");
		nextButton = true;
	}

	public void nextButtonOff ()
	{
		nextButton = false;
	}
	
	public void prevButtonOn ()
	{
		prevButton = true;
	}

	public void prevButtonOff ()
	{
		prevButton = false;
	}
}
