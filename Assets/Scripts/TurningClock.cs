using UnityEngine;
using System.Collections;

public class TurningClock : MonoBehaviour {

	public GameObject littleHand;
	public GameObject bigHand;

	public GameObject klokLamp;
	public float speed;
	
	public int startingposition; //0, 1 of 2
	public int kloknummer;

	private Vector3 rotationLittleHand = Vector3.zero;
	private Vector3 rotationBigHand = Vector3.zero;

	private bool _triggered;
	private bool rotate;

	private object[] temp = new object[2];

	// Use this for initialization
	void Start () {
		_triggered = false;
		rotate = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (rotate == true) 
		{
			TurnClock ();
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
				case 0:
					rotationLittleHand.z = -60;
					rotationBigHand.z = 50;
					temp[0] = kloknummer;
					temp[1] = -60f;
					klokLamp.SendMessage("LampAan", temp);
					break;
				case 1:
					rotationLittleHand.z = -90;
					rotationBigHand.z = 30;
					temp[0] = kloknummer;
					temp[1] = -90f;
					klokLamp.SendMessage("LampAan", temp);
					break;
				case 2:
					rotationLittleHand.z = -70;
					rotationBigHand.z = 130;
					temp[0] = kloknummer;
					temp[1] = -70f;
					klokLamp.SendMessage("LampAan", temp);
					break;
				default:
					startingposition = 0;
					break;
			}
		}

	}
	void OnTriggerExit2D(Collider2D collision)
	{
		if (_triggered == true) 
		{
			//changes the Vector3.z's in ontrigger
			startingposition++;
			if(startingposition >= 3)
				startingposition = 0;
		}
		_triggered = false;
		//de klok zal niet draaien als je niet op het gewicht staat
		//de quaternation pakt de huidige rotation wel weer op, so no worries
		rotate = false;
	}

	void TurnClock()
	{
		//draaien [van - naar - snelheid] met Quaternions <3
		littleHand.transform.rotation = Quaternion.RotateTowards (littleHand.transform.rotation, Quaternion.Euler(0, 0, rotationLittleHand.z), Time.deltaTime * speed);
		bigHand.transform.rotation = Quaternion.RotateTowards (bigHand.transform.rotation, Quaternion.Euler(0, 0, rotationBigHand.z), Time.deltaTime * speed);
	}
}
