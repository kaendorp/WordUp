using UnityEngine;
using System.Collections;

public class RotateCog : MonoBehaviour {

	public GameObject blueCog; // - 35
	public GameObject greenCog; // +35
	public GameObject yellowCog; //z = -35 
	public GameObject uselessCog; // +35

	public GameObject lever;

	public int speed;

	private int teller;
	private int startingposition; //1 of 2
	private bool _triggered;
	private bool rotate;

	private Vector3 rotationBack = Vector3.zero;
	private Vector3 rotationForth = Vector3.zero;

	private AudioClip _audioSource;
	private Vector3 positie;

	// Use this for initialization
	void Start () {
		_triggered = false;
		rotate = false;
		startingposition = 1;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (rotate == true) 
		{
			Rotate ();
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			_triggered = true;
			rotate = true;
			switch(startingposition)
			{
				case 1:
					rotationBack.z = 35f;
					rotationForth.z = 0f;
					lever.transform.Rotate(0, 0, 20);
					break;
				case 2:
					rotationBack.z = 0f;
					rotationForth.z = 35f;
					lever.transform.Rotate(0, 0, -20);
					break;
			}
			if(_triggered == true && teller == 0)
			{
				AudioSource.PlayClipAtPoint (_audioSource, positie);
				teller++;
			}
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (_triggered == true) 
		{
			startingposition++;
			if(startingposition > 2)
				startingposition = 1;
		}
		_triggered = false;
		teller = 0;
	}

	void Rotate()
	{
		yellowCog.transform.rotation = Quaternion.RotateTowards (yellowCog.transform.rotation, Quaternion.Euler(0, 0, rotationBack.z), Time.deltaTime * speed);
		blueCog.transform.rotation = Quaternion.RotateTowards (blueCog.transform.rotation, Quaternion.Euler(0, 0, rotationForth.z), Time.deltaTime * speed);
		greenCog.transform.rotation = Quaternion.RotateTowards (greenCog.transform.rotation, Quaternion.Euler(0, 0, rotationBack.z), Time.deltaTime * speed);
		uselessCog.transform.rotation = Quaternion.RotateTowards (uselessCog.transform.rotation, Quaternion.Euler(0, 0, rotationForth.z), Time.deltaTime * speed);
	}

}
