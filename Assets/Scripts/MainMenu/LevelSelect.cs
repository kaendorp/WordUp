using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public GameObject mainMenu;
    public Text levelSelect;

    private GUISkin skin;

    // Keyboard control
    string[] buttons = new string[4] { "Tutorial", "Level 1", "Level 2", "Level 3" };
    private int selected = 0;

    private Rect button1Rect = new Rect(15, 15, 160, 30);
    private Rect button2Rect = new Rect(15, 15, 160, 30);
    private Rect button3Rect = new Rect(15, 15, 160, 30);
    private Rect button4Rect = new Rect(15, 15, 160, 30);

    void Start()
    {
        // Load a skin for the buttons
        skin = Resources.Load("ButtonSkin") as GUISkin;

        selected = 0;
    }

    void Update()
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

        button3Rect.x = (Screen.width / 2) - (button3Rect.width / 2);
        button3Rect.y = (Screen.height / 2) - (button3Rect.height / 2);

        button4Rect.x = (Screen.width / 2) - (button4Rect.width / 2);
        button4Rect.y = (Screen.height / 2) - (button4Rect.height / 2);

        button1Rect.y = button1Rect.y - 80;
        button2Rect.y = button2Rect.y - 40;
        button3Rect.y = button3Rect.y;
        button4Rect.y = button4Rect.y + 40;

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
            GameControl.control.loadLevel = "Tutorial"; // Zet laadlevel op Tutorial
            levelSelect.text = ": Tutorial";

            this.gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        GUI.SetNextControlName(buttons[1]);
        // Prestaties Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button2Rect,
            "Level  1"
        ))
        {
            GameControl.control.loadLevel = "Level1"; // Zet laadlevel op level 1
            levelSelect.text = ": Level 1";

            this.gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        GUI.SetNextControlName(buttons[2]);
        // Opties Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button3Rect,
            "Level  2"
        ))
        {
            GameControl.control.loadLevel = "Level2"; // Zet laadlevel op level 2
            levelSelect.text = ": Level 2";

            this.gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        GUI.SetNextControlName(buttons[3]);
        // Exit Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button4Rect,
            "Level  3"
        ))
        {
            GameControl.control.loadLevel = "Level3";  // Zet laadlevel op level 3
            levelSelect.text = ": Level 3";

            this.gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        GUI.FocusControl(buttons[selected]);
    }
}
