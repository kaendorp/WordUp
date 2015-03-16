using UnityEngine;
using System.Collections;


/*
 * List of selectable enemy types 
 */
public enum EnemyType
{
    stationary, patrol
}
public class EnemyController : MonoBehaviour {
    
    public EnemyType type;

    // Target, usually the player
    // TODO: Make this a list, for players and friendly NPC's
    public string targetLayer = "Player";
    public GameObject target;
    // Firing Projectiles
    public Transform firePoint;             // Point from which the enemy fires
    public GameObject projectilePrefab;     // Projectile
    public float projectileSpeed = 5;       // Speed of the projectile
    public float projectileLifeTime = 2;    // How long the projectile exists before selfdestructing
    public float fireDelay = 3;             // Time between shots
    
    // Spot
    public float spotRadius = 3;            // Radius in which a player can be spotted
    public bool drawSpotRadiusGismo = true; // Visual aid in determening if the spot radius
    private Collider2D[] collisionObjects;
    public bool playerSpotted = false;      // Debug purposes, to see in the editor if an enemy spotted the player
    
    // Shoot
    private GameObject projectile;          // Selected projectile, should handle selfdestruct and damage
    private float timeToFire = 0;           // Future date to trigger shot
    public bool playerIsLeft;               // Simple check to see if the player is left to the enemy, important for facing.
    private bool facingLeft = true;         // For determining which way the player is currently facing.

    // Patrol
    public float walkSpeed = 1f;            // Amount of velocity
    private bool walkingRight;              // Simple check to see in what direction the enemy is moving, important for facing.

    public float collideDistance = 0.6f;    // Distance from enemy to check for a wall.
    private bool colliding = false;         // If true, it touched a wall and should flip.
    public bool stopPatrolIfSpotted = false;// Stops patrol if player is spotted

	// Use this for initialization
    void Start()
    {
    }

    void FixedUpdate()
    {
        // Will set 'playerSpotted' to true if spotted
        IsPlayerInRange();
        // Stops the patrol and starts shooting at player
        if (playerSpotted)
        {
            stopPatrolIfSpotted = true;
            FacePlayer();

            // Delay before shooting again
            if (Time.time > timeToFire)
            {
                timeToFire = Time.time + fireDelay;
                Shoot();
            }
        }
        else
        {
            stopPatrolIfSpotted = false;
        }

        if (type == EnemyType.patrol)
        {
            // Stop moving if the player is spotted
            if (!stopPatrolIfSpotted)
            {
                Patrol();
                FaceDirectionOfWalking();
            }
        }
    }

    /*
     * Patrol script for enemy, 
     * will walk untill the linecast hits a collider, then walk the other way
     */
    private void Patrol()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, GetComponent<Rigidbody2D>().velocity.y);
        
        colliding = Physics2D.Linecast(
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 4))),
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2)))
            );

        if (colliding)
        {
            Debug.Log(this.name + " hit a wall, now walking the other way.");
            walkSpeed *= -1;
            collideDistance *= -1;
        }
    }

    /*
     * Checks to see if an entity of the "Player" layer has entered the range of the enemy.
     * 
     * Gets a list colliders that collided with the overlapcircle and uses the first result to 
     * become the target of the enemy. This is so that you don't have to manually add the target to every enemy
     * and will help when multiplayer is implemented
     */
    private void IsPlayerInRange()
    {
        collisionObjects = Physics2D.OverlapCircleAll(this.transform.position, spotRadius, 1 << LayerMask.NameToLayer(targetLayer));

        if (collisionObjects.Length > 0)
        {
            target = collisionObjects[0].gameObject;
            playerSpotted = true;
        }
        else
        {
            playerSpotted = false;
        }
    }

    /*
     * Script to make the enemy face the player
     */
    private void FacePlayer()
    {
        //Player could be destroyed
        if (target != null)
        {
            playerIsLeft = target.transform.position.x < this.transform.position.x;

            if (!playerIsLeft && facingLeft)
            {
                Flip();
            }
            else if (playerIsLeft && !facingLeft)
            {
                Flip();
            }
        }
    }

    /*
     * This method makes sure the enemy will be facing the direction it is going in
     */
    private void FaceDirectionOfWalking()
    {
        if (GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            walkingRight = true;
        }
        else
        {
            walkingRight = false;
        }
        if (walkingRight && facingLeft)
        {
            Flip();
        }
        else if (!walkingRight && !facingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingLeft = !facingLeft;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        //Changes the speed to negative, making it fire the other way
        projectileSpeed = -projectileSpeed;
    }

    /*
     * Shoots a projectile in the direction the enemy is facing.
     * 
     * Auto destructs after lifetime has ended. 
     * Projectile should have a script attached to destruct it on collision.
     */
    private void Shoot()
    {
        projectile = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2((projectileSpeed * -1), GetComponent<Rigidbody2D>().velocity.y);
        Destroy(projectile, projectileLifeTime);
    }

    /*
     * Draws a circle gizmo to show the field of view or 'agro' range of an enemy
     */
    private void OnDrawGizmos()
    {
        if (drawSpotRadiusGismo)
        {
            Gizmos.color = Color.red;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(this.transform.position, spotRadius);
        }
        // Draws the collision for the patrol enemies
        if (type == EnemyType.patrol)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 4))),
                new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2)))
                );
        }
    }
}
