using UnityEngine;
using System.Collections;

public class Sleutel : MonoBehaviour {

	public GameObject deur;

	private bool _triggered;
	//private AudioSource _audioSource;
	// Use this for initialization
	void Awake () 
	{
		//_audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	void Update () 
	{
		if (_triggered ) //&& !_audioSource.isPlaying
		{
			Destroy (gameObject);
			deur.SendMessage("GetKey");

		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			_triggered = true;
		}

		//_audioSource.enabled = true;
	}
}
