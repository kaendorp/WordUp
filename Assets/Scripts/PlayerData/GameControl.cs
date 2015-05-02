using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

    // GameControl.control kan je in elk level aanroepen
    // Vanuit daar elke public waarde
    public static GameControl control;
    
    // Level select
    public string loadLevel = "Tutorial";
    // Tutorial = 0 || Level 1 = 1 || Level 2 = 2 || Level 3 = 3
    public bool[] unlockedLevels = new bool[4];

    // Player select
    public string selectPlayer;
    public bool isMainMenu;

    // Achievements
    

    // Highscore
    public int aantalGevondenKinderen;
    public Time tijd;


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

        // Laad de juist speler
        GameObject fynn = GameObject.Find("Player");
        GameObject fiona = GameObject.Find("Player2");

        if (isMainMenu == false)
        {
            Debug.Log("NotmainMenu");
            if (GameControl.control.selectPlayer == "Fynn")
            {
                Debug.Log("Fynn");
                fynn.SetActive(true);
                fiona.SetActive(false);
            }
            else if (GameControl.control.selectPlayer == "Fiona")
            {
                Debug.Log("Fiona");
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
