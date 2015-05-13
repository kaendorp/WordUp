using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject speler;
    private GUISkin skin;

    // Keyboard control
    string[] buttons = new string[4] { "Tutorial", "Level 1", "Level 2", "Level 3" };
    private int selected;

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);
    private Rect button3Rect = new Rect(15, 15, 160, 30);
    private Rect button4Rect = new Rect(15, 15, 160, 30);
    private Rect button5Rect = new Rect(15, 15, 160, 30);

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

        button4Rect.x = (Screen.width / 2) - (button4Rect.width / 2);
        button4Rect.y = (Screen.height / 2) - (button4Rect.height / 2);

        button5Rect.x = (Screen.width / 2) - (button5Rect.width / 2);
        button5Rect.y = (Screen.height / 2) - (button5Rect.height / 2);

        button1Rect.y = button1Rect.y - 80;
        button2Rect.y = button2Rect.y - 40;
        button3Rect.y = button3Rect.y;
        button4Rect.y = button4Rect.y + 40;
        button5Rect.y = button5Rect.y + 80;

        // Set the skin to use
        GUI.skin = skin;

        GUI.SetNextControlName(buttons[0]);
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button1Rect,
            "Tutorial"
        ))
        {
            GameControl.control.loadLevel = "Intro"; // Zet laadlevel op Intro en dan naar Tutorial     
            _audioSource.Play();            
            selected = 0;

            // playerSelect
            this.gameObject.SetActive(false);
            speler.SetActive(true); // Select Speler            
        }

        if (GameControl.control.unlockedLevels[0] == true)
        {
            GUI.SetNextControlName(buttons[1]);
            // Prestaties Button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button2Rect,
                "Level  1"
            ))
            {
                GameControl.control.loadLevel = "Level1"; // Zet laadlevel op level 1            
                _audioSource.Play();                
                selected = 1;

                // playerSelect
                this.gameObject.SetActive(false);
                speler.SetActive(true); // Select Speler                
            }
        }
        if (GameControl.control.unlockedLevels[1] == true)
        {
            GUI.SetNextControlName(buttons[2]);
            // Opties Button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button3Rect,
                "Level  2"
            ))
            {
                GameControl.control.loadLevel = "Level2-1"; // Zet laadlevel op level 2            
                _audioSource.Play();                
                selected = 2;
                
                // playerSelect
                this.gameObject.SetActive(false);
                speler.SetActive(true); // Select Speler                
            }
        }
        if (GameControl.control.unlockedLevels[2] == true)
        {
            GUI.SetNextControlName(buttons[3]);
            // Exit Button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button4Rect,
                "Level  3"
            ))
            {
                GameControl.control.loadLevel = "Level3-1";  // Zet laadlevel op level 3            
                _audioSource.Play();                
                selected = 3;

                // playerSelect
                this.gameObject.SetActive(false);
                speler.SetActive(true); // Select Speler
            }
        }
        // Terug Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button5Rect,
            "< Terug"
        ))
        {
            _audioSource.Play();

            this.gameObject.SetActive(false);
            mainMenu.GetComponent<MainMenu>()._mainMenuUit = false;
        }
        GUI.FocusControl(buttons[selected]);
    }
}
