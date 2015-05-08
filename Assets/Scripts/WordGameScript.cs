using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WordGameScript : MonoBehaviour {

	private GUISkin skin;
	private GUISkin menuskin;

	public Image controle;	    

    public Text[] lettersText;

    public int aantalLetters;

	public bool Active = false;
	public bool WGcomplete = false;
	public RectTransform Wordgame;

    public Text gevondenLetters;

	private int klik;

    // Welke letters, welk woord
    public string nieuwWoord;
    public string[] letterKeuze;    

	// Buttons plaatsing
	private Rect button1Rect = new Rect(15,15,160,30);
	private Rect letter1Rect = new Rect(15, 15, 50, 50);
	private Rect letter2Rect = new Rect(15, 15, 50, 50);
	private Rect letter3Rect = new Rect(15, 15, 50, 50);
    private Rect letter4Rect = new Rect(15, 15, 50, 50);
    private Rect letter5Rect = new Rect(15, 15, 50, 50);
    private Rect letter6Rect = new Rect(15, 15, 50, 50);
    private Rect letter7Rect = new Rect(15, 15, 50, 50);	

    public int wordGameLevel;

	// Use this for initialization
	void Start ()
	{
		skin = Resources.Load("WordgameButton") as GUISkin;
		menuskin = Resources.Load("ButtonSkin") as GUISkin;		       
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (aantalLetters == 3)
        {
            if (lettersText[0].text + lettersText[1].text + lettersText[2].text == nieuwWoord)
            {
                controle.color = new Color32(19, 182, 40, 225);
                WGcomplete = true;
            }
            else if (klik == 3)
            {
                controle.color = new Color32(250, 66, 66, 225);

                lettersText[0].text = "";
                lettersText[1].text = "";
                lettersText[2].text = "";
                klik = 0;
            }
        }
        if (aantalLetters == 4)
        {
            if (lettersText[0].text + lettersText[1].text + lettersText[2].text + lettersText[3].text == nieuwWoord)
            {
                controle.color = new Color32(19, 182, 40, 225);
                WGcomplete = true;
            }
            else if (klik == 4)
            {
                controle.color = new Color32(250, 66, 66, 225);

                lettersText[0].text = "";
                lettersText[1].text = "";
                lettersText[2].text = "";
                lettersText[3].text = "";
                klik = 0;
            }
        }
        if (aantalLetters == 6)
        {
            if (lettersText[0].text + lettersText[1].text + lettersText[2].text + lettersText[3].text + lettersText[4].text + lettersText[5].text == nieuwWoord)
            {
                controle.color = new Color32(19, 182, 40, 225);
                WGcomplete = true;
            }
            else if (klik == 6)
            {
                controle.color = new Color32(250, 66, 66, 225);

                lettersText[0].text = "";
                lettersText[1].text = "";
                lettersText[2].text = "";
                lettersText[3].text = "";
                lettersText[4].text = "";
                lettersText[5].text = "";
                klik = 0;
            }
        }
        if (aantalLetters == 7)
        {
            if (lettersText[0].text + lettersText[1].text + lettersText[2].text + lettersText[3].text + lettersText[4].text + lettersText[5].text + lettersText[6].text == nieuwWoord)
            {
                controle.color = new Color32(19, 182, 40, 225);
                WGcomplete = true;
            }
            else if (klik == 7)
            {
                controle.color = new Color32(250, 66, 66, 225);

                lettersText[0].text = "";
                lettersText[1].text = "";
                lettersText[2].text = "";
                lettersText[3].text = "";
                lettersText[4].text = "";
                lettersText[5].text = "";
                lettersText[5].text = "";
                klik = 0;
            }
        }    		
	}	

	void OnGUI()
	{
        button1Rect.x = (Screen.width / 2) - (button1Rect.width / 2);
        button1Rect.y = (Screen.height / 2) - (button1Rect.height / 2);

        letter1Rect.x = (Screen.width / 2) - (letter1Rect.width / 2);
        letter1Rect.y = (Screen.height / 2) - (letter1Rect.height / 2);

        letter2Rect.x = (Screen.width / 2) - (letter2Rect.width / 2);
        letter2Rect.y = (Screen.height / 2) - (letter2Rect.height / 2);

        letter3Rect.x = (Screen.width / 2) - (letter3Rect.width / 2);
        letter3Rect.y = (Screen.height / 2) - (letter3Rect.height / 2);

        letter4Rect.x = (Screen.width / 2) - (letter4Rect.width / 2);
        letter4Rect.y = (Screen.height / 2) - (letter4Rect.height / 2);

        letter5Rect.x = (Screen.width / 2) - (letter5Rect.width / 2);
        letter5Rect.y = (Screen.height / 2) - (letter5Rect.height / 2);

        letter6Rect.x = (Screen.width / 2) - (letter6Rect.width / 2);
        letter6Rect.y = (Screen.height / 2) - (letter6Rect.height / 2);

        letter7Rect.x = (Screen.width / 2) - (letter7Rect.width / 2);
        letter7Rect.y = (Screen.height / 2) - (letter7Rect.height / 2);
		
		if (Active == true) 
		{
            if (aantalLetters == 3)
            {
                letter1Rect.x = letter1Rect.x - 55;
                letter1Rect.y = letter1Rect.y + 70;

                letter2Rect.y = letter2Rect.y + 70;

                letter3Rect.x = letter3Rect.x + 55;
                letter3Rect.y = letter3Rect.y + 70;
            }
            if (aantalLetters == 4)
            {
                letter1Rect.x = letter1Rect.x - 82.5f;
                letter1Rect.y = letter1Rect.y + 70;

                letter2Rect.x = letter2Rect.x - 27.5f;
                letter2Rect.y = letter2Rect.y + 70;

                letter3Rect.x = letter3Rect.x + 27.5f;
                letter3Rect.y = letter3Rect.y + 70;

                letter4Rect.x = letter4Rect.x + 82.5f;
                letter4Rect.y = letter4Rect.y + 70;
            }
            if (aantalLetters == 6)
            {
                letter1Rect.x = letter1Rect.x - 137.5f;
                letter1Rect.y = letter1Rect.y + 70;

                letter2Rect.x = letter2Rect.x - 82.5f;
                letter2Rect.y = letter2Rect.y + 70;

                letter3Rect.x = letter3Rect.x - 27.5f;
                letter3Rect.y = letter3Rect.y + 70;

                letter4Rect.x = letter4Rect.x + 27.5f;
                letter4Rect.y = letter4Rect.y + 70;

                letter5Rect.x = letter5Rect.x + 82.5f;
                letter5Rect.y = letter5Rect.y + 70;

                letter6Rect.x = letter6Rect.x + 137.5f;
                letter6Rect.y = letter6Rect.y + 70;
            }
            if (aantalLetters == 7)
            {
                letter1Rect.x = letter1Rect.x - 165;
                letter1Rect.y = letter1Rect.y + 70;

                letter2Rect.x = letter2Rect.x - 110;
                letter2Rect.y = letter2Rect.y + 70;

                letter3Rect.x = letter3Rect.x - 55;
                letter3Rect.y = letter3Rect.y + 70;

                letter4Rect.x = letter4Rect.x + 0;
                letter4Rect.y = letter4Rect.y + 70;

                letter5Rect.x = letter5Rect.x + 55;
                letter5Rect.y = letter5Rect.y + 70;

                letter6Rect.x = letter6Rect.x + 110;
                letter6Rect.y = letter6Rect.y + 70;

                letter7Rect.x = letter7Rect.x + 165;
                letter7Rect.y = letter7Rect.y + 70;
            }


            if (aantalLetters >= 3)
            {               
                Time.timeScale = 0;

                Wordgame.gameObject.SetActive(true);

                // Set the skin to use
                GUI.skin = skin;
                
                // Eerste letter
                if (GUI.Button(  
                    letter1Rect,
                    letterKeuze[0]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[0];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[0];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[0];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[0];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[0];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[0];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[0];
                    }
                }
                
                // Tweede letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter2Rect,
                    letterKeuze[1]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[1];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[1];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[1];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[1];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[1];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[1];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[1];
                    }
                }
               
                // Derde letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter3Rect,
                    letterKeuze[2]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[2];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[2];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[2];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[2];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[2];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[2];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[2];
                    }
                }
            }
			
            if (aantalLetters >= 4)
            {          
                // Vierde letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter4Rect,
                    letterKeuze[3]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[3];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[3];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[3];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[3];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[3];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[3];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[3];
                    }
                }
            }

            if (aantalLetters >= 6)
            {            
                // Vierde letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter5Rect,
                    letterKeuze[4]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[4];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[4];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[4];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[4];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[4];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[4];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[4];
                    }
                }
                
                // Vierde letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter6Rect,
                    letterKeuze[5]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[5];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[5];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[5];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[5];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[5];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[5];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[5];
                    }
                }

            }

            if (aantalLetters >= 7)
            {           
                // Vierde letter
                if (GUI.Button(
                    // Center in X, 2/3 of the height in Y
                    letter7Rect,
                    letterKeuze[6]
                    ))
                {
                    klik += 1;
                    if (lettersText[0].text == "")
                    {
                        lettersText[0].text = letterKeuze[6];
                    }
                    else if (lettersText[1].text == "")
                    {
                        lettersText[1].text = letterKeuze[6];
                    }
                    else if (lettersText[2].text == "")
                    {
                        lettersText[2].text = letterKeuze[6];
                    }
                    else if (lettersText[3].text == "")
                    {
                        lettersText[3].text = letterKeuze[6];
                    }
                    else if (lettersText[4].text == "")
                    {
                        lettersText[4].text = letterKeuze[6];
                    }
                    else if (lettersText[5].text == "")
                    {
                        lettersText[5].text = letterKeuze[6];
                    }
                    else if (lettersText[6].text == "")
                    {
                        lettersText[6].text = letterKeuze[6];
                    }
                }
            }
		}

		if (WGcomplete == true) 
		{
            // Stuur bericht naar Gamecontrol voor achievements
            GameControl.control.WordGameComplete(wordGameLevel);

            Active = false;
            gevondenLetters.gameObject.SetActive(false);
			// Plaatsing buttons
			button1Rect.y = button1Rect.y + 125;			

			// Set the skin to use
			GUI.skin = menuskin;
			
			// Correct Button
			if (GUI.Button (
				// Center in X, 2/3 of the height in Y
				button1Rect,
				"Correct"
				)) 
			{				
				WGcomplete = false;
                lettersText[0].text = "";
                lettersText[1].text = "";
                lettersText[2].text = "";
				Time.timeScale = 1;
				Wordgame.gameObject.SetActive(false);
			}
		}
	}
}
