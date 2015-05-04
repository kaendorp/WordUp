using UnityEngine;
using System.Collections;

public class Stoel : MonoBehaviour 
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
		if (collision.gameObject.tag == "Stoel") 
		{
			AudioSource.PlayClipAtPoint (_audioSource, positie);
		}
	}
}
