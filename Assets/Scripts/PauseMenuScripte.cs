using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class PauseMenuScripte : MonoBehaviour {

    private GUISkin skin;

    public bool PauseActive = false;
    public RectTransform pauzeMenu;

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);  

    private Text kindTextHUD;
    private int maxKids;

    // Analytics
    private bool pauseStarted = false;
    private float pauseStartTime;
    private bool pauseContinue;

	// Use this for initialization
	void Start () 
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
        
        // Pauzemenu
        if (PauseActive == true)
        {
            if (!pauseStarted)
            {
                pauseStartTime = Time.realtimeSinceStartup;
                pauseStarted = true;
            }

            button1Rect.y = button1Rect.y + 65;
            button2Rect.y = button2Rect.y + 135;
            // Activeer Ingame menu
            pauzeMenu.gameObject.SetActive(true);

            // Zet game op stil
            Time.timeScale = 0;

            // Set the skin to use
            GUI.skin = skin;
            
            // Terug Button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button1Rect,
                "Terug"
                ))
            {
                pauseContinue = true;
                pauseStarted = false;
                StartGameAnalytics();

                PauseActive = false;
                Time.timeScale = 1;
                pauzeMenu.gameObject.SetActive(false);
            }
           
            // Naar Main menu button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button2Rect,
                "Menu"
                ))
            {
                pauseContinue = false;
                pauseStarted = false;
                StartGameAnalytics();

                PauseActive = false;
                Time.timeScale = 1;
                pauzeMenu.gameObject.SetActive(false);
                Application.LoadLevel("MainMenu");
            }
        }                
    }

    /**
     * Sends the selected player and level to analytics
     */
    void StartGameAnalytics()
    {
        float timeSpent = (Time.realtimeSinceStartup - pauseStartTime);
        GameControl.control.pauseDuration += timeSpent;

        UnityAnalytics.CustomEvent("pauseMenu", new Dictionary<string, object>
        {
            { "selectedLevel", Application.loadedLevelName },
            { "runningTime", Time.timeSinceLevelLoad },
            { "timeSpentInPauseMenu", timeSpent },
            { "continueAfterPauseMenu", pauseContinue },
        });
    }
}
