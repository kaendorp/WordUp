using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;

public class GameControl : MonoBehaviour {    

    // GameControl.control kan je in elk level aanroepen
    // Vanuit daar elke public waarde
    public static GameControl control;
    
    // Level select
    public string loadLevel = "Tutorial";   

    // Player select
    public string selectPlayer;
    public bool isMainMenu;

    // Highscore
    public int highScore;

    // Achievements level
    // Tutorial = 0 || Level 1 = 1 || Level 2 = 2 || Level 3 = 3
    public bool[] unlockedLevels = new bool[4];

    // Achievements Boss
    public bool[] verslaStilte = new bool[4];

    // Achievements Wordgame
    public bool[] wordGame = new bool[4];

    // Achievements 
    public bool kinderenTutorial;
    public bool[] kinderenLevel1 = new bool[4];
    public bool[] kinderenLevel2 = new bool[5];
    public bool[] kinderenLevel3 = new bool[6];

    public List<string> namen = new List<string>();

    public void LevelComplete(int level)
    {
        unlockedLevels[level] = true;

        if (level == 0)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_HetAvontuurBegint.html".ToString());
        }
        else if (level == 1)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_DeHoogteIn.html".ToString());
        }
        else if (level == 2)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Ijsbreker.html".ToString());            
        }
        else if (level == 3)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Intellectueel.html".ToString());
        }

        // Als alle items waar zijn
        if (System.Array.TrueForAll(unlockedLevels, item => item) == true)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_WordUp.html".ToString());
        }
    }

    public void StilteVerslagen(int baas)
    {
        verslaStilte[baas] = true;

        if (baas == 0)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_StilteVoorDeStorm.html".ToString());
        }
        else if (baas == 1)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_StilteVerstoorder.html".ToString());
        }
        else if (baas == 2)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_StilteOntregelaar.html".ToString());
        }
        else if (baas == 3)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_StilteVerbreker.html".ToString());
        }      
    }

    public void WordGameComplete(int wg)
    {
        wordGame[wg] = true;

        if (wg == 0)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Lef.html".ToString());
        }
        else if (wg == 1)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Luid.html".ToString());
        }
        else if (wg == 2)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Warmte.html".ToString());
        }
        else if (wg == 3)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Familie.html".ToString());
        }

        // Als alle items waar zijn
        if (System.Array.TrueForAll(wordGame, item => item) == true)
        {
            FBAchievement.fbControl.GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_VlotteSpreker.html".ToString());         
        }
    }

    public void AchievementCheck()
    {
        if (isMainMenu == true)
        {                       
            FBAchievement.fbControl.GetAllAchievements();                                
        }
    }

	void Awake () 
    {   
        // Creerd GameControl als deze er niet is en vangt af als hij er wel al is
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }        

        loadLevel = "Tutorial";
        selectPlayer = "Fynn";

        // Laad de juist speler
        GameObject fynn = GameObject.Find("Player");
        GameObject fiona = GameObject.Find("Player2");

        if (isMainMenu == false)
        {            
            if (GameControl.control.selectPlayer == "Fynn")
            {                
                fynn.SetActive(true);
                fiona.SetActive(false);
            }
            else if (GameControl.control.selectPlayer == "Fiona")
            {                
                fynn.SetActive(false);
                fiona.SetActive(true);
            }
        }        
	}

    // Zet de waardes van de achievements goed
    void OnGUI()
    {
		if (FB.IsLoggedIn) {
			namen = FBAchievement.fbControl.namen;               
		}
        // Zet level unlock op true || 5 Achievements
        if (namen.Contains("Het avontuur begint"))
        {
            unlockedLevels[0] = true;
        }
        if (namen.Contains("De hoogte in"))
        {
            unlockedLevels[1] = true;
        }
        if (namen.Contains("Ijsbreker"))
        {
            unlockedLevels[2] = true;
        }
        if (namen.Contains("Intellectueel"))
        {
            unlockedLevels[3] = true;
        }

        // Zet Stilte verslagen op true || 4 achievements
        if (namen.Contains("Stilte voor de storm"))
        {
            verslaStilte[0] = true;
        }
        if (namen.Contains("Stilteverstoorder"))
        {
            verslaStilte[1] = true;
        }
        if (namen.Contains("Stilteontregelaar"))
        {
            verslaStilte[2] = true;
        }
        if (namen.Contains("Stilteverbreker"))
        {
            verslaStilte[3] = true;
        }

        // Zet Wordgame op true || 5 achievements
        if (namen.Contains("Lef"))
        {
            wordGame[0] = true;
        }
        if (namen.Contains("Luid"))
        {
            wordGame[1] = true;
        }
        if (namen.Contains("Warmte"))
        {
            wordGame[2] = true;
        }
        if (namen.Contains("Familie"))
        {
            wordGame[3] = true;
        }         
    }
}
