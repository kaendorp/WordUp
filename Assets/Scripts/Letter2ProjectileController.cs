using UnityEngine;
using System.Collections;

public class Letter2ProjectileController : MonoBehaviour {

	public float speed = 1.0f;
	public PlatformerCharacter2D player;
	public bool bounceUp = false;
	public float bounceHeight = .25f;
	public float heightDifference = 0.0f;
	public float hitPosition = 0.0f;

	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (bounceUp == true) {
			transform.Translate(speed * Time.deltaTime, .75f * Time.deltaTime, 0);	
			heightDifference = transform.position.y - hitPosition;
			if(bounceHeight <= heightDifference) {
				bounceUp = false;
			}
		} else {
			transform.Translate(speed * Time.deltaTime, -1.0f * Time.deltaTime, 0);	
		}
	}



	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Untagged") {

			Debug.Log ("Hit");
            RaycastHit2D hit = new RaycastHit2D();


            if (Physics2D.Raycast(transform.position, Vector2.right , 0.1f) || Physics2D.Raycast(transform.position, Vector2.right, 0.1f))
			{
				Destroy(gameObject);
			}
			else{
				bounceUp = true;
				hitPosition = transform.position.y;
			}
		}

	}
	

}
