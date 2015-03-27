using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	private GUISkin skin;

	// Keyboard control
	string[] buttons = new string[4] {"START", "Prestaties", "Opties", "Exit"};	
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
		const int buttonWidth = 160;
		const int buttonHeight = 30;
		
		// Set the skin to use
		GUI.skin = skin;

		GUI.SetNextControlName(buttons[0]);
		// Start Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 7) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"START"
		)) 
		{
			Application.LoadLevel ("Tutorial"); // Load Level One
		}

		GUI.SetNextControlName(buttons[1]);
		// Prestaties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 4) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Prestaties"
		)) 
		{
			Debug.Log ("Prestaties"); // Load Prestaties Scene
		}

		GUI.SetNextControlName(buttons[2]);
		// Opties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3.5f) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Opties"
		)) 
		{		
			Debug.Log ("Opties"); // Load Opties Scene
		}

		GUI.SetNextControlName(buttons[3]);
		// Exit Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			new Rect (Screen.width / 2 - (buttonWidth / 2), (2 * Screen.height / 3.1f) - (buttonHeight / 2), buttonWidth, buttonHeight),
			"Exit"
		)) 
		{
			Application.Quit();  // Exit game
		}

		GUI.FocusControl(buttons[selected]);
	}
}
