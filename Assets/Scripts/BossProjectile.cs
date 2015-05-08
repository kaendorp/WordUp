using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {
    public GameObject enemyDeathEffect;

	void onTriggerEnter2D(Collision2D triggered)
	{
		//Debug.Log("Enemy projectile: HIT");
		
		//If collides with player
		if (triggered.gameObject.tag == "Player")
		{
			Instantiate(enemyDeathEffect, triggered.transform.position, triggered.transform.rotation);
		}
        if (triggered.gameObject.layer.ToString() != "Default")
		    Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D collided)
	{
		//Debug.Log("Enemy projectile: HIT");
		
		//If collides with player
		if (collided.gameObject.tag == "Player")
		{
			Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
		}
		//If it collides with anything, destroy projectile
        if (collided.gameObject.layer.ToString() != "Default")
            Destroy(gameObject);
	}
}
