using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class PlayerSelect : MonoBehaviour {

    public GameObject mainMenu;    

    private GUISkin skin;    

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);
    private Rect button3Rect = new Rect(15, 15, 160, 30);

    public AudioSource _audioSource;

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

        button3Rect.x = (Screen.width / 2) - (button3Rect.width / 2);
        button3Rect.y = (Screen.height / 2) - (button3Rect.height / 2); 

        button1Rect.y = button1Rect.y - 80;
        button1Rect.x = button1Rect.x;
        button2Rect.y = button2Rect.y - 30;
        button2Rect.x = button2Rect.x;
        button3Rect.y = button3Rect.y + 20;
        button3Rect.x = button3Rect.x; 
      

        // Set the skin to use
        GUI.skin = skin;
       
        // Fynn Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button1Rect,
            "Fynn"
        ))
        {
            GameControl.control.selectPlayer = "Fynn"; // Zet speler op Fynn 
            _audioSource.Play();

            // Send data to Analytics
            StartGameAnalytics();

            // LoadLevel
            GameControl.control.isMainMenu = false;
            Application.LoadLevel(GameControl.control.loadLevel); // Load Intro            
        }
        
        // Fiona Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button2Rect,
            "Fiona"
        ))
        {
            GameControl.control.selectPlayer = "Fiona"; // Zet speler op Fiona           
            _audioSource.Play();

            // Send data to Analytics
            StartGameAnalytics();

            // LoadLevel
            GameControl.control.isMainMenu = false;
            Application.LoadLevel(GameControl.control.loadLevel); // Load Intro     
        }

        // Terug Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button3Rect,
            "< Terug"
        ))
        {
            _audioSource.Play();

            this.gameObject.SetActive(false);
            mainMenu.GetComponent<MainMenu>()._mainMenuUit = false;            
        }  
    }

    /**
     * Sends the selected player and level to analytics
     */
    void StartGameAnalytics()
    {
        UnityAnalytics.CustomEvent("startFromMainMenu", new Dictionary<string, object>
        {
            { "selectedLevel", GameControl.control.loadLevel},
            { "selectedPlayer", GameControl.control.selectPlayer}
        });
    }
}
