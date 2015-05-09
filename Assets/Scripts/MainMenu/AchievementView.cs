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
    public GameObject button;   

    private GUISkin skin;    

    private Rect button1Rect = new Rect(15, 15, 160, 30);    

    public AudioSource _audioSource;

    void Start()
    {
        // Load a skin for the buttons
        skin = Resources.Load("ButtonSkin") as GUISkin;
    }

    // Tutorial
    public void TutorialButton1()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Kindvriendelijk";

        string list = string.Join(",", GameControl.control.namen.ToArray());

        unlock.text = "Nee";
        // if unlocked
        
        omschrijving.text = "";
        image.sprite = GameObject.Find("Kindvriendelijk").GetComponent<Image>().sprite;
    }
    public void TutorialButton2()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Lef";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[0] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Lef").GetComponent<Image>().sprite;
    }
    public void TutorialButton3()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Stilte voor de storm";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[0] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Stilte voor de storm").GetComponent<Image>().sprite;
    }   

    // Level 1
    public void Level1Button1()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "De hoogte in";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("De hoogte in").GetComponent<Image>().sprite;
    }
    public void Level1Button2()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Icarus";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Icarus").GetComponent<Image>().sprite;
    }
    public void Level1Button3()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Kindervriend";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Kindervriend").GetComponent<Image>().sprite;
    }
    public void Level1Button4()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Luid";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Luid").GetComponent<Image>().sprite;
    }
    public void Level1Button5()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Stilteverstoorder";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[1] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Stilteverstoorder").GetComponent<Image>().sprite;
    }   

    // Level 2
    public void Level2Button1()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Droog over";

        // if unlocked
        unlock.text = "Nee";
        
        omschrijving.text = "";
        image.sprite = GameObject.Find("Droog over").GetComponent<Image>().sprite;
    }
    public void Level2Button2()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "IJsbreker";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("IJsbreker").GetComponent<Image>().sprite;
    }
    public void Level2Button3()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "IJsvrij";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("IJsvrij").GetComponent<Image>().sprite;
    }
    public void Level2Button4()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Redder in nood";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Redder in nood").GetComponent<Image>().sprite;
    }
    public void Level2Button5()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Warmte";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Warmte").GetComponent<Image>().sprite;
    }
    public void Level2Button6()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Stilteontregelaar";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[2] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Stilteontregelaar").GetComponent<Image>().sprite;
    }   

    // Level 3
    public void Level3Button1()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Intellectueel";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Intellectueel").GetComponent<Image>().sprite;
    }
    public void Level3Button2()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Held";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Held").GetComponent<Image>().sprite;
    }
    public void Level3Button3()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Familie";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.wordGame[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Familie").GetComponent<Image>().sprite;
    }
    public void Level3Button4()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Stilteverbreker";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.verslaStilte[3] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Stilteverbreker").GetComponent<Image>().sprite;
    }  

    // Overal
    public void Overal1Button1()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Starting out";

        // if unlocked
        unlock.text = "Nee";
        if (GameControl.control.unlockedLevels[0] == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Starting out").GetComponent<Image>().sprite;
    }
    public void Overal1Button2()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Diplomaat";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Diplomaat").GetComponent<Image>().sprite;
    }
    public void Overal1Button3()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Gulle gever";

        // if unlocked
        unlock.text = "Nee";       

        omschrijving.text = "";
        image.sprite = GameObject.Find("Gulle gever").GetComponent<Image>().sprite;
    }
    public void Overal1Button4()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Levende legende";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Levende legende").GetComponent<Image>().sprite;
    }
    public void Overal1Button5()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Onaantasbaar";

        // if unlocked        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Onaantasbaar").GetComponent<Image>().sprite;
    }
    public void Overal1Button6()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Smooth talker";

        // if unlocked
        unlock.text = "Nee";
        if (System.Array.TrueForAll(GameControl.control.wordGame, item => item) == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("Smooth talker").GetComponent<Image>().sprite;
    }
    public void Overal1Button7()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "Veteraan";

        // if unlocked
        unlock.text = "Nee";        

        omschrijving.text = "";
        image.sprite = GameObject.Find("Veteraan").GetComponent<Image>().sprite;
    }
    public void Overal1Button8()
    {
        StartCoroutine(ShowAchievement());

        achievement.text = "WordUp!";

        // if unlocked
        unlock.text = "Nee";
        if (System.Array.TrueForAll(GameControl.control.unlockedLevels, item => item) == true)
        {
            unlock.text = "Ja";
        }

        omschrijving.text = "";
        image.sprite = GameObject.Find("WordUp").GetComponent<Image>().sprite;
    }  


    IEnumerator ShowAchievement()
    {
        PopUp.SetActive(true);
        yield return new WaitForSeconds(2);
        PopUp.SetActive(false);
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
            mainMenu.SetActive(true);            
        }
    }
}
