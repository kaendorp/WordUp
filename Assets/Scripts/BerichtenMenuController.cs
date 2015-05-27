using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;
using UnitySampleAssets.CrossPlatformInput;

public class BerichtenMenuController : MonoBehaviour
{
    public RectTransform berichtenMaker;                    // BerichtenMaker background (Located in HUD)
    public Text displayText;
    public GameObject[] berichtenPrefabs;                   // List messagepoints in a level
    private BerichtGetSet berichtGetSet;                    // Used to get access to the script

    public bool berichtMakerActive = false;                 // Bool to trigger the menu
    private GUISkin skin;                                   // GUI Skin
    private string selectedText = "";                       // Selected text, set after the menu selection

    private string[] messageList = new string[8];           // Text stored in between selections, back and forth

    private GameObject messagePrefab;                       // Used to set the message to this prefab, instaid of from the server

    private string selectedMessage;                         // Selected part of the message after menu selection

    private int stage = 0;                                  // Current menu stage

    private Rect buttonRect = new Rect(15, 40, 195, 15);    // Default button size

    private string[] wordOptions;                           // Menuitems
    private float arrayLength;                              // Amount of half the menu items

    private static string back = "< terug";                 // Static string, subject to change and used by many stringArrays
    private static string stop = "Annuleren";
    private static string done = "Afronden";

    private string categoryLabelText;

    // ANALYTICS
    private float analyticsTimeStart = 0f;
    private bool analyticsTimeStarted = false;

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

    private string[] category = new string[7]{
        "Wezens",
        "Objecten",
        "Techniek",
        "Locatie",
        "Oriëntatie",
        "Concepten",
        "Overpeinzingen"
    };

    private string[] wezens = new string[12]{
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
        "ladder",
	    "obstakel",
	    "letter",
	    "knop",
	    "blok",
	    "deur",
	    "lamp",
	    "sleutel",
        "klok",
	    "iets",
	    "iets moois",
	    "bericht",
    };

    private string[] techniek = new string[7]{
	    "tegenaanval",
	    "aanvallen van een afstand",
	    "één voor één uitschakelen",
	    "doorheen rennen",
	    "springen",
	    "hinderlaag",
	    "schild",
    };

    private string[] locatie = new string[5]{
	    "platform",
        "grot",
	    "gevaarlijke plek",
	    "verborgen plek",
	    "veilige plek",
    };

    private string[] oriëntatie = new string[8]{
        "voorwaards",
        "terug",
        "links",	
        "rechts",
        "omhoog",
        "omlaag",
        "boven",
        "beneden",
    };

