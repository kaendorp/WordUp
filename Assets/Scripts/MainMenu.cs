using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private GUISkin skin;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;
	}
	
	void OnGUI()
	{
		const int buttonWidth = 160;
		const int buttonHeight = 30;
		
		// Set the skin to use
		GUI.skin = skin;
		
		// Start Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 7) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"START"
		)) {
			// On Click, load the first level.
			Application.LoadLevel ("LevelOne"); // Load Level One
		}

		// Prestaties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 5.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Prestaties"
		)) {
			// On Click, load the first level.
			Debug.Log ("Prestaties"); // Load Prestaties Scene
		}

		// Opties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Opties"
		)) {
			// On Click, load the first level.
			Debug.Log ("Opties"); // Load Opties Scene
		}
		
		// Exit Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3.8f) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Exit"
		)) {
			// On Click, load the first level.
			Application.Quit();  // Exit game
		}
	}
}
