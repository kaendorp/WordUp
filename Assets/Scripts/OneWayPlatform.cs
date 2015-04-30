using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {

	PlatformerCharacter2D character;
	
	void OnTriggerEnter2D(Collider2D player)
	{
		transform.parent.GetComponent<Collider2D>().isTrigger = false;

	}

	void OnTriggerStay2D(Collider2D player)
	{
		if (Input.GetKey(KeyCode.S)) {
			transform.parent.GetComponent<Collider2D>().isTrigger = true;
		}
	}

	void OnTriggerExit2D(Collider2D player)
	{
		transform.parent.GetComponent<Collider2D>().isTrigger = true;
	}
	
	// Use this for initialization
	void Start () {
		transform.parent.GetComponent<Collider2D>().isTrigger = true;
	}
	void fixedUpdate() {
	}
}
