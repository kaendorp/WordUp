using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {

	public int damage = 1;
	public bool isEnemyShot = false;
	public GameObject enemyDeathEffect;
	// Use this for initialization
	private void Start () {
		Destroy (gameObject, 3);
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

	void onTriggerEnter2D(Collision2D triggered)
	{
		Debug.Log("Enemy projectile: HIT");
		
		//If collides with player
		if (triggered.gameObject.tag == "Player")
		{
			Instantiate(enemyDeathEffect, triggered.transform.position, triggered.transform.rotation);
		}
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D collided)
	{
		Debug.Log("Enemy projectile: HIT");
		
		//If collides with player
		if (collided.gameObject.tag == "Player")
		{
			Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
		}
		//If it collides with anything, destroy projectile
		Destroy(gameObject);
	}
}
