using UnityEngine;
using System.Collections;

public class Sleutel : MonoBehaviour {

	public GameObject deur;
	//private AudioSource _audioSource;
	// Use this for initialization
	void Awake () 
	{
		//_audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	void Update () 
	{

	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			Destroy (gameObject);
			deur.SendMessage("GetKey");
		}

		//_audioSource.enabled = true;
	}
}
