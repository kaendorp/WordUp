using UnityEngine;
using System.Collections;

public class LetterProjectile3Controller : MonoBehaviour {

    public float speed;
    public PlatformerCharacter2D player;
	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;

	// Use this for initialization
	void Start () {
		// Debug.Log ("help");
		player = FindObjectOfType<PlatformerCharacter2D>();
        if (player.transform.localScale.x < 0)
        {
            speed = -1f;
        }
		if (player.transform.localScale.x > 0) {
            speed = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime, 6f * Time.deltaTime, 0);	
        if (this.gameObject != null)
		    Destroy (this.gameObject,5);
		// Destroy (this.impactEffect);
	}

	void onTriggerEnter2D(Collision2D obj)
    {
		Debug.Log ("Hit");
			Instantiate(enemyDeathEffect, obj.transform.position, obj.transform.rotation);
            Destroy(this.gameObject);		
    }

	void OnCollisionEnter2D(Collision2D collider)
	{ 
		Debug.Log("Collision Detected");
		
		if (collider.gameObject.tag == "Enemy") {
			Instantiate (enemyDeathEffect, collider.transform.position, collider.transform.rotation);
		}
		Instantiate (impactEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
	}
}
