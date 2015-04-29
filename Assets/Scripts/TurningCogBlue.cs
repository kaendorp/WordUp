using UnityEngine;
using System.Collections;

public class TurningCogBlue : MonoBehaviour {
	private bool rotated;
	private Vector3 rotation = Vector3.zero;
	private bool lampisaan;
	private int modulo;

	public float angle;

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
		//dit kan vast beter maar dat is voor de debug weken -_-" 
		//deze blijft nu natuurlijk checken ad infinitum
		if (angle < 0) {
			if (rotated == true && rotation.z > angle) {
				Rotate ();
			}
		} else {
			if (rotated == true && rotation.z < angle) {
				Rotate ();
			}
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
			//play sound
			rotated = true;
			AudioSource.PlayClipAtPoint (_audioSource, positie, 0.1f);
		}
	}

	void Rotate()
	{
		if (angle < 0) {
			rotation.z += -Time.deltaTime * 20;
			gameObject.transform.Rotate (0, 0, -Time.deltaTime * 20);
		} else {
			rotation.z += Time.deltaTime * 20;
			gameObject.transform.Rotate (0, 0, Time.deltaTime * 20);
		}

		//to do foreach
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
