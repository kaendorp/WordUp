using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {
    public GameObject enemyDeathEffect;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyProjectile"), LayerMask.NameToLayer("Default"));
    }

	void onTriggerEnter2D(Collision2D triggered)
	{		
		//If collides with player
		if (triggered.collider.gameObject.tag == "Player")
		{
			Instantiate(enemyDeathEffect, triggered.transform.position, triggered.transform.rotation);
            // Destroy is handled in player for better hit detection
		}
        else
        {
            //If it collides with anything, destroy projectile
            Destroy(this.gameObject);
        }
	}

	void OnCollisionEnter2D(Collision2D collided)
	{
		//If collides with player
        if (collided.collider.gameObject.tag == "Player")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            // Destroy is handled in player for better hit detection
        }
        else
        {
            //If it collides with anything, destroy projectile
            Destroy(this.gameObject);
        }
	}
}
