using UnityEngine;
using System.Collections;

public class BerichtenMenuController : MonoBehaviour
{
    public RectTransform berichtenMaker;

    public bool berichtMakerActive = false;
    private GUISkin skin;
    [TextArea(1, 2)]
    public string selectedText = "zz";
    [TextArea(1, 2)]
    public string setText = "";

    private RectTransform window;

    private string[] messageList = new string[7];

    private GameObject messagePrefab;

    private string[] wordOptions;

    public string selectedMessage;

    public int selected = 0;
    public int stage = 0;

    private Rect buttonRect = new Rect(15, 20, 260, 30);

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

    private string[] baseWord2 = new string[13] {
        back,
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

    private static string back = "< terug";

    private string[] category = new string[8]{
        back,
        "Wezens",
        "Objecten",
        "Techniek",
        "Locatie",
        "Oriëntatie",
        "Concepten",
        "Overpeinzingen"
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

    private string[] objecten = new string[12]{
	    back,
        "ladder",
	    "obstakel",
	    "letter",
	    "knop",
	    "blok",
	    "deur",
	    "lamp",
	    "sleutel",
	    "iets",
	    "iets moois",
	    "bericht",
    };

    private string[] techniek = new string[8]{
        back,
	    "tegenaanval",
	    "aanvallen van een afstand",
	    "één voor één uitschakelen",
	    "doorheen rennen",
	    "springen",
	    "hinderlaag",
	    "schild",
    };

    private string[] locatie = new string[6]{
        back,
	    "platform",
        "grot",
	    "gevaarlijke plek",
	    "verborgen plek",
	    "veilige plek",
    };

    private string[] oriëntatie = new string[9]{
        back,
        "voorwaards",
        "terug",
        "links",	
        "rechts",
        "omhoog",
        "omlaag",
        "boven",
        "beneden",
    };

    private string[] concepten = new string[10]{
        back,
        "kans",
        "hint",
        "geheim",
        "vreugde",
        "leven",
        "victorie",
        "zelfvertrouwen",
        "hoop",
        "kracht",
    };

    private string[] overpeinzingen = new string[14]{
	    back,
        "succes",
	    "goed werk",
	    "het lukte!",
	    "hier!",
	    "niet hier!",
	    "kijk goed",
	    "luister goed",
	    "denk goed",
	    "blijf doorgaan",
	    "geef niet op",
	    "alweer hier?",
	    "ben je er klaar voor?",
	    "fantastisch uitzicht!",
    };

    private string[] tussenvoegsel = new string[8]{
        back,
	    ", ",
	    " en dan ",
	    " maar ",
	    " daarom ",
	    " of ",
	    " dus ",
	    " trouwens ",
    };

    private string[] done = new string[3] 
    {
        back,
        "Annuleren",
        "Afronden",
    };

    // Use this for initialization
    void Start()
    {
        skin = Resources.Load("BerichtWoordSkin") as GUISkin;

        selected = 0;
        wordOptions = baseWord;
    }

    // Update is called once per frame
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
            Debug.Log("Escape!");
            wordOptions = done;
        }
    }

    public void SetMessage(string message, int stage)
    {
        if (message == "Afronden")
        {
            SendMessageToPrefab();
            Time.timeScale = 1;
            berichtMakerActive = false;
            berichtenMaker.gameObject.SetActive(false);
            return;
        }
        else if (message == "Annuleren")
        {
            Time.timeScale = 1;
            berichtMakerActive = false;
            berichtenMaker.gameObject.SetActive(false);
        }

        switch (stage)
        {
            case 0:
                messageList[0] = message;
                break;
            case 1:
                messageList[1] = messageList[0];
                break;
            case 2:
                if (message != null)
                    messageList[2] = messageList[1].Replace("***", message);
                else
                    messageList[2] = messageList[1];
                break;
            case 3:
                messageList[3] = messageList[2] + message;
                break;
            case 4:
                messageList[4] = messageList[3] + message;
                break;
            case 5:
                messageList[5] = messageList[4];
                break;
            case 6:
                if (message != null)
                    messageList[6] = messageList[5].Replace("***", message);
                else
                    messageList[6] = messageList[5];
                break;
            case 7:
                // Confirm message
                break;
        }
        if (stage != 7)
            selectedText = messageList[stage];
    }

    void OnGUI()
    {
        buttonRect.width = window.rect.width;
        buttonRect.x = (window.rect.width / 2) - (buttonRect.width / 2);
        //buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2);

        if (berichtMakerActive == true)
        {
            Time.timeScale = 0;
            berichtenMaker.gameObject.SetActive(true);

            //OverwriteSpacebar();

            if (wordOptions != done)
                ChangeMenuItems();
            buttonRect.width = this.gameObject.GetComponent<RectTransform>().rect.width;
            buttonRect.x = (Screen.width / 2) - (buttonRect.width / 2);
            //buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2);


            // Set the skin to use
            GUI.skin = skin;
            float height = buttonRect.y + 6f;

            GUI.SetNextControlName("Bericht");
            GUI.Label(
                buttonRect,
                new GUIContent(selectedText)
                    );

            for (int i = 0; i < wordOptions.Length; i++)
            {
                Rect buttonRectTemp = buttonRect;
                buttonRectTemp.y += height * i;

                GUI.SetNextControlName(wordOptions[i]);
                //buttonRect.y += (height*i);
                if (GUI.Button(buttonRectTemp, wordOptions[i]))
                {
                    selectedMessage = wordOptions[i];
                    selected = 0;
                    SendMessage();
                }
            }
            GUI.FocusControl(wordOptions[selected]);
        }
    }

    public void GetMessagePrefab(GameObject messageObject)
    {
        messagePrefab = messageObject;
    }

    public void SendMessageToPrefab()
    {
        // TODO
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

    public void SendMessage()
    {
        if (selectedMessage == back)
        {
            if (wordOptions != done)
            {
                stage--;
            }
            else
            {
                wordOptions = baseWord;
            }
            SetMessage(null, stage);
        }
        else
        {
            SetMessage(selectedMessage, stage);
            stage++;
        }
    }

    public void ChangeMenuItems()
    {
        switch (stage)
        {
            // wordoptions
            case 0:
                wordOptions = baseWord;
                break;
            // category
            case 1:
                wordOptions = category;
                break;
            // sub-category
            case 2:
                ChangeCategory(selectedMessage);
                break;
            // tussenvoegsel
            case 3:
                wordOptions = tussenvoegsel;
                break;
            case 4:
                wordOptions = baseWord2;
                break;
            case 5:
                wordOptions = category;
                break;
            case 6:
                ChangeCategory(selectedMessage);
                break;
        }
    }

    public void ChangeCategory(string message)
    {
        switch (message)
        {
            case ("Wezens"):
                wordOptions = wezens;
                break;
            case ("Objecten"):
                wordOptions = objecten;
                break;
            case ("Techniek"):
                wordOptions = techniek;
                break;
            case ("Locatie"):
                wordOptions = locatie;
                break;
            case ("Oriëntatie"):
                wordOptions = oriëntatie;
                break;
            case ("Concepten"):
                wordOptions = concepten;
                break;
            case ("Overpeinzingen"):
                wordOptions = overpeinzingen;
                break;
        }
    }

    private void OverwriteSpacebar()
    {
        if (Event.current.type == EventType.keyDown)
        {
            if (Event.current.keyCode == KeyCode.Space)
            {
                Debug.Log("Space was pressed, default unity hotkey is overridden.");
                Event.current.Use();    // if you don't use the event, the default action will still take place.
            }
        }
    }
}
