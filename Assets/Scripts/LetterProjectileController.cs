using UnityEngine;
using System.Collections;

public class LetterProjectileController : MonoBehaviour {

    public float speed;

    public PlatformerCharacter2D player;
	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;


	// Use this for initialization
	void Start () {
		Debug.Log ("help");
		player = FindObjectOfType<PlatformerCharacter2D>();
        if (player.transform.localScale.x < 0)
        {
            speed = -speed;
        }
		if (player.transform.localScale.x > 0) {
			transform.forward = -transform.forward;
		}
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, GetComponent<Rigidbody2D>().velocity.y);
		Destroy (gameObject,2);
		Destroy (impactEffect);
	}

	void onTriggerEnter2D(Collision2D obj)
    {
		Debug.Log ("Hit");

		if (obj.gameObject.tag == "Enemy") {
			DestroyObject(obj.gameObject);		
		}
			Instantiate(enemyDeathEffect, obj.transform.position, obj.transform.rotation);
			Destroy(gameObject);		
    }

	void OnCollisionEnter2D(Collision2D collider)
	{ 
		Debug.Log("Collision Detected");


		if (collider.gameObject.tag == "Enemy") {
			Instantiate (enemyDeathEffect, collider.transform.position, collider.transform.rotation);
						Destroy (collider.gameObject);
			Destroy(gameObject);
				}
		Instantiate (impactEffect, transform.position, transform.rotation);
		Destroy(gameObject);
		/*
         if(collider.gameObject.name == "CannonBall")
         {
             megonLife -= 1;
             Instantiate(bossExplosion, transform.position, transform.rotation); // Instantiate Explosion Effect
         }
         */
	}
}