using UnityEngine;
using System.Collections;

public class BossProjectile : MonoBehaviour {
    public GameObject enemyDeathEffect;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyProjectile"), LayerMask.NameToLayer("Default"));
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
        Destroy(this.gameObject);
	}
}
