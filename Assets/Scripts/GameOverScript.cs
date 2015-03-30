using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private GUISkin skin;

	public static bool GameOverActive = false;
	public RectTransform gameoverMenu;

	public static bool PauseActive = false;
	public RectTransform pauzeMenu;

	public static bool WinActive = false;
	public RectTransform finishMenu;

	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect button2Rect = new Rect(15,15,160,30);

	// Keyboard control
	string[] buttons = new string[5] {"Opnieuw?", "Menu", "Terug", "Menu", "Menu"};	
	private int selected = 0;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;

		selected = 0;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			selected = menuSelection(buttons, selected, "up");			
		}
		
		if(Input.GetKeyDown(KeyCode.S))
		{			
			selected = menuSelection(buttons, selected, "down");			
		}
	}
	
	int menuSelection (string[] buttonsArray, int selectedItem, string direction) 
	{		
		if (direction == "up") 
		{			
			if (selectedItem == 0) 
			{				
				selectedItem = buttonsArray.Length - 1;				
			} 
			else 
			{				
				selectedItem -= 1;				
			}			
		}
		
		if (direction == "down") {
			
			if (selectedItem == buttonsArray.Length - 1) 
			{				
				selectedItem = 0;				
			} 
			else 
			{				
				selectedItem += 1;				
			}			
		}
		return selectedItem;		
	}

	void OnGUI()
	{
		button1Rect.x = (Screen.width / 2) - (button1Rect.width / 2);
		button1Rect.y = (Screen.height / 2) - (button1Rect.height / 2);

		button2Rect.x = (Screen.width / 2) - (button2Rect.width / 2);
		button2Rect.y = (Screen.height / 2) - (button2Rect.height / 2);

		GUI.FocusControl(buttons[selected]);

		// Set Menu op actief en daarna op inactief
		if (GameOverActive == true) 
		{
			// Plaatsing buttons
			button1Rect.y = button1Rect.y - 20;
			button2Rect.y = button2Rect.y + 65;

			// Activeer Ingame menu
			gameoverMenu.gameObject.SetActive(true);

			// Set the skin to use
			GUI.skin = skin;

			GUI.SetNextControlName(buttons[0]);
			// Opnieuw Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button1Rect,
				"Opnieuw?"
				)) 
			{
				GameOverActive = false;
				gameoverMenu.gameObject.SetActive(false);
				Application.LoadLevel ("Tutorial"); // Load Totorial
			}

			GUI.SetNextControlName(buttons[1]);
			// Naar Main menu button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button2Rect,
				"Menu"
				)) 
			{
				GameOverActive = false;
				gameoverMenu.gameObject.SetActive(false);
				Application.LoadLevel ("MainMenu"); // Load Main Menu
			}
		}

		// Pauzemenu
		if (PauseActive == true) 
		{
			button1Rect.y = button1Rect.y + 65;
			button2Rect.y = button2Rect.y + 135;
			// Activeer Ingame menu
			pauzeMenu.gameObject.SetActive(true);
			
			// Zet game op stil
			Time.timeScale = 0;
			
			// Set the skin to use
			GUI.skin = skin;

			GUI.SetNextControlName(buttons[0]);
			// Terug Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button1Rect,
				"Terug"
				)) 
			{
				PauseActive = false;
				Time.timeScale = 1;
				pauzeMenu.gameObject.SetActive(false);
			}

			GUI.SetNextControlName(buttons[1]);
			// Naar Main menu button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button2Rect,
				"Menu"
				)) 
			{
				PauseActive = false;
				Time.timeScale = 1;
				pauzeMenu.gameObject.SetActive(false);
				Application.LoadLevel ("MainMenu");
			}
		}

		// Gewonnen menu
		if (WinActive == true) 
		{
			button1Rect.y = button1Rect.y + 75;

			// Activeer Ingame menu
			finishMenu.gameObject.SetActive(true);

			// Pauzeer spel
			Time.timeScale = 0;
			// Set the skin to use
			GUI.skin = skin;

			GUI.SetNextControlName(buttons[0]);
			// Naar main menu Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button1Rect,
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
