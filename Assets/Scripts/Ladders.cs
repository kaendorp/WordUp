using UnityEngine;
using System.Collections;

public class Ladders : MonoBehaviour {

	GameObject player;
	PlatformerCharacter2D pfc;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		pfc = player.GetComponent<PlatformerCharacter2D>();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			Debug.Log ("Started Climbing");
			pfc.GetComponent<Rigidbody2D>().gravityScale = 1;
			pfc.isClimbing = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			Debug.Log ("Stopped Climbing");
			pfc.GetComponent<Rigidbody2D>().gravityScale = 3;
			pfc.isClimbing = false;
		}
	}
}
