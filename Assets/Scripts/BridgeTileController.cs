using UnityEngine;
using System.Collections;

public class BridgeTileController : MonoBehaviour {

	private AudioClip _audioSource;
	private Vector3 positie;
	private bool isPlayed;

	// Use this for initialization
	void Start () {
		positie = gameObject.transform.position;
		_audioSource = gameObject.GetComponent<AudioSource> ().clip;
		isPlayed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && !isPlayed) {
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			isPlayed = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			isPlayed = false;
		}
	}
}
