using UnityEngine;
using System.Collections;

public class LetterHint : MonoBehaviour 
{
	private AudioClip _audioSource;
	private Vector3 positie;
	// Use this for initialization
	void Start () 
	{
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
			AudioSource.PlayClipAtPoint (_audioSource, positie, 0.1f);
		}
	}
}
