using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuScripte : MonoBehaviour {

    private GUISkin skin;

    public bool PauseActive = false;
    public RectTransform pauzeMenu;

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);

    // Keyboard control
    string[] buttons = new string[2] {"Terug", "Menu"};
    private int selected = 0;

    private Text kindTextHUD;
    private int maxKids;

    void KindPlus()
    {
        //kindTextHUD.text = GameObject.Find("kind_teller_tekst").GetComponent<Text>().text;
        Debug.Log("PLus");
    }

	// Use this for initialization
	void Start () 
    {
        // Load a skin for the buttons
        skin = Resources.Load("ButtonSkin") as GUISkin;

        selected = 0;

        
        //kindTextHUD.text = "0" + "  " + maxKids;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selected = menuSelection(buttons, selected, "up");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selected = menuSelection(buttons, selected, "down");
        }
	}

    int menuSelection(string[] buttonsArray, int selectedItem, string direction)
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

        if (direction == "down")
        {

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

        // Pauzemenu
        if (PauseActive == true)
        {
            button1Rect.y = button1Rect.y + 65;
            button2Rect.y = button2Rect.y + 135;
            // Activeer Ingame menu
            pauzeMenu.gameObject.SetActive(true);

            // Zet game op stil
            Time.timeScale = 0;

            // Set the skin to use
            GUI.skin = skin;

            GUI.SetNextControlName(buttons[0]);
            // Terug Button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button1Rect,
                "Terug"
                ))
            {
                PauseActive = false;
                Time.timeScale = 1;
                pauzeMenu.gameObject.SetActive(false);
            }

            GUI.SetNextControlName(buttons[1]);
            // Naar Main menu button
            if (GUI.Button(
                // Center in X, 2/3 of the height in Y
                button2Rect,
                "Menu"
                ))
            {
                PauseActive = false;
                Time.timeScale = 1;
                pauzeMenu.gameObject.SetActive(false);
                Application.LoadLevel("MainMenu");
            }
        }                
    }
}
