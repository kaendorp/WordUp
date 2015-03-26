using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private GUISkin skin;
	public static bool GameOverActive = false;
	public static bool PauseActive = false;
	public static bool WinActive = false;

	public RectTransform pauzeMenu;
	public RectTransform finishMenu;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;
	}

	void OnGUI()
	{
		// Set Menu op actief en daarna op inactief
		if (GameOverActive == true) 
		{
			// Set the skin to use
			GUI.skin = skin;

			const int buttonWidth = 160;
			const int buttonHeight = 30;
			
			// Opnieuw Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 5.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Opnieuw?"
				)) 
			{
				GameOverActive = false;
				Application.LoadLevel ("Tutorial"); // Load Level One
			}
			
			// Naar Main menu button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Menu"
				)) 
			{
				GameOverActive = false;
				Application.LoadLevel ("MainMenu"); // Load Level One
			}
		}
		// Pauzemenu
		if (PauseActive == true) 
		{
			// Activeer Ingame menu
			pauzeMenu.gameObject.SetActive(true);

			// Zet game op stil
			Time.timeScale = 0;

			// Set the skin to use
			GUI.skin = skin;
			
			const int buttonWidth = 160;
			const int buttonHeight = 30;

			// Terug Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3.25f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Terug"
				)) 
			{
				PauseActive = false;
				Time.timeScale = 1;
				pauzeMenu.gameObject.SetActive(false);
			}
			
			// Naar Main menu button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 2.65f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Menu"
				)) 
			{
				PauseActive = false;
				Time.timeScale = 1;
				pauzeMenu.gameObject.SetActive(false);
				Application.LoadLevel ("MainMenu"); // Load Level One
			}
		}
		// Gewonnen menu
		if (WinActive == true) 
		{
			// Activeer Ingame menu
			finishMenu.gameObject.SetActive(true);

			// Pauzeer spel
			Time.timeScale = 0;
			// Set the skin to use
			GUI.skin = skin;
			
			const int buttonWidth = 160;
			const int buttonHeight = 30;

			// Naar main menu Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3.25f) - (buttonHeight / 2), buttonWidth, buttonHeight),
				"Menu"
				)) 
			{
				WinActive = false;
				Time.timeScale = 1;
				finishMenu.gameObject.SetActive(false);
				Application.LoadLevel ("MainMenu"); // Load Level One
			}	
		}
	}
}
