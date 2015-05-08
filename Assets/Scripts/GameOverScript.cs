using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour
{
	private GUISkin skin;

	public bool GameOverActive = false;
	public RectTransform gameoverMenu;

	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect button2Rect = new Rect(15,15,160,30);	

    public string herstartlevel;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;		
	}	

	void OnGUI()
	{
		button1Rect.x = (Screen.width / 2) - (button1Rect.width / 2);
		button1Rect.y = (Screen.height / 2) - (button1Rect.height / 2);

		button2Rect.x = (Screen.width / 2) - (button2Rect.width / 2);
		button2Rect.y = (Screen.height / 2) - (button2Rect.height / 2);
		

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
                Application.LoadLevel(herstartlevel); // Load Totorial
			}
			
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
