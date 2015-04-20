using UnityEngine;
using System.Collections;

public class Letter2ProjectileController : MonoBehaviour {

	public float speed = 2.0f;
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

		player = FindObjectOfType<PlatformerCharacter2D>();
		if (player.transform.localScale.x < 0)
		{
			speed = -speed;
		}
		if (player.transform.localScale.x > 0) {
            speed = 2.0f;

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (bounceUp == true) {
			transform.Translate(speed * Time.deltaTime, 3f * Time.deltaTime, 0);	
			heightDifference = transform.position.y - hitPosition;
			if(bounceHeight <= heightDifference) {
				bounceUp = false;
			}
		} else {
			transform.Translate(speed * Time.deltaTime, -3f * Time.deltaTime, 0);	
		}
	}



	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Untagged") {

            RaycastHit2D hit = new RaycastHit2D();
            if (Physics2D.Raycast(transform.position, new Vector2(1, 0), hit.distance - 0.5f, 1) || Physics2D.Raycast(transform.position, new Vector2(-1, 0), hit.distance - 0.5f, 1))
			{
				Destroy(gameObject);
			}
			else
            {
				bounceUp = true;
				hitPosition = transform.position.y;
			}
		}

	}
    void DestroyProjectile()
    {
        Destroy(gameObject, 5);
    }
	

}
