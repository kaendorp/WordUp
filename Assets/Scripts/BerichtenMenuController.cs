using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    private string[] wordOptions;                            // Menuitems

    private static string back = "< terug";                 // Static string, subject to change and used by many stringArrays

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
	    ",\n",
	    " en dan\n",
	    " maar\n",
	    " daarom\n",
	    " of\n",
	    " dus\n",
	    " trouwens\n",
    };

    private string[] done = new string[4] 
    {
        back,
        "Afronden",
        "Doorgaan",
        "Annuleren",
    };

    private string[] done2 = new string[3] 
    {
        back,
        "Afronden",
        "Annuleren",
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
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    wordOptions = done;
        //}
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
            //buttonRect.width = this.gameObject.GetComponent<RectTransform>().rect.width;
            buttonRect.x = (Screen.width / 2) - (buttonRect.width / 2);
            buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2) - 80f;
            Time.timeScale = 0;                             // Pause the game and activate the menu
            berichtenMaker.gameObject.SetActive(true);      // Activate the menu

            ChangeMenuItems();                          // Overwrite the menuItems 'wordOptions' with selected menu

            GUI.skin = skin;                                // Set the skin to use

            displayText.GetComponent<Text>().text = selectedText;

            for (int i = 0; i < wordOptions.Length; i++)    // For every item in menu, make a button
            {
                Rect buttonRectTemp = buttonRect;
                float height = 20f;
                buttonRectTemp.y += height * i;
                
                GUI.SetNextControlName(wordOptions[i]);
                if (GUI.Button(buttonRectTemp, wordOptions[i].Replace("\n","")))
                {
                    selectedMessage = wordOptions[i];       // Set the selected word after button is pressed
                    SendMessage();                          // Send the selected word for processing
                }
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
                break;
            // category
            case 1:
                wordOptions = category;
                break;
            // sub-category
            case 2:
                ChangeCategory(selectedMessage);
                break;
            case 3:
                wordOptions = done;
                break;
            // tussenvoegsel
            case 4:
                wordOptions = tussenvoegsel;
                break;
            case 5:
                wordOptions = baseWord2;
                break;
            case 6:
                wordOptions = category;
                break;
            case 7:
                ChangeCategory(selectedMessage);
                break;
            case 8:
                wordOptions = done2;
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

    /**
     * Manages how the menu switches stages 
     * and calls SetMessage() with the intended stage.
     */
    public void SendMessage()
    {
        if (selectedMessage != back)
        {
            SetMessage(selectedMessage, stage);
            if (stage < 8)
                stage++;
        }
        else
        {
            if (stage == 8 || stage == 3)
            {
                stage -= 2;
            }
            else if (stage >= 1)
            {
                stage--;
            }
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
        if (messagePassed == "Afronden")
        {
            SendMessageToPrefab();
            stage = 0;
            Time.timeScale = 1;
            berichtMakerActive = false;
            berichtenMaker.gameObject.SetActive(false);
            return;
        }
        else if (messagePassed == "Annuleren")
        {
            messageList = new string[8];
            stage = 0;
            Time.timeScale = 1;
            berichtMakerActive = false;
            berichtenMaker.gameObject.SetActive(false);
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
                    messageList[3] = messageList[2];
                break;
            case 4:
                messageList[4] = messageList[3] + messagePassed;
                break;
            case 5:
                messageList[5] = messageList[4] + messagePassed;
                break;
            case 6:
                messageList[6] = messageList[5];
                break;
            case 7:
                if (messagePassed != null)
                    messageList[7] = messageList[6].Replace("***", messagePassed);
                else
                    messageList[7] = messageList[6];
                break;
            case 8:
                messageList[8] = messageList[7];
                break;

        }

        if (stagePassed != 9) // user is done with the message
            selectedText = messageList[stagePassed];
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

        int key = messagePrefab.GetComponent<BerichtController>().messageKey;

        // TODO: Get unique userkey from playerData
        string user = "Guest";

        StartCoroutine(berichtGetSet.submitMessage(key, user, selectedText));
    }

}
