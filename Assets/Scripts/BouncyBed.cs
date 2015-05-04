using UnityEngine;
using System.Collections;

public class BouncyBed : MonoBehaviour {

	public Vector2 velocity;

	private AudioClip _audioSource;
	private Vector3 positie;
	private bool playSound;

	private GameObject player;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
		if (player == null) {
			player = GameObject.Find ("Player2");
		}
		playSound = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			playSound = true;
			player.GetComponent<Rigidbody2D>().velocity = velocity;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if(playSound)
		{
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			playSound = false;
		}
	}
}
