using UnityEngine;
using System.Collections;

public class TurningCogBlue : MonoBehaviour {
	private bool rotated;
	private Vector3 rotation = Vector3.zero;
	private bool lampisaan;

	public float angle; //angle is new position -this appears to bug for -100, but -10 gives results as if -100

	public GameObject[] platformen;
	public GameObject[] platformenOff;
	public GameObject[] alwayson;

	public GameObject[] punt;
	public GameObject spotlight;
	public GameObject[] chainweightLamp;

	private AudioClip _audioSource;
	private Vector3 positie;

	// Use this for initialization
	void Start () 
	{
		rotated = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//deze kan op false worden gezet, mits de collider buiten de draaiing 
		//komt te staan :/ als dat gebeurt, dan wel trigger enabled laten, 
		//ook, dan triggered het geluid meerdere keren
		if (rotated == true) 
		{
			Rotate ();
		}
	}

	void LampAan(bool aan)
	{
		lampisaan = true;
		ShowPlatform(lampisaan);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player" && rotated == false)
		{
			rotated = true;
			rotation.z = angle;

			//voorkom triggeren nog een geluidje door random springen
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			//play sound
			AudioSource.PlayClipAtPoint (_audioSource, positie, 0.1f);
		}
	}

	void Rotate()
	{
		gameObject.transform.rotation = Quaternion.RotateTowards (gameObject.transform.rotation, Quaternion.Euler(0, 0, rotation.z), Time.deltaTime * 20);

		int teller = 0;
		foreach(GameObject chain in chainweightLamp)
		{
			chain.transform.position = punt[teller].transform.position; 
			teller++;
		}

		//kijken welke platformen aan moeten staan
		ShowPlatform(lampisaan);
	}

	void ShowPlatform(bool lampisaan)
	{
		if (lampisaan == false) {
			foreach (GameObject o in platformen) {
				o.SetActive (false);
			}  
			foreach (GameObject o in platformenOff) {
				o.SetActive (false);
			}
		} else {
			foreach (GameObject o in alwayson) {
				o.SetActive (true);
			}  
		}

		if (rotated == false && lampisaan) 
		{
			foreach (GameObject o in platformen)
			{
				o.SetActive(false);
			}  
			
			foreach(GameObject o in platformenOff)
			{
				o.SetActive(true);
			}
		}

		if (lampisaan && rotated) 
		{
			foreach (GameObject o in platformen)
			{
				o.SetActive(true);
			}  
			
			foreach(GameObject o in platformenOff)
			{
				o.SetActive(false);
			}
		}
	}

}
