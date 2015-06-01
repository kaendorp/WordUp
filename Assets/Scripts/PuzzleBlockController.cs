using UnityEngine;
using System.Collections;

public class PuzzleBlockController : MonoBehaviour {
	private Rigidbody2D rb;
	private AudioSource _audioSource;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		rb = GetComponent<Rigidbody2D> ();
		_audioSource = gameObject.GetComponent<AudioSource> ();
		_audioSource.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 0.05f) {
			_audioSource.enabled = true;
		} else {
			_audioSource.enabled = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Bridge" || collider.tag == "Water")
		gameObject.transform.position = startPosition;
	}
}


