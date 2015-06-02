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
		// Debug.Log ("help");
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
        if (this.gameObject != null)
		    Destroy (this.gameObject,2);
	}

    void onTriggerEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Instantiate(enemyDeathEffect, collider.transform.position, collider.transform.rotation);
            EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
                enemyController.TakeDamage();
            else
                Debug.LogError(this.gameObject.name + ": Could not find EnemyController on Enemy target");
        }
        else if (collider.gameObject.tag == "Boss")
        {
            Instantiate(enemyDeathEffect, collider.transform.position, collider.transform.rotation);
            // The enemy and boss will destroy this object to ensure it detects the hit properly
        }
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

	void OnCollisionEnter2D(Collision2D collider)
	{
        if (collider.gameObject.tag == "Enemy")
        {
            Instantiate(enemyDeathEffect, collider.transform.position, collider.transform.rotation);
            EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
                enemyController.TakeDamage();
            else
                Debug.LogError(this.gameObject.name + ": Could not find EnemyController on Enemy target");
        }
        else if (collider.gameObject.tag == "Boss")
        {
            Instantiate(enemyDeathEffect, collider.transform.position, collider.transform.rotation);
            // The enemy and boss will destroy this object to ensure it detects the hit properly
        }
        else
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
	}
}
