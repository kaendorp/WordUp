using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementView : MonoBehaviour {

    public GameObject mainMenu;

    public GameObject PopUp;
    public Image image;
    public Text achievement;
    public Text unlock;
    public Text omschrijving;

    //Test
    public Image button;   

    private GUISkin skin;    

    private Rect button1Rect = new Rect(15, 15, 160, 30);    

    public AudioSource _audioSource;

    // ButtonImages
    public Image Kindvriendelijk;
    public Image Lef;
    public Image StilteVoorDeStorm;

    public Image DeHoogteIn;
    public Image Icarus;
    public Image Kindervriend;
    public Image Luid;
    public Image Stilteverstoorder;

    public Image DroogOver;
    public Image IJsbreker;
    public Image IJsvrij;
    public Image RedderInNood;
    public Image Warmte;
    public Image StilteOntregelaar;

    public Image Intellectueel;
    public Image Held;
    public Image Familie;
    public Image StilteVerbreker;

    public Image StartingOut;
    public Image Diplomaat;
    public Image GulleGever;
    public Image LevendeLegende;
    public Image Onaantasbaar;
    public Image SmoothTalker;
    public Image Veteraan;
    public Image WordUp;

    GameControl control;

    void Start()
    {
        // Load a skin for the buttons
        skin = Resources.Load("ButtonSkin") as GUISkin;

        control = GameControl.control;
    }

    void Update()
    {
        AchievementTrueOrFalse();
    }

    void AchievementTrueOrFalse()
    { 
        // Zet level unlock op true || 5 Achievements
        if (control.unlockedLevels[0] == true)
        {
            StartingOut.color = new Color32(255, 255, 255, 255);
        }
        if (control.unlockedLevels[1] == true)
        {
            DeHoogteIn.color = new Color32(255, 255, 255, 255);
        }
        if (control.unlockedLevels[2] == true)
        {
            IJsbreker.color = new Color32(255, 255, 255, 255);
        }
        if (control.unlockedLevels[3] == true)
        {
            Intellectueel.color = new Color32(255, 255, 255, 255);
        }
        if (System.Array.TrueForAll(control.unlockedLevels, item => item) == true)
        {
            WordUp.color = new Color32(255, 255, 255, 255);
        }

        // Zet Stilte verslagen op true || 4 achievements
        if (control.verslaStilte[0] == true)
        {
            StilteVoorDeStorm.color = new Color32(255, 255, 255, 255);
        }
        if (control.verslaStilte[1] == true)
        {
            Stilteverstoorder.color = new Color32(255, 255, 255, 255);
        }
        if (control.verslaStilte[2] == true)
        {
            StilteOntregelaar.color = new Color32(255, 255, 255, 255);
        }
        if (control.verslaStilte[3] == true)
        {
            StilteVerbreker.color = new Color32(255, 255, 255, 255);
        }

        // Zet Wordgame op true || 5 achievements
        if (control.wordGame[0] == true)
        {
            Lef.color = new Color32(255, 255, 255, 255);
        }
        if (control.wordGame[1] == true)
        {
            Luid.color = new Color32(255, 255, 255, 255);
        }
        if (control.wordGame[2] == true)
        {
            Warmte.color = new Color32(255, 255, 255, 255);
        }
        if (control.wordGame[3] == true)
        {
            Familie.color = new Color32(255, 255, 255, 255);
        }
        if (System.Array.TrueForAll(control.wordGame, item => item) == true)
        {
            SmoothTalker.color = new Color32(255, 255, 255, 255);
        }
    }

    // Tutorial
    public void TutorialButton1()
    {
        ShowAchievement();

        achievement.text = "Kindvriendelijk";

        unlock.text = "Nee";
        // if unlocked
        
        omschrijving.text = "Vindt alle kinderen in de tutorial.";
        image.sprite = GameObject.Find("Kindvriendelijk").GetComponent<Image>().sprite;
    }
    public void TutorialButton2()
    {
        ShowAchievement();

        achievement.text = "Lef";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[0] == true)
        {
            unlock.text = "Ja";            
        }

        omschrijving.text = "Los de eerste woordpuzzel op.";
        image.sprite = GameObject.Find("Lef").GetComponent<Image>().sprite;
    }
    public void TutorialButton3()
    {
        ShowAchievement();

        achievement.text = "Stilte voor de storm";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[0] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Versla de Stilte voor de eerste keer.";
        image.sprite = GameObject.Find("Stilte voor de storm").GetComponent<Image>().sprite;
    }   

    // Level 1
    public void Level1Button1()
    {
        ShowAchievement();

        achievement.text = "De hoogte in";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Voltooi Level 1 uit.";
        image.sprite = GameObject.Find("De hoogte in").GetComponent<Image>().sprite;
    }
    public void Level1Button2()
    {
        ShowAchievement();

        achievement.text = "Icarus";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Icarus").GetComponent<Image>().sprite;
    }
    public void Level1Button3()
    {
        ShowAchievement();

        achievement.text = "Kindervriend";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "Vindt alle kinderen in het eerste level.";
        image.sprite = GameObject.Find("Kindervriend").GetComponent<Image>().sprite;
    }
    public void Level1Button4()
    {
        ShowAchievement();

        achievement.text = "Luid";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Los de tweede woordpuzzel op.";
        image.sprite = GameObject.Find("Luid").GetComponent<Image>().sprite;
    }
    public void Level1Button5()
    {
        ShowAchievement();

        achievement.text = "Stilteverstoorder";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Versla de Stilte in het eerste level.";
        image.sprite = GameObject.Find("Stilteverstoorder").GetComponent<Image>().sprite;
    }   

    // Level 2
    public void Level2Button1()
    {
        ShowAchievement();

        achievement.text = "Droog over";

        // if unlocked
        unlock.text = "Nee";
        
        omschrijving.text = "";
        image.sprite = GameObject.Find("Droog over").GetComponent<Image>().sprite;
    }
    public void Level2Button2()
    {
        ShowAchievement();

        achievement.text = "IJsbreker";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Voltooi het tweede level.";
        image.sprite = GameObject.Find("IJsbreker").GetComponent<Image>().sprite;
    }
    public void Level2Button3()
    {
        ShowAchievement();

        achievement.text = "IJsvrij";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("IJsvrij").GetComponent<Image>().sprite;
    }
    public void Level2Button4()
    {
        ShowAchievement();

        achievement.text = "Redder in nood";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "Vindt alle kinderen in het tweede level.";
        image.sprite = GameObject.Find("Redder in nood").GetComponent<Image>().sprite;
    }
    public void Level2Button5()
    {
        ShowAchievement();

        achievement.text = "Warmte";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Los de derde woordpuzzel op.";
        image.sprite = GameObject.Find("Warmte").GetComponent<Image>().sprite;
    }
    public void Level2Button6()
    {
        ShowAchievement();

        achievement.text = "Stilteontregelaar";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Versla de Stilte voor de derde keer.";
        image.sprite = GameObject.Find("Stilteontregelaar").GetComponent<Image>().sprite;
    }   

    // Level 3
    public void Level3Button1()
    {
        ShowAchievement();

        achievement.text = "Intellectueel";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Voltooi het derde level.";
        image.sprite = GameObject.Find("Intellectueel").GetComponent<Image>().sprite;
    }
    public void Level3Button2()
    {
        ShowAchievement();

        achievement.text = "Held";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "Vindt alle kinderen in het derde level.";
        image.sprite = GameObject.Find("Held").GetComponent<Image>().sprite;
    }
    public void Level3Button3()
    {
        ShowAchievement();

        achievement.text = "Familie";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Los de laatste woordpuzzel op.";
        image.sprite = GameObject.Find("Familie").GetComponent<Image>().sprite;
    }
    public void Level3Button4()
    {
        ShowAchievement();

        achievement.text = "Stilteverbreker";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Versla de Stilte voor de laatste keer.";
        image.sprite = GameObject.Find("Stilteverbreker").GetComponent<Image>().sprite;
    }  

    // Overal
    public void Overal1Button1()
    {
        ShowAchievement();

        achievement.text = "Starting out";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[0] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Voltooi de tutorial.";
        image.sprite = GameObject.Find("Starting out").GetComponent<Image>().sprite;
    }
    public void Overal1Button2()
    {
        ShowAchievement();

        achievement.text = "Diplomaat";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Diplomaat").GetComponent<Image>().sprite;
    }
    public void Overal1Button3()
    {
        ShowAchievement();

        achievement.text = "Gulle gever";

        // if unlocked
        unlock.text = "Nee";       

        omschrijving.text = "Doneer aan KidsRights.";
        image.sprite = GameObject.Find("Gulle gever").GetComponent<Image>().sprite;
    }
    public void Overal1Button4()
    {
        ShowAchievement();

        achievement.text = "Levende legende";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Levende legende").GetComponent<Image>().sprite;
    }
    public void Overal1Button5()
    {
        ShowAchievement();

        achievement.text = "Onaantasbaar";

        // if unlocked        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Onaantasbaar").GetComponent<Image>().sprite;
    }
    public void Overal1Button6()
    {
        ShowAchievement();

        achievement.text = "Smooth talker";

        // if unlocked
        unlock.text = "Nee";
        if (System.Array.TrueForAll(GameControl.control.wordGame, item => item) == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Los alle woordpuzzels op.";
        image.sprite = GameObject.Find("Smooth talker").GetComponent<Image>().sprite;
    }
    public void Overal1Button7()
    {
        ShowAchievement();

        achievement.text = "Veteraan";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Veteraan").GetComponent<Image>().sprite;
    }
    public void Overal1Button8()
    {
        ShowAchievement();

        achievement.text = "WordUp!";

        // if unlocked
        unlock.text = "Nee";
        if (System.Array.TrueForAll(GameControl.control.unlockedLevels, item => item) == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "Voltooi alle levels in WordUp!";
        image.sprite = GameObject.Find("WordUp").GetComponent<Image>().sprite;
    }  


    void ShowAchievement()
    {
        PopUp.SetActive(true);        
    }

    void OnGUI()
    {
        button1Rect.x = (Screen.width / 2) - (button1Rect.width / 2);
        button1Rect.y = (Screen.height / 2) - (button1Rect.height / 2);

        button1Rect.y = button1Rect.y + 260;

        // Set the skin to use
        GUI.skin = skin;
        
        // Start Button
        if (GUI.Button(
            // Center in X, 2/3 of the height in Y
            button1Rect,
            "< Terug"
        ))
        {
            _audioSource.Play();
            this.gameObject.SetActive(false);
            mainMenu.GetComponent<MainMenu>()._mainMenuUit = false;         
        }
    }
}
