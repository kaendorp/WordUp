using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	// Variables //
	public GameObject impactEffect;
	public RectTransform healthTransform; // healthbar in UI
	public Image visualHealth; // healthbar image to change color
	private float cachedY; // saved Y position, does not change
	private float minXValue; // minimal X position of healthbar
	private float maxXValue; // maximal X position of healthbar
	private int currentHealth; // current health value
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

	public int fallBoundary = -10;

	public Text Boodschap;

	public Text kindText; // Kid counter for UI
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
	public int maxKids = 5; // Maximum amount of kids in level

	public RectTransform letterPanel;

	private int countLetters;
	private int CountLetters // Sets current amount of kids through HandleKids()
	{
		get { return countLetters;}
		set 
		{ 
			countLetters = value;
			HandleLetters();
		}
	}
	public int maxLetters = 3;

	// Fysieke locatie letters in HUD
	public Text letter_1;
	public Text letter_2;
	public Text letter_3;

	// Methods //
	void Start()
	{
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
		onCoolDown = false;

		kindText.text = countKids + "  " + maxKids;
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
		kindText.text = countKids + "   " + maxKids;
		Boodschap.text = "Hoera, je hebt me gevonden!!!";
		StartCoroutine (showMessage());
	}
	private void HandleLetters ()
	{
		Boodschap.text = "Je hebt een letter gevonden!";
		StartCoroutine (showMessage());
	}

	IEnumerator showMessage()
	{
		yield return new WaitForSeconds (3);
		Boodschap.text = "";;
	}

	IEnumerator coolDownDMG()
	{
		onCoolDown = true;
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
				Instantiate (impactEffect, transform.position, transform.rotation);
				StartCoroutine(coolDownDMG());
				CurrentHealth -= 1;
			}
			Destroy (impactEffect, 1);
		}
		if (collision.gameObject.tag == "Enemy") // If health goes below zero, while near enemy
		{
			if (currentHealth == 0)
			{
				Respawn();
			}
		}
		
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "Bericht")
		{
			Boodschap.text = "Gebruik 'W' om te springen!";
		}

		if (collision.gameObject.tag == "Child") // If child is found
		{ 
			if (countKids < 5)
			{
				CountKids += 1;
				Destroy(collision.gameObject);
				
				if (currentHealth < 10) // If damaged, health increases
				{
					CurrentHealth += 1; // Child found health increases + 1
				}
			}
		}
		if (collision.gameObject.tag == "Letter") 
		{
			if (countLetters < 3)
			{
				CountLetters += 1;
				if (letter_1.text == "")
				{
					letter_1.text = collision.gameObject.name;
				}
				else if (letter_2.text == "")
				{
					letter_2.text = collision.gameObject.name;
				}
				else if (letter_3.text == "")
				{
					letter_3.text = collision.gameObject.name;
				}
				Destroy(collision.gameObject);
				StartCoroutine(showLetters());
			}
		}
	}
	void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.gameObject.tag == "Bericht")
		{
			Boodschap.text = "";
		}
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	void Respawn ()
	{
		// Leegt boodschap text, dit bleef anders constant aanstaan.
		if (Boodschap.text.Length > 1) 
		{
			Boodschap.text = "";
		}

		Destroy (this.gameObject);
		GameOverScript.MenuActive = true;
	}

	void Update () 
	{
		if (transform.position.y <= fallBoundary) 
		{
			FallDamage(10);
		}
	}

	void FallDamage(int dmg)
	{
		currentHealth = currentHealth - dmg;

		if (currentHealth == 0) 
		{
			Respawn();
		}
	}

}
