using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnMainMenu : MonoBehaviour
{
	// Return to main menu when clicked
	public void OnClick()
	{
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}