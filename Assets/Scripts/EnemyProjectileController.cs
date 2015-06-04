using UnityEngine;
using System.Collections;

public class EnemyProjectileController : MonoBehaviour {
    public GameObject enemyDeathEffect;
	private AudioClip projectileHits;
	private Vector3 positie;

	// Use this for initialization
    void Start()
    {
        // EnemyProjectile layer should ignore itself, not collide
        // TODO: Make sure it doesn't collide with player projectile
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyProjectile"), LayerMask.NameToLayer("EnemyProjectile"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("EnemyProjectile"));

		projectileHits = gameObject.GetComponent<AudioSource> ().clip;
		positie = gameObject.transform.position;
    }

    void onTriggerEnter2D(Collision2D collided)
    {
        //If collides with player
        if (collided.collider.gameObject.tag == "Player")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            Transform stats = collided.gameObject.transform.FindChild("Stats");
            if (stats != null)
            {
                Player playerComponent = stats.GetComponent<Player>();
                if (playerComponent != null)
                    playerComponent.TakeDamage();
            }
            else
            {
                Debug.LogError(this.gameObject.name + ": Could not find Stats Object");
            }
        }
        else if (collided.collider.gameObject.tag == "Friendly")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            FriendlyController friendlyController = collided.gameObject.GetComponent<FriendlyController>();
            if (friendlyController != null)
                friendlyController.TakeDamage();
            else
                Debug.LogError(this.gameObject.name + ": Could not find FriendlyController on Friendly target");
        }
        else
        {
			if (projectileHits)
                AudioSource.PlayClipAtPoint(projectileHits, positie, 0.5f);
        }
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collided)
    {
        //If collides with player
        if (collided.collider.gameObject.tag == "Player")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            Transform stats = collided.gameObject.transform.FindChild("Stats");
            if (stats != null)
            {
                Player playerComponent = stats.GetComponent<Player>();
                if (playerComponent != null)
                    playerComponent.TakeDamage();
            }
            else
            {
                Debug.LogError(this.gameObject.name + ": Could not find Stats Object");
            }
        }
        else if (collided.collider.gameObject.tag == "Friendly")
        {
            Instantiate(enemyDeathEffect, collided.transform.position, collided.transform.rotation);
            FriendlyController friendlyController = collided.gameObject.GetComponent<FriendlyController>();
            if (friendlyController != null)
                friendlyController.TakeDamage();
            else
                Debug.LogError(this.gameObject.name + ": Could not find FriendlyController on Friendly target");
        }
        else
        {
			if (projectileHits)
                AudioSource.PlayClipAtPoint(projectileHits, positie, 0.5f);
        }
        Destroy(this.gameObject);
    }
}
