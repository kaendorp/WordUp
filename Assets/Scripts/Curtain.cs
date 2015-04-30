using UnityEngine;
using System.Collections;

public class Curtain : MonoBehaviour {

	public GameObject gordijn;
	public GameObject container;
	public Animator anim;

	private AudioClip _audioSource;
	private Vector3 positie;

	private bool triggered;
	// Use this for initialization
	void Start () {
		triggered = false;
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
			gordijn.GetComponent<Animator>().enabled = true;
			if(!triggered)
			{
				container.transform.Translate (0, -Time.deltaTime * 10, 0);
				AudioSource.PlayClipAtPoint (_audioSource, positie);
				triggered = true;
			}
		}
	}

}
