using UnityEngine;
using System.Collections;

/*
 * List of selectable enemy types 
 */
public enum EnemyType
{
    stationary, patrol
}

/*
 * List of posible states an enemy can be in
 */
public enum EnemyState
{
    idle, attack
}

public class EnemyController : MonoBehaviour {
    public EnemyType type;
    private EnemyState _state = EnemyState.idle; // Local variable to represent our state
    private Animator anim;
    
    // Spawn friendly
    public GameObject friendlyPatrol;
    public GameObject friendlyStationary;
    private GameObject spawn;

    // Message
    public string message = "";             // The message the friendly will use after this enemy is defeated

    // Health
    public float currentHealth = 2f;
    public float coolDown = 2f;             // length of damage cooldown
    private bool onCoolDown = false;        // Cooldown active or not
    
    // Target (usually the player)
    public string targetLayer = "Player";   // TODO: Make this a list, for players and friendly NPC's
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
    public float collideDistance = 0.5f;    // Distance from enemy to check for a wall.
    public bool edgeDetection = true;       // If checked, it will try to detect the edge of a platform
    private bool collidingWithWall = false; // If true, it touched a wall and should flip.
    private bool collidingWithGround = true;// If true, it is not about to fall off an edge

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.attack:
                Attack();
                break;
        }

        if (currentHealth <= 0)
        {
            EnemyDeath();
        }
    }

    /*
     * Take damage when hit with the players projectile. When this entity gets hit
     * it will get a period in which it can not be hurt ('onCoolDown'), granting
     * it invincibility for a short period of time.
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile") 
        {
            if (!onCoolDown && currentHealth > 0)
            {
                StartCoroutine(coolDownDMG());
                Debug.Log(this.gameObject.name + ": Au!");
                currentHealth -= 1;
                anim.SetTrigger("isHit");
            }
        }
    }

    /*
     * Sets the delay when this entity can get hurt again.
     */
    IEnumerator coolDownDMG()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDown);
        onCoolDown = false;
    }

    /*
     * Enemy death
     * 
     * When an enemy dies, it will be replaced with a friendly of the same type.
     */
    void EnemyDeath()
    {
        Debug.Log(this.gameObject.name + ": 'Yay! Ik ben nu vriendelijk!'");
        if (type == EnemyType.patrol)
        {
            spawn = Instantiate(friendlyPatrol, this.transform.position, this.transform.rotation) as GameObject;
        }
        else if (type == EnemyType.stationary)
        {
            spawn = Instantiate(friendlyStationary, this.transform.position, this.transform.rotation) as GameObject;
        }
        spawn.SendMessage("GetMessage", message);
        Destroy(this.gameObject);
    }

    /*
     * Idle state
     * 
     * In this state, the enemy will wait to spot a player, and then it will go to its attack state.
     * Patroling enemys will resume to patrol after it shot at the player, as the attack state
     * will reset the timer. The first time the patroling enemy spots an enemy, the timer will
     * already have passed and it will immediately go into the attack state.
     */
    private void Idle()
    {
        // Sends the patroling enemy to patrol
        if (type == EnemyType.patrol)
        {
            Patrol();
        }

        // Will set 'playerSpotted' to true if spotted
        IsTargetInRange();
        if (playerSpotted)
        {
            if (type == EnemyType.stationary)
            {
                timeToFire = Time.time + fireDelay;
                _state = EnemyState.attack;
                //Debug.Log("Switch to Attack!");
            }
            else if (type == EnemyType.patrol)
            {
                // This delay is so that the enemy will resume patrol after shooting at the player
                if (Time.time > timeToFire)
                {
                    timeToFire = Time.time + (fireDelay/2);
                    _state = EnemyState.attack;
                    //Debug.Log("Switch to Attack!");
                }
            }
        }
    }

    /*
     * Patrol script for enemy, 
     * will walk untill the collidingWithWall linecast hits a collider, then walk the other way
     * or (if checked) will detect if the enemy is to hit the edge of a platform
     */
    private void Patrol()
    {
		anim.SetFloat ("speed", walkSpeed);
        GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, GetComponent<Rigidbody2D>().velocity.y);

        FaceDirectionOfWalking();

        collidingWithWall = Physics2D.Linecast(
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 4))),
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2))),
            ~(
                
                (1 << LayerMask.NameToLayer(targetLayer)) + 
                (1 << LayerMask.NameToLayer("EnemyProjectile")) +
                (1 << LayerMask.NameToLayer("PlayerProjectile"))
            ) // Collide with all layers, except the targetlayer and the projectiles
        );

        if (edgeDetection)
        {
            collidingWithGround = Physics2D.Linecast(
                new Vector2(this.transform.position.x, this.transform.position.y),
                new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y))),
                ~(
                    (1 << this.gameObject.layer) +
                    (1 << LayerMask.NameToLayer("EnemyProjectile")) +
                    (1 << LayerMask.NameToLayer("PlayerProjectile"))
                ) // Collide with all layers, except the targetlayer and the projectiles
            );
        }
        else
        {
            collidingWithGround = true;
        }

        if (collidingWithWall || !collidingWithGround)
        {
            //Debug.Log(this.name + " hit a wall, now walking the other way.");
            walkSpeed *= -1;
            collideDistance *= -1;
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

    /*
     * Checks to see if an entity of the "Player" layer has entered the range of the enemy.
     * 
     * Gets a list colliders that collided with the overlapcircle and uses the first result to 
     * become the target of the enemy. This is so that you don't have to manually add the target to every enemy
     * and will help when multiplayer is implemented
     */
    private void IsTargetInRange()
    {
        collisionObjects = Physics2D.OverlapCircleAll(
            this.transform.position, 
            spotRadius, 
            ( 
                (1 << LayerMask.NameToLayer(targetLayer)) + 
                (1 << LayerMask.NameToLayer("Friendly")) 
            ) 
        );

        if (collisionObjects.Length > 0)
        {
            target = collisionObjects[0].gameObject;

            // If there are multiple targets, prioritise the player
            if (collisionObjects.Length > 1)
            {
                foreach (Collider2D spottedObject in collisionObjects)
                {
                    if (spottedObject.gameObject.layer == LayerMask.NameToLayer(targetLayer))
                    {
                        target = spottedObject.gameObject;
                    }
                }
            }

            playerSpotted = true;
        }
        else
        {
            playerSpotted = false;
        }
    }

    /*
     * Attack!
     * 
     * The enemy spots its target and will attack it.
     * First it will face the player, wait for the delay to run out and then shoot
     * before going back to the idle state. 
     * */
    private void Attack()
    {
        // Patroling enemy needs to stop moving before shooting.
        // This enemy will resume patrol in the idle state
        if (type == EnemyType.patrol)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        FacePlayer();

        if (Time.time > timeToFire)
        {
            //Debug.Log("Shooting!");
            Shoot();
            timeToFire = Time.time + fireDelay;
            _state = EnemyState.idle;
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
     * Flips the sprite of the enemy the other way around so it will face left/right.
     * 
     * Used by both FacePlayer() and FaceDirectionOfWalking().
     */
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
     * Should also trigger the attack animation.
     */
    private void Shoot()
    {
        if (anim != null)
        {
            anim.SetTrigger("attacktrigger");
        }

        projectile = (GameObject)Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2((projectileSpeed * -1), GetComponent<Rigidbody2D>().velocity.y);
        if (!facingLeft)
        {
            projectile.transform.localScale *= -1;
        }
        Destroy(projectile, projectileLifeTime);
    }

    /*
     * Get the message of the friendly after it has been defeated.
     * 
     * This is to save the message between transformations.
     */

    void GetMessage(string messageGet)
    {
        message = messageGet;
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

            if (edgeDetection)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(
                    new Vector2(this.transform.position.x, this.transform.position.y),
                    new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y)))
                    );
            }
        }
    }
}
