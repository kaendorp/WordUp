using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    public GameObject level;
    public GameObject speler;
    
    private GUISkin skin;	

	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect button2Rect = new Rect(15,15,160,30);
	private Rect button3Rect = new Rect(15,15,160,30);
	private Rect button4Rect = new Rect(15,15,160,30);

    public AudioSource _audioSource;

	void Start()
	{
        level.SetActive(false);
        speler.SetActive(false);

		// Load a skin for the buttons
		skin = Resources.Load("ButtonSkin") as GUISkin;	
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

		button1Rect.y = button1Rect.y - 130;
        button2Rect.y = button2Rect.y - 80;
		button3Rect.y = button3Rect.y - 30;
		button4Rect.y = button4Rect.y + 20;       
		
		// Set the skin to use
		GUI.skin = skin;
		
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
        
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button2Rect,
            "Speler"
        ))
        {
            _audioSource.enabled = true;
            speler.SetActive(true); // Select Speler
            this.gameObject.SetActive(false);
        }
        
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

		// Prestaties Button
		if (GUI.Button (
			// Center in X, 2/3 of the height in Y
			button4Rect,
			"Prestaties"
		)) 
		{
             // Load Prestaties Scene
		}		
	}

    void OnPointerEnter()
    {
        
    }
}
