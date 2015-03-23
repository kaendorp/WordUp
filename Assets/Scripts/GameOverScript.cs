using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private GUISkin skin;
	public static bool MenuActive = false;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;
	}

	void OnGUI()
	{
		// Set Menu op actief en daarna op inactief
		if (MenuActive == true) 
		{
			// Set the skin to use
			GUI.skin = skin;

			const int buttonWidth = 160;
			const int buttonHeight = 30;
			
			// Start Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 5.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Opnieuw?"
				)) 
			{
				MenuActive = false;
				Application.LoadLevel ("Tutorial"); // Load Level One
			}
			
			// Prestaties Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Menu"
				)) 
			{
				MenuActive = false;
				Application.LoadLevel ("MainMenu"); // Load Level One
			}
		}
	}
}
