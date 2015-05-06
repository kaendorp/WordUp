using UnityEngine;
using System.Collections;

public class PuzzleBlockController : MonoBehaviour {
	private Rigidbody2D rb;
	private AudioSource _audioSource;
	// Use this for initialization
	void Start () {
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
}


