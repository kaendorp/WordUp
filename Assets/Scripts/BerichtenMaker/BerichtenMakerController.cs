using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BerichtenMakerController : MonoBehaviour {

    public BerichtenMenuController berichtenMenuController;

    //public GameObject level;
    //public GameObject speler;
    
    private GUISkin skin;
    // Keyboard control

    private string[] wordOptions;

    private string[] baseWord = new string[12] {
        "***",
        "***?",
        "***!",
        "***...",
        "*** verderop",
        "*** vereist",
        "Pas op voor ***",
        "Tijd voor ***",
        "Probeer ***",
        "Denk terug aan ***",
        "Hoera voor ***!",
        "En allemaal dankzij ***"
    };
    
    private static string back = "< terug naar menu";

    private string[] category = new string[2]{
        back,
        "Wezens",
    };

    private string[] wezens = new string[13]{
        back,
	    "vijand",
	    "vijanden",
	    "duo",
	    "trio",
	    "stationaire vijand",
	    "lopende vijand",
	    "vliegende vijand",
	    "hangende vijand",
	    "De Stilte",
	    "vriend",
	    "vrienden",
	    "vogel",
    };

    public int selected = 0;
    public int stage = 0;

    private RectTransform window;
    private Rect buttonRect = new Rect(15, 20, 260, 30);


    void Start()
    {
        //level.SetActive(false);
        //speler.SetActive(false);

        // Load a skin for the buttons
        skin = Resources.Load("BerichtWoordSkin") as GUISkin;

        selected = 0;
        wordOptions = baseWord;
    }

    void Update()
    {
        window = this.gameObject.GetComponent<RectTransform>();
        if (Input.GetKeyDown(KeyCode.W))
        {
            selected = menuSelection(wordOptions, selected, "up");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selected = menuSelection(wordOptions, selected, "down");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selected = 0;
            wordOptions = baseWord;
            stage = 0;
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
        buttonRect.width = window.rect.width;
        buttonRect.x = (Screen.width / 2) - (buttonRect.width / 2);
        //buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2);

        // Set the skin to use
        GUI.skin = skin;
        float height = buttonRect.y + 6f;

        for (int i = 0; i < wordOptions.Length; i++)
        {
            Rect buttonRectTemp = buttonRect;
            buttonRectTemp.y += height * i;

            GUI.SetNextControlName(wordOptions[i]);
            //buttonRect.y += (height*i);
            if (GUI.Button(
                buttonRectTemp,
                wordOptions[i]
                    ))
            {
                SendMessage(wordOptions[i]);
                selected = 0;
            }
        }
        GUI.FocusControl(wordOptions[selected]);        
    }

    public void SendMessage(string message)
    {
        // wordoptions
        if (stage == 0)
        {
            berichtenMenuController.SetMessage(message, stage);
            wordOptions = category;
            stage = 1;
            
        }
        // category
        else if (stage == 1)
        {
            if (message == back)
            {
                stage = 0;
                wordOptions = baseWord;
            }
            else
            {
                stage = 2;
                ChangeCategory(message);
            }
        }
        else if (stage == 2)
        {
            berichtenMenuController.SetMessage(message, stage);
        }
    }

    public void ChangeCategory(string message) {
        switch (message)
        {
            case ("Wezens"):
                wordOptions = wezens;
                break;
        }
    }
}
