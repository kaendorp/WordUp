using UnityEngine;
using System.Collections;

public class OneWayPlatformScript : MonoBehaviour {

	
	PlatformerCharacter2D plyr;
	
	void OnTriggerEnter2D(Collider2D player)
	{
		if(player.gameObject.tag == "Player")
		transform.parent.GetComponent<Collider2D>().isTrigger = false;
		
	}
	
	void OnTriggerStay2D(Collider2D player)
	{
	}
	
	void OnTriggerExit2D(Collider2D player)
	{
		if(player.gameObject.tag == "Player")
		transform.parent.GetComponent<Collider2D>().isTrigger = true;
	}
	
	// Use this for initialization
	void Start () {
		transform.parent.GetComponent<Collider2D>().isTrigger = true;
	}
	void fixedUpdate() {
	}
}
