using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class Player : MonoBehaviour {

	// Variables //
	public GameObject impactEffect;
	private RectTransform healthTransform; // healthbar in UI
	private Image visualHealth; // healthbar image to change color
	private float cachedY; // saved Y position, does not change
	private float minXValue; // minimal X position of healthbar
	private float maxXValue; // maximal X position of healthbar
	public int currentHealth; // current health value
	private int CurrentHealth // Sets health through HandelHealth()
	{
		get { return currentHealth;}
		set 
		{ 
			currentHealth = value;
			HandleHealth();
		}
	}
	public int maxHealth; // maximum value of health. Currently 10
	public float coolDown; // length of damage cooldown
	private bool onCoolDown; // Cooldown active or not	
    private RectTransform plusOne;

    //Boodschap
	private Text boodschap;

    public GameMaster gameMaster;

    // Kinderen
	[Header("KIDS")]
    public bool kindhealthPlus;
    public bool kindPlus;
	private Text kindText; // Kind counter for UI
    public Text kindTextHUD;
	private int countKids = 0; // current amount of kids collected 
	private int CountKids // Sets current amount of kids through HandleKids()
	{
		get { return countKids;}
		set 
		{ 
			countKids = value;
			HandleKids();
		}
	}
	public int maxKids; // Maximum amount of kids in level

	[Header("LETTERS")]
	private RectTransform letterPanel;

	public int countLetters;
	private int CountLetters // Sets current amount of kids through HandleKids()
	{
		get { return countLetters;}
		set 
		{ 
			countLetters = value;
			HandleLetters();
		}
	}
	public int maxLetters;

	// Fysieke locatie letters in HUD   
    public Text[] letters;
    public Text[] lettersHUD;

    // Zet menu's active
    private GameObject HUD;

	//audio get letters
	private AudioClip _audioSource;
	private Vector3 positie;

	//voice
	[Header("SPEECH")]
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;
	
	public float speed;
	private byte[] low;
	private AudioSource _playerSource;

	// Methods //
	void Start()
	{
        // HUD
        HUD = GameObject.Find("HUD");
        
        // Moed
        GameObject health = GameObject.Find("moedValue");
        healthTransform = health.GetComponent<RectTransform>();
        visualHealth = health.GetComponent<Image>();

        // Boodschap
        boodschap = GameObject.Find("Boodschap").GetComponent<Text>();  

        // Plus        
        plusOne = GameObject.Find("PlusOne").GetComponent<RectTransform>();
        plusOne.gameObject.SetActive(false);

        // kindteller
        kindText = GameObject.Find("kind_teller_tekst").GetComponent<Text>();        

        // lettercontainer        
        letterPanel = GameObject.Find("Letter_container").GetComponent<RectTransform>();
        letterPanel.gameObject.SetActive(false);        

		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
		onCoolDown = false;

		kindText.text = countKids + "  " + maxKids;
		kindTextHUD.text = countKids + "  " + maxKids;

		//audio
		_playerSource = gameObject.GetComponent<AudioSource> ();

        //find gamemaster
        gameMaster = FindObjectOfType<GameMaster>();

        // Analytics
        GameControl.control.damageTaken = 0; 
        GameControl.control.lettersFound = "";
        GameControl.control.kidsFound = 0;
        GameControl.control.bossBattleStarted = false;
        GameControl.control.bossDamageTaken = 0;
        GameControl.control.respawns = 0;
        GameControl.control.timesPaused = 0;
        GameControl.control.pauseDuration = 0;
	}

	private void HandleHealth()
	{
		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY);

		if (currentHealth > maxHealth / 2) // Health is green
		{ 
			visualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth/2, maxHealth, 200, 0), 200, 0, 255);
		}
		else // Health is red
		{ 
			visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth/2, 0, 255), 0, 255);
		}
	}
	private void HandleKids ()
	{
		kindText.text = countKids + "  " + maxKids;
        //PauseMenuScripte pause = HUD.GetComponent<PauseMenuScripte>();
        kindTextHUD.text = countKids + "  " + maxKids;
        //pause.SendMessage("KindPlus");
	}
	private void HandleLetters ()
	{
		boodschap.text = "Je hebt een letter gevonden!";
		StartCoroutine (showMessage());
	}

	IEnumerator showMessage()
	{
		yield return new WaitForSeconds (3);
		boodschap.text = "";;
	}

	IEnumerator coolDownDMG()
	{
		onCoolDown = true;
		StartCoroutine(PlaySound("Au"));
		yield return new WaitForSeconds (coolDown);
		onCoolDown = false;
	}

	IEnumerator showLetters()
	{
		letterPanel.gameObject.SetActive(true);
		yield return new WaitForSeconds (3);
		letterPanel.gameObject.SetActive(false);
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy") // Near enemy? Health goes down every 2 seconds
		{
            if (!onCoolDown && currentHealth > 0)
            {
                Object spawnedImpactEffect = Instantiate(impactEffect, transform.position, transform.rotation);
                StartCoroutine(coolDownDMG());
                CurrentHealth -= 1;
                GameControl.control.damageTaken++; // Used for analytics
                Destroy(spawnedImpactEffect, 1);
            }
            else if (currentHealth == 0)
            {
                Respawn();
            }

            if (LayerMask.LayerToName(collision.gameObject.layer) == "EnemyProjectile")
                Destroy(collision.gameObject);
		}        
		
        if (collision.gameObject.tag == "Water")
        {
            if (currentHealth > 0)
            {
                CurrentHealth-=1;
                gameMaster.Respawn();
            }
            else
            {
                Respawn();
            }            
        }

	}

	void OnTriggerEnter2D (Collider2D collision)
	{        
		if (collision.gameObject.tag == "Letter") 
		{
			if (countLetters < maxLetters)
			{
                // Aantal letters gevonden
                CountLetters += 1;

				//speel geluidje
				_audioSource = collision.gameObject.GetComponent<AudioSource>().clip;
				positie = gameObject.transform.position;
				AudioSource.PlayClipAtPoint (_audioSource, positie);

                Destroy(collision.gameObject);
                StartCoroutine(showLetters());

                GameControl.control.lettersFound += collision.gameObject.name;

                // Aantal letters is 7
                switch (maxLetters)
                {
                    case 3:
                        if (letters[0].text == "")
                        {                            
                             letters[0].text = collision.gameObject.name;
                             lettersHUD[0].text = collision.gameObject.name;
                        }
                        else if (letters[1].text == "")
                        {
                            letters[1].text = collision.gameObject.name;
                            lettersHUD[1].text = collision.gameObject.name;
                        }
                        else if (letters[2].text == "")
                        {
                            letters[2].text = collision.gameObject.name;
                            lettersHUD[2].text = collision.gameObject.name;
                        }                                         
                        break;

                    case 4:
                        if (letters[0].text == "")
                        {
                            letters[0].text = collision.gameObject.name;
                            lettersHUD[0].text = collision.gameObject.name;
                        }
                        else if (letters[1].text == "")
                        {
                            letters[1].text = collision.gameObject.name;
                            lettersHUD[1].text = collision.gameObject.name;
                        }
                        else if (letters[2].text == "")
                        {
                            letters[2].text = collision.gameObject.name;
                            lettersHUD[2].text = collision.gameObject.name;
                        }
                        else if (letters[3].text == "")
                        {
                            letters[3].text = collision.gameObject.name;
                            lettersHUD[3].text = collision.gameObject.name;
                        }
                        break;

                    case 6:
                        if (letters[0].text == "")
                        {
                            letters[0].text = collision.gameObject.name;
                            lettersHUD[0].text = collision.gameObject.name;
                        }
                        else if (letters[1].text == "")
                        {
                            letters[1].text = collision.gameObject.name;
                            lettersHUD[1].text = collision.gameObject.name;
                        }
                        else if (letters[2].text == "")
                        {
                            letters[2].text = collision.gameObject.name;
                            lettersHUD[2].text = collision.gameObject.name;
                        }
                        else if (letters[3].text == "")
                        {
                            letters[3].text = collision.gameObject.name;
                            lettersHUD[3].text = collision.gameObject.name;
                        }
                        else if (letters[4].text == "")
                        {
                            letters[4].text = collision.gameObject.name;
                            lettersHUD[4].text = collision.gameObject.name;
                        }
                        else if (letters[5].text == "")
                        {
                            letters[5].text = collision.gameObject.name;
                            lettersHUD[5].text = collision.gameObject.name;
                        }
                        break;
                    
                    case 7:
                        if (letters[0].text == "")
                        {
                            letters[0].text = collision.gameObject.name;
                            lettersHUD[0].text = collision.gameObject.name;
                        }
                        else if (letters[1].text == "")
                        {
                            letters[1].text = collision.gameObject.name;
                            lettersHUD[1].text = collision.gameObject.name;
                        }
                        else if (letters[2].text == "")
                        {
                            letters[2].text = collision.gameObject.name;
                            lettersHUD[2].text = collision.gameObject.name;
                        }
                        else if (letters[3].text == "")
                        {
                            letters[3].text = collision.gameObject.name;
                            lettersHUD[3].text = collision.gameObject.name;
                        }
                        else if (letters[4].text == "")
                        {
                            letters[4].text = collision.gameObject.name;
                            lettersHUD[4].text = collision.gameObject.name;
                        }
                        else if (letters[5].text == "")
                        {
                            letters[5].text = collision.gameObject.name;
                            lettersHUD[5].text = collision.gameObject.name;
                        }
                        else if (letters[6].text == "")
                        {
                            letters[6].text = collision.gameObject.name;
                            lettersHUD[6].text = collision.gameObject.name;
                        }
                        break;
                }		
			}
		}

		if (countLetters >= maxLetters) 
		{
			if (collision.gameObject.tag == "WordGame")
			{
				boodschap.text = "";
				letterPanel.gameObject.SetActive(false);
				Destroy(collision.gameObject);				
                HUD.GetComponent<WordGameScript>().Active = true;
			}			
		}
		
		if (collision.gameObject.tag == "Finish") 
		{
			boodschap.text = "";
			letterPanel.gameObject.SetActive(false);
			
            HUD.GetComponent<WinMenuScript>().WinActive = true;
		}

        if (collision.gameObject.tag == "Tand")
        {
            Rigidbody2D rb;

            // Get Rigidbody
            if (GameObject.Find("Player") == null)
            {
                rb = GameObject.Find("Player2").GetComponent<Rigidbody2D>();
            }
            else
            {
                rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            }

            if (currentHealth > 0)
            {
                rb.AddForce(Vector3.up * 300);
                
                if (!onCoolDown)
                {
                    Object spawnedImpactEffect = Instantiate(impactEffect, transform.position, transform.rotation);
                    StartCoroutine(coolDownDMG());
                    CurrentHealth -= 1;
                    Destroy(spawnedImpactEffect, 1);
                }             
            }
            else
            {
                Respawn();
            } 
        }

        if (collision.gameObject.tag == "ijstand") // Near enemy? Health goes down every 2 seconds
        {
            if (!onCoolDown && currentHealth > 0)
            {
                StartCoroutine(coolDownDMG());
                CurrentHealth -= 1;
            }
            else if (currentHealth == 0)
            {
                Respawn();
            }
        }      
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	void Respawn ()
	{
		// Leegt boodschap text, dit bleef anders constant aanstaan.
		if (boodschap.text.Length > 1) 
		{
			boodschap.text = "";
		}

		Destroy (this.gameObject);		
        HUD.GetComponent<GameOverScript>().GameOverActive = true;
	}

	void Update () 
	{

        if (HUD.GetComponent<WordGameScript>().Active != true && HUD.GetComponent<WinMenuScript>().WinActive != true && HUD.GetComponent<GameOverScript>().GameOverActive != true && HUD.GetComponent<BerichtenMenuController>().berichtMakerActive != true)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                boodschap.text = "";
                letterPanel.gameObject.SetActive(false);
                GameControl.control.timesPaused++;

                HUD.GetComponent<PauseMenuScripte>().PauseActive = true;
            }
        }		

        if (kindhealthPlus == true)
        {
            if (countKids < maxKids)
            {               
                if (currentHealth < 10) // If damaged, health increases
                {
                    CurrentHealth += 1; // Child found health increases + 1
                    StartCoroutine(PlusOneActive()); 
                }                
            }
            kindhealthPlus = false;
        }
        if (kindPlus == true)
        {
            if (countKids < maxKids)
            {
                CountKids += 1;
                GameControl.control.kidsFound++;
            }
            kindPlus = false;
        }
	}

    IEnumerator PlusOneActive()
    {
        plusOne.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        plusOne.gameObject.SetActive(false);
    }

	/**
     * Converts any string in message to sound
     */
	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			//Debug.Log (b);
			if(b < 65)
			{
				_playerSource.clip = number1;
				_playerSource.volume = 0.2f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 65 && b < 105)
			{
				_playerSource.clip = number2;
				_playerSource.volume = 0.4f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 105 && b < 115)
			{	
				_playerSource.clip = number3;
				_playerSource.volume = 0.6f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else
			{	
				_playerSource.clip = number4;
				_playerSource.volume = 0.8f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
		}
	}

    /**
     * Sends the selected player and level to analytics
     * 
     * Called by Respawn()
     */
    void StartGameAnalytics()
    {
        string customEventName = "PlayerDeath" + Application.loadedLevelName;

        if (!GameControl.control.bossBattleStarted)
        {
            UnityAnalytics.CustomEvent(customEventName, new Dictionary<string, object>
            {
                { "runningTime", Time.timeSinceLevelLoad },
                { "damageTaken", GameControl.control.damageTaken },
                { "projectile1Shot", GameControl.control.projectile1Shot },
                { "projectile2Shot", GameControl.control.projectile2Shot },
                { "projectile3Shot", GameControl.control.projectile3Shot },
                { "kidsFound", GameControl.control.kidsFound },
                { "lettersFound", GameControl.control.lettersFound },
                { "enemyDefeated", GameControl.control.enemiesDefeated },
                { "respawns", GameControl.control.respawns },
                { "timesPaused", GameControl.control.timesPaused },
            });
        }
        else
        {
            float bossBattleDuration = (Time.timeSinceLevelLoad - GameControl.control.bossBattleStartTime);

            UnityAnalytics.CustomEvent(customEventName + "Boss", new Dictionary<string, object>
            {
                { "runningTime", Time.timeSinceLevelLoad },
                { "damageTaken", GameControl.control.damageTaken },
                { "projectile1Shot", GameControl.control.projectile1Shot },
                { "projectile2Shot", GameControl.control.projectile2Shot },
                { "projectile3Shot", GameControl.control.projectile3Shot },
                { "bossBattleHealth", GameControl.control.bossDamageTaken },
                { "bossBattleDuration", bossBattleDuration },
                { "timesPaused", GameControl.control.timesPaused },
                { "pauseDuration", GameControl.control.pauseDuration },
            });
        }
    }
}

