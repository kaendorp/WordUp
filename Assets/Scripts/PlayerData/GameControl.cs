using UnityEngine;
using System.Collections;

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

    public void LevelComplete(int level)
    {
        unlockedLevels[level] = true;

        if (level == 0)
        {
            // unlock
        }
        else if (level == 1)
        {
            // unlock
        }
        else if (level == 2)
        {
            // unlock
        }
        else if (level == 3)
        {
            // unlock
        }

        // Als alle items waar zijn
        if (System.Array.TrueForAll(unlockedLevels, item => item) == true)
        {
            // unlock Achievement          
        }
    }

    public void StilteVerslagen(int baas)
    {
        verslaStilte[baas] = true;

        if (baas == 0)
        {
            // unlock
        }
        else if (baas == 1)
        {
            // unlock
        }
        else if (baas == 2)
        {
            // unlock
        }
        else if (baas == 3)
        {
            // unlock
        }
        
        // Als alle items waar zijn
        if (System.Array.TrueForAll(verslaStilte, item => item) == true)
        {
            // unlock Achievement          
        }
    }

    public void WordGameComplete(int wg)
    {
        wordGame[wg] = true;

        if (wg == 0)
        {
            // unlock
        }
        else if (wg == 1)
        {
            // unlock
        }
        else if (wg == 2)
        {
            // unlock
        }
        else if (wg == 3)
        {
            // unlock
        }

        // Als alle items waar zijn
        if (System.Array.TrueForAll(wordGame, item => item) == true)
        {
            // unlock Achievement          
        }
    }

    void Update()
    {
        
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

    // toont de volgende waardes in elk level
    void OnGUI()
    {
        GUI.Label(new Rect(10, 70, 150, 30), "Level: " + loadLevel);
        GUI.Label(new Rect(10, 80, 150, 30), "Speler: " + selectPlayer);
    }
}
