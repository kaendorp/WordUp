using UnityEngine;
using System.Collections;

public class BouncyBed : MonoBehaviour {

	GameObject bouncer;
	public Vector2 velocity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			bouncer = collision.gameObject;
			bouncer.GetComponent<Rigidbody2D>().velocity = velocity;
		}
	}
}
