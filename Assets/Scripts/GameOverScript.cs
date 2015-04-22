using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private GUISkin skin;

	public bool GameOverActive = false;
	public RectTransform gameoverMenu;

	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect button2Rect = new Rect(15,15,160,30);

	// Keyboard control
	string[] buttons = new string[2] {"Opnieuw?", "Menu"};	
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

			// Zet game op stil
			Time.timeScale = 0;

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
				Time.timeScale = 1;
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
				Time.timeScale = 1;
				Application.LoadLevel ("MainMenu"); // Load Main Menu
			}
		}		
	}
}
