using UnityEngine;
using System.Collections;

public class VineWallController : MonoBehaviour {

	private AudioSource _audioSource;

	// Use this for initialization
	void Start ()
	{
		_audioSource = gameObject.GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			_audioSource.enabled = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			_audioSource.enabled = false;
		}
	}
}
