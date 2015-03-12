using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	// Variables //
	public RectTransform healthTransform; // healthbar in UI
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

	public Text letterText;
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

	// Methods //
	void Start()
	{
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
		onCoolDown = false;

		kindText.text = countKids + "  " + maxKids;
		letterText.text = countLetters + "  " + maxLetters;
	}

	private void HandleHealth()
	{
		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY);
	}
	private void HandleKids ()
	{
		kindText.text = countKids + "   " + maxKids;
		Boodschap.text = "Hoera, je hebt me gevonden!!!";
		StartCoroutine (showMessage());
	}
	private void HandleLetters ()
	{
		letterText.text = countLetters + "   " + maxLetters;
		Boodschap.text = "Je hebt de letter 'L' gevonden!";
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

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy") // Near enemy? Health goes down every 2 seconds
		{
			Debug.Log("Taking Damage");
			if (!onCoolDown && currentHealth > 0)
			{
				StartCoroutine(coolDownDMG());
				CurrentHealth -= 1;
			}
		}
		if (collision.gameObject.tag == "Enemy") // If health goes below zero, while near enemy
		{
			if (currentHealth == 0)
			{
				Respawn();
			}
		}

		if (collision.gameObject.tag == "Child") // If child is found
		{ 
			if (countKids < 5)
			{
				CountKids += 1;
				Destroy(collision.gameObject);

				if (currentHealth < 10) // If damaged, health increases
				{
					CurrentHealth += 1; // Child found health increases + 2
				}
			}
		}
		if (collision.gameObject.tag == "Letter") 
		{
			if (countLetters < 3)
			{
				CountLetters += 1;
				Destroy(collision.gameObject);
			}
		}
	}
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	void Respawn ()
	{
		GameMaster.KillPlayer (this);
		CurrentHealth += maxHealth;
		currentHealth = maxHealth;
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