    private string[] concepten = new string[9]{
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

    private string[] overpeinzingen = new string[13]{
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

    private string[] tussenvoegsel = new string[7]{
	    ",\n",
	    " en dan\n",
	    " maar\n",
	    " daarom\n",
	    " of\n",
	    " dus\n",
	    " trouwens\n",
    };

    // Use this for initialization
    void Start()
    {
        berichtGetSet = this.gameObject.GetComponent<BerichtGetSet>();

        skin = Resources.Load("BerichtWoordSkin") as GUISkin;

        PopulateBerichten();            // Starts the population of messages to prefabs
        wordOptions = baseWord;         // Default menu list
    }

    /**
     * Controls to navigate the menus.
     *
     * Escape triggers the 'done' menu to finalise or cancel setting a new message.
     */
    void Update()
    {
        if (berichtMakerActive && CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            ExitMessageMenu();
        }
    }

    /**
     * Called by BerichtController.OnTriggerStay2D()
     *
     * Used to pass itself to this controller.
     */
    public void GetMessagePrefab(GameObject messageObject)
    {
        messagePrefab = messageObject;
    }

    /**
     * Triggerd by Start()
     *
     * Will attempt to set a message for every messageprefab in the level by a one
     * from the database. This by using the PopulateBericht coroutine for each
     * messageprefab.
     */
    public void PopulateBerichten()
    {
        foreach (GameObject berichtenPrefab in berichtenPrefabs)
        {
            StartCoroutine(PopulateBericht(berichtenPrefab));
        }
    }

    /**
     * Contact the database to get a message.
     *
     * Each prefab should have an unique key, with 0 being the default value. 
     * Therefore it should not be used, maybe usefull for special Dev Messages that 
     * you don't want to have overwritten.
     *
     * It could happen that the database returns null, in that case do nothing and 
     * use the default message given to the gameObject in the scene.
     */
    IEnumerator PopulateBericht(GameObject berichtprefb)
    {
        int prefabKey = berichtprefb.GetComponent<BerichtController>().messageKey;
        if (prefabKey > 0)
        {
            yield return StartCoroutine(berichtGetSet.RetrieveMessagesFromServer(prefabKey, (getResult) =>
            {
                if (!string.IsNullOrEmpty(getResult))
                    berichtprefb.GetComponent<BerichtController>().message = getResult;
            }));
        }
    }

    /**
     * Draws the menu's, label and buttons
     */
    void OnGUI()
    {
        if (berichtMakerActive == true)
        {
            if (!analyticsTimeStarted)
            {
                analyticsTimeStart = Time.realtimeSinceStartup;
                analyticsTimeStarted = true;
            }

            //buttonRect.width = this.gameObject.GetComponent<RectTransform>().rect.width;
            buttonRect.x = (Screen.width / 2) - (buttonRect.width / 2);
            buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2) - 50f;
            Time.timeScale = 0;                             // Pause the game and activate the menu
            berichtenMaker.gameObject.SetActive(true);      // Activate the menu

            ChangeMenuItems();                          // Overwrite the menuItems 'wordOptions' with selected menu

            GUI.skin = skin;                                // Set the skin to use

            displayText.GetComponent<Text>().text = selectedText;

            float heightPadding = 20f; // height between buttons

            Rect topLabel = buttonRect;
            GUI.SetNextControlName("TopLabel");
            topLabel.y -= heightPadding+10f;
            topLabel.width = Screen.width;
            topLabel.x = (Screen.width / 2) - (topLabel.width / 2);
            GUI.Label(topLabel, categoryLabelText);

            /**
             * List buttons buttons
             */
            if (stage != 7)
            {
                int j = 0;
                for (int i = 0; i < wordOptions.Length; i++)    // For every item in menu, make a button
                {
                    Rect buttonRectTemp = buttonRect;

                    // Check to see if the list is even or odd, if odd, the left list should be one more
                    if (wordOptions.Length % 2 == 0)         // even amount
                        arrayLength = wordOptions.Length / 2;
                    else                                     // odd amount
                        arrayLength = Mathf.RoundToInt(wordOptions.Length / 2) + 1;

                    if (i < arrayLength)
                    {
                        buttonRectTemp.x = buttonRect.x - (buttonRect.width / 2);
                        buttonRectTemp.y += heightPadding * i;
                    }
                    else
                    {
                        buttonRectTemp.x = buttonRect.x + (buttonRect.width / 2);
                        buttonRectTemp.y = buttonRect.y;
                        buttonRectTemp.y += heightPadding * j;
                        j++;
                    }

                    GUI.SetNextControlName(wordOptions[i]);
                    if (GUI.Button(buttonRectTemp, wordOptions[i].Replace("\n", "")))
                    {
                        selectedMessage = wordOptions[i];       // Set the selected word after button is pressed
                        SendMessage();                          // Send the selected word for processing
                    }
                }
            }

            /**
             * User is done, ask the user to finish the message.
             */
            else
            {
                categoryLabelText = "";

                Rect afrondenLabelRect = buttonRect;
                GUI.SetNextControlName("AfrondenLabel");
                GUI.Label(afrondenLabelRect, "Bericht plaatsen?");

                Rect afrondenButtonRect = buttonRect;
                afrondenButtonRect.y += heightPadding;
                GUI.SetNextControlName("Afronden?");
                if (GUI.Button(afrondenButtonRect, "Afronden"))
                {
                    selectedMessage = done;
                    SendMessage();
                }
            }

            /**
             * Show 'finish message?' button when on the 'add conjuction' stage.
             */
            if (stage == 3)
            {
                Rect afrondenLabelRect = buttonRect;
                afrondenLabelRect.y += heightPadding * (arrayLength + 1);
                GUI.SetNextControlName("AfrondenLabel");
                GUI.Label(afrondenLabelRect, "Bericht plaatsen?");

                Rect afrondenButtonRect = buttonRect;
                afrondenButtonRect.y += heightPadding * (arrayLength + 2);
                GUI.SetNextControlName("Afronden?");
                if (GUI.Button(afrondenButtonRect, "Afronden"))
                {
                    selectedMessage = done;
                    SendMessage();
                }
            }

            /**
             * Buttons at the bottom of the menu:
             * Stop, Back, Done
             */
            // Stop
            Rect cancelButtonRect = buttonRect;
            cancelButtonRect.width = buttonRect.width / 2;
            cancelButtonRect.x = (Screen.width / 2) - (cancelButtonRect.width/2) - 240;
            cancelButtonRect.y = (Screen.height / 2) + 195;

            GUI.SetNextControlName(stop);
            if (GUI.Button(cancelButtonRect, stop))
            {
                selectedMessage = stop;
                SendMessage();
            }

            // Back, only after you can actually go back, after the first stage
            Rect backButtonRect = buttonRect;
            backButtonRect.width = buttonRect.width / 2;
            backButtonRect.x = (Screen.width / 2) - (backButtonRect.width / 2);
            backButtonRect.y = (Screen.height / 2) + 195;

            GUI.SetNextControlName(back);
            if (stage > 0)
            {
                if (GUI.Button(backButtonRect, back))
                {
                    selectedMessage = back;
                    SendMessage();
                }
            }
            else
            {
                GUI.Label(backButtonRect, back);
            }

            // Done, show label if you can't finish the message, otherwise button
            Rect doneButtonRect = buttonRect;
            doneButtonRect.width = buttonRect.width / 2;
            doneButtonRect.x = (Screen.width / 2) - (cancelButtonRect.width / 2) + 240;
            doneButtonRect.y = (Screen.height / 2) + 195;
            
            GUI.SetNextControlName(done);
            if (stage == 3 || stage == 7)
            {
                if (GUI.Button(doneButtonRect, done))
                {
                    selectedMessage = done;
                    SendMessage();
                }
            }
            else
            {
                GUI.Label(doneButtonRect, done);
            }
        }
    }

    /**
     * Constructing messages goes in stages, 
     * in stage 0 you select the kind of base sentaince to use
     * in stage 1 you select a category
     * in stage 2 you use the item in a category to place in the sentaince selected in stage 1
     * in stage 3 the player is asked to continue, stop or finish
     * in stage 4 is your connection betweet the first part of the sentaince and the second
     * stage 5, 6 and 7 are repeats of 0,1 and 2.
     * in stage 8 the message is considered complete
     *
     * Called in OnGUI(), after button press
     */
    public void ChangeMenuItems()
    {
        switch (stage)
        {
            // wordoptions
            case 0:
                wordOptions = baseWord;
                categoryLabelText = "Kies een bericht om toe te voegen:";
                break;
            // category
            case 1:
                wordOptions = category;
                categoryLabelText = "Kies een woord uit de volgende categorieën:";
                break;
            // sub-category
            case 2:
                ChangeCategory(selectedMessage);
                break;
            // tussenvoegsel
            case 3:
                wordOptions = tussenvoegsel;
                categoryLabelText = "Voegwoord:";
                break;
            case 4:
                wordOptions = baseWord;
                categoryLabelText = "Kies een bericht om toe te voegen:";
                break;
            case 5:
                wordOptions = category;
                categoryLabelText = "Kies een woord uit de volgende categorieën:";
                break;
            case 6:
                ChangeCategory(selectedMessage);
                break;
        }
    }

    /**
     * Called in ChangeMenuItems()
     * 
     * Sets the menu items to the category selected in stage 2 and 5.
     */
    public void ChangeCategory(string messagePassed)
    {
        switch (messagePassed)
        {
            case ("Wezens"):
                wordOptions = wezens;
                categoryLabelText = "Wezens:";
                break;
            case ("Objecten"):
                wordOptions = objecten;
                categoryLabelText = "Objecten:";
                break;
            case ("Techniek"):
                wordOptions = techniek;
                categoryLabelText = "Techniek:";
                break;
            case ("Locatie"):
                wordOptions = locatie;
                categoryLabelText = "Locatie:";
                break;
            case ("Oriëntatie"):
                wordOptions = oriëntatie;
                categoryLabelText = "Oriëntatie:";
                break;
            case ("Concepten"):
                wordOptions = concepten;
                categoryLabelText = "Concepten:";
                break;
            case ("Overpeinzingen"):
                wordOptions = overpeinzingen;
                categoryLabelText = "Overpeinzingen:";
                break;
        }
    }

    /**
     * Manages how the menu switches stages 
     * and calls SetMessage() with the intended stage.
     */
    public void SendMessage()
    {
        if (selectedMessage != back)
        {
            SetMessage(selectedMessage, stage);
            if (stage <= 6 && selectedMessage != null) // selectedMessage will be null if the users stops
                stage++;
        }
        else
        {
            if (stage >= 1 && stage != 3)
                stage--;
            else if (stage == 3)
                stage = 1;
            
            SetMessage(null, stage);
        }
    }

    /**
     * Processes the selected message.
     * Called in SendMessage().
     *
     * First it checks if the player wants to stop, either because he is done
     * or wants to cancel.
     *
     * If the player wants to continue it will process the message.
     * Depending on what state it is, it will add to the string or replace
     * placeholder. It writes it in an array so that it's easier to go back
     * through the stages.
     *
     * At the end it sets the processed message in the selectedText string.
     * This string is used to send to the label in the GUI, the prefab and the database.
     */
    public void SetMessage(string messagePassed, int stagePassed)
    {
        if (messagePassed == done)
        {
            SendMessageToPrefab();
            ExitMessageMenu();
            return;
        }
        else if (messagePassed == stop)
        {
            ExitMessageMenu();
            return;
        }

        switch (stagePassed)
        {
            case 0:
                messageList[0] = messagePassed;
                break;
            case 1:
                messageList[1] = messageList[0];
                break;
            case 2:
                if (messagePassed != null)
                    messageList[2] = messageList[1].Replace("***", messagePassed);
                else
                    messageList[2] = messageList[1];
                break;
            case 3:
                messageList[3] = messageList[2] + messagePassed;
                break;
            case 4:
                messageList[4] = messageList[3] + messagePassed;
                break;
            case 5:
                messageList[5] = messageList[4];
                break;
            case 6:
                if (messagePassed != null)
                    messageList[6] = messageList[5].Replace("***", messagePassed);
                else
                    messageList[6] = messageList[5];
                break;

        }

        if (stagePassed != 7) // user is done with the message
            selectedText = messageList[stagePassed];
    }

    public void ExitMessageMenu()
    {
        StartGameAnalytics();
        analyticsTimeStarted = false;

        wordOptions = baseWord;
        messageList = new string[8];
        stage = 0;
        selectedMessage = null;
        Time.timeScale = 1;
        berichtMakerActive = false;
        berichtenMaker.gameObject.SetActive(false);
    }

    /**
     * Triggered by SetMessage() after the player selected "Afronden".
     *
     * Sends the finished message to the prefab for the player to enjoy,
     * then sends it to the database.
     */
    public void SendMessageToPrefab()
    {
        messagePrefab.GetComponent<BerichtController>().message = selectedText;
        messagePrefab.GetComponent<BerichtController>().isRewritten = true;
        int key = messagePrefab.GetComponent<BerichtController>().messageKey;

        // TODO: Get unique userkey from playerData
        string user = "Guest";

        StartCoroutine(berichtGetSet.submitMessage(key, user, selectedText));
    }

    /**
     * Sends the selected player and level to analytics
     */
    void StartGameAnalytics()
    {
        float timeSpent = (Time.realtimeSinceStartup - analyticsTimeStart);
        int messageKey = messagePrefab.GetComponent<BerichtController>().messageKey;
        bool isWritten = messagePrefab.GetComponent<BerichtController>().isRewritten;

        AnalyticsResult results = UnityAnalytics.CustomEvent("berichtMaker", new Dictionary<string, object>
        {
            { "selectedLevel", Application.loadedLevelName },
            { "berichtID", messageKey },
            { "timeSpentInBerichtMenu", timeSpent},
            { "messagePlaced", isWritten },
        });

        if (results != AnalyticsResult.Ok)
            Debug.LogError("Analytics berichtMaker: " + results.ToString());
        else
            Debug.Log("Analytics berichtMaker: Done");
    }
}
