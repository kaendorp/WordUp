using UnityEngine;
using System.Collections;

public class EnemyProjectileController : MonoBehaviour {

    public GameObject enemyDeathEffect;
    public GameObject impactEffect;

    // Use this for initialization
    void Start()
    {
        // EnemyProjectile layer should ignore itself, not collide
        // TODO: Make sure it doesn't collide with player projectile
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyProjectile"), LayerMask.NameToLayer("EnemyProjectile"));
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
