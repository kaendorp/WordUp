using UnityEngine;
using System.Collections;

public class Sleutel : MonoBehaviour {

	public GameObject deur;
	private AudioClip _audioSource;
	private Vector3 positie;
	// Use this for initialization
	void Awake () 
	{
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	void Update () 
	{

	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			Destroy (gameObject);
			deur.SendMessage("GetKey");
		}

		//_audioSource.enabled = true;
	}
}
