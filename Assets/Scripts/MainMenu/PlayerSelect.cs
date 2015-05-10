using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSelect : MonoBehaviour {

    public GameObject mainMenu;    

    private GUISkin skin;

    // Keyboard control
    string[] buttons = new string[2] { "Fynn", "Fiona"};
    private int selected;

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);

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

        button1Rect.y = button1Rect.y - 80;
        button1Rect.x = button1Rect.x;
        button2Rect.y = button2Rect.y - 30;
        button2Rect.x = button2Rect.x;       

        // Set the skin to use
        GUI.skin = skin;

        GUI.SetNextControlName(buttons[0]);
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button1Rect,
            "Fynn"
        ))
        {
            GameControl.control.selectPlayer = "Fynn"; // Zet speler op Fynn 
            _audioSource.Play();

            this.gameObject.SetActive(false);            
            mainMenu.GetComponent<MainMenu>()._mainMenuUit = false;
            selected = 0;
        }

        GUI.SetNextControlName(buttons[1]);
        // Prestaties Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button2Rect,
            "Fiona"
        ))
        {
            GameControl.control.selectPlayer = "Fiona"; // Zet speler op Fiona           
            _audioSource.Play();

            this.gameObject.SetActive(false);
            mainMenu.GetComponent<MainMenu>()._mainMenuUit = false;
            selected = 1;
        }   
        GUI.FocusControl(buttons[selected]);
    }
}
