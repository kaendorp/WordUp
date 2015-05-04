using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public GameObject level;
    public GameObject speler;
    
    private GUISkin skin;

	// Keyboard control
	string[] buttons = new string[6] { "Start", "Level", "Speler", "Prestaties", "Opties", "Exit" };	
	private int selected = 0;

	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect button2Rect = new Rect(15,15,160,30);
	private Rect button3Rect = new Rect(15,15,160,30);
	private Rect button4Rect = new Rect(15,15,160,30);
    private Rect button5Rect = new Rect(15,15,160,30);
    private Rect button6Rect = new Rect(15,15,160,30);

	void Start()
	{
        level.SetActive(false);
        speler.SetActive(false);

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

		button3Rect.x = (Screen.width / 2) - (button3Rect.width / 2);
		button3Rect.y = (Screen.height / 2) - (button3Rect.height / 2);
		
		button4Rect.x = (Screen.width / 2) - (button4Rect.width / 2);
		button4Rect.y = (Screen.height / 2) - (button4Rect.height / 2);

        button5Rect.x = (Screen.width / 2) - (button5Rect.width / 2);
        button5Rect.y = (Screen.height / 2) - (button5Rect.height / 2);
        
        button6Rect.x = (Screen.width / 2) - (button6Rect.width / 2);
        button6Rect.y = (Screen.height / 2) - (button6Rect.height / 2);

		button1Rect.y = button1Rect.y - 130;
        button2Rect.y = button2Rect.y - 90;
		button3Rect.y = button3Rect.y - 50;
		button4Rect.y = button4Rect.y - 10;
        button5Rect.y = button5Rect.y + 30;
        button6Rect.y = button6Rect.y + 70;
		
		// Set the skin to use
		GUI.skin = skin;

		GUI.SetNextControlName(buttons[0]);
		// Start Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			button1Rect,
			"START"
		)) 
		{
            GameControl.control.isMainMenu = false;
			Application.LoadLevel (GameControl.control.loadLevel); // Load Intro
		}

        GUI.SetNextControlName(buttons[1]);
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button2Rect,
            "Speler"
        ))
        {
            speler.SetActive(true); // Select Speler
            this.gameObject.SetActive(false);
        }

        GUI.SetNextControlName(buttons[2]);
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button3Rect,
            "Level"
        ))
        {            
            level.SetActive(true);// Select Level
            this.gameObject.SetActive(false);
        }


		GUI.SetNextControlName(buttons[3]);
		// Prestaties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			button4Rect,
			"Prestaties"
		)) 
		{
             // Load Prestaties Scene
		}

		GUI.SetNextControlName(buttons[4]);
		// Opties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			button5Rect,
			"Opties"
		)) 
		{		
			Debug.Log ("Opties"); // Load Opties Scene
		}

		GUI.SetNextControlName(buttons[5]);
		// Exit Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			button6Rect	,
			"Exit"
		)) 
		{
			Application.Quit();  // Exit game
		}
		GUI.FocusControl(buttons[selected]);
	}
}
