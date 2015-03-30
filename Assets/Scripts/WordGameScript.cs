using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WordGameScript : MonoBehaviour {

	private GUISkin skin;
	private GUISkin menuskin;

	public Image controle;

	public RectTransform letter1;
	public RectTransform letter2;
	public RectTransform letter3;

	public Text letter1text;
	public Text letter2text;
	public Text letter3text;

	public static bool Active = false;
	public static bool WGcomplete = false;
	public RectTransform Wordgame;

	private int klik;

	// Buttons plaatsing
	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect letter1Rect = new Rect (15, 15, 50, 50);
	private Rect letter2Rect = new Rect (15, 15, 50, 50);
	private Rect letter3Rect = new Rect (15, 15, 50, 50);

	// Keyboard control
	string[] buttons = new string[4] {"F", "L", "E", "Correct"};	
	private int selected = 0;

	// Use this for initialization
	void Start ()
	{
		skin = Resources.Load("WordgameButton") as GUISkin;
		menuskin = Resources.Load("ButtonSkin") as GUISkin;

		selected = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (letter1text.text == "L" && letter2text.text == "E" && letter3text.text == "F") 
		{
			controle.color = new Color32(19, 182, 40, 225);
			WGcomplete = true;
		} 
		else if (klik == 3) 
		{
			controle.color = new Color32(250, 66, 66, 225);

			letter1text.text = "";
			letter2text.text = "";
			letter3text.text = "";
			klik = 0;
		}

		if(Input.GetKeyDown(KeyCode.D))
		{			
			selected = menuSelection(buttons, selected, "up");			
		}
		
		if(Input.GetKeyDown(KeyCode.A))
		{
			
			selected = menuSelection(buttons, selected, "down");			
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			
			selected = menuSelection(buttons, selected, "done");			
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
		if (direction == "done") 
		{
			selectedItem = 3;
		}
		return selectedItem;		
	}

	void OnGUI()
	{
		button1Rect.x = (Screen.width / 2) - (button1Rect.width / 2);
		button1Rect.y = (Screen.height / 2) - (button1Rect.height / 2);

		letter1Rect.x = (Screen.width / 2) - (letter1Rect.width / 2);
		letter1Rect.y = (Screen.height / 2) - (letter1Rect.height / 2);

		letter2Rect.x = (Screen.width / 2) - (letter2Rect.width / 2);
		letter2Rect.y = (Screen.height / 2) - (letter2Rect.height / 2);

		letter3Rect.x = (Screen.width / 2) - (letter3Rect.width / 2);
		letter3Rect.y = (Screen.height / 2) - (letter3Rect.height / 2);

		GUI.FocusControl(buttons[selected]);
		if (Active == true) 
		{
			letter1Rect.x = letter1Rect.x + 55;
			letter1Rect.y = letter1Rect.y + 70;

			letter2Rect.y = letter2Rect.y + 70;

			letter3Rect.x = letter3Rect.x - 55;
			letter3Rect.y = letter3Rect.y + 70;

			Time.timeScale =0;

			Wordgame.gameObject.SetActive(true);
			
			// Set the skin to use
			GUI.skin = skin;

			GUI.SetNextControlName(buttons[0]);
			// Eerste letter
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				letter1Rect,
				"F"
				)) 
			{
				klik += 1;
				if (letter1text.text == "")
				{
					letter1text.text = "F";
				}
				else if (letter2text.text == "")
				{
					letter2text.text = "F";
				}
				else if (letter3text.text == "")
				{
					letter3text.text = "F";
				}
			}
			GUI.SetNextControlName(buttons[1]);
			// Tweede letter
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				letter2Rect,
				"L"
				)) 
			{
				klik += 1;
				if (letter1text.text == "")
				{
					letter1text.text = "L";
				}
				else if (letter2text.text == "")
				{
					letter2text.text = "L";
				}
				else if (letter3text.text == "")
				{
					letter3text.text = "L";
				}
			}

			GUI.SetNextControlName(buttons[2]);
			// Derde letter
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				letter3Rect,
				"E"
				)) 
			{		
				klik += 1;
				if (letter1text.text == "")
				{
					letter1text.text = "E";
				}
				else if (letter2text.text == "")
				{
					letter2text.text = "E";
				}
				else if (letter3text.text == "")
				{
					letter3text.text = "E";
				}
			}
		}

		if (WGcomplete == true) 
		{
			// Plaatsing buttons
			button1Rect.y = button1Rect.y + 125;

			// Selecteert automatisch correct button, als het woord goed is.
			selected = menuSelection(buttons, selected, "done");

			// Set the skin to use
			GUI.skin = menuskin;

			GUI.SetNextControlName(buttons[3]);
			// Correct Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button1Rect,
				"Correct"
				)) 
			{
				Active = false;
				WGcomplete = false;
				letter1text.text = "";
				letter2text.text = "";
				letter3text.text = "";
				Time.timeScale = 1;
				Wordgame.gameObject.SetActive(false);
			}
		}
	}
}
