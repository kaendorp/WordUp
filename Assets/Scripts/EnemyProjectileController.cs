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
        Physics2D.IgnoreLayerCollision(12, 12);
    }



    void OnCollisionEnter2D(Collision2D collided)
    {
        Debug.Log("Collision Detected");
        
        //If collides with player, destroy player
        if (collided.gameObject.tag == "Player")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            Destroy(collided.gameObject);
        }
        //If it collides with anything, destroy projectile
        Destroy(gameObject);
    }
}
