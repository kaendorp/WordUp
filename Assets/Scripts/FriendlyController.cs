using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * List of selectable Friendly types
 */
public enum FriendlyType
{
    stationary,
    patrol
}

/*
 * List of posible states an Friendly can be in
 */
public enum FriendlyState
{
    idle,
    wait
}

public class FriendlyController : MonoBehaviour
{
    public FriendlyType type;
    private FriendlyState _state = FriendlyState.idle;      // Local variable to represent our state
    private Animator anim;

    // Spawn enemy
    public GameObject enemyPatrol;
    public GameObject enemyStationary;
    private GameObject spawn;

    // Message
    public string message = "";                 // The message the fiendly will display when the player gets close
    public GameObject messageObject;            // TextMesh object that will display our message

    // Health
    public float currentHealth = 5f;
    public float invincibilityDuration = 2f;    // length of damage cooldown
    private bool onCoolDown = false;            // Cooldown active or not

    // Patrol
    public float walkSpeed = 1f;                // Amount of velocity
    private bool walkingRight;                  // Simple check to see in what direction the enemy is moving, important for facing.
    public float collideDistance = 0.5f;        // Distance from enemy to check for a wall.
    public bool edgeDetection = true;           // If checked, it will try to detect the edge of a platform
    private bool collidingWithWall = false;     // If true, it touched a wall and should flip.
    private bool collidingWithGround = true;    // If true, it is not about to fall off an edge

    // Target (usually the player)
    public string targetLayer = "Player";       // TODO: Make this a list, for players and friendly NPC's
    private GameObject target;

    // Stop patroling delay
    private bool delayCoroutineStarted = false;
    private bool isBlinded = false;
    public float blindedDelay = 2f;

    // Shoot
    private bool playerIsLeft;                  // Simple check to see if the player is left to the friendly, important for facing.
    private bool facingLeft = true;             // For determining which way the player is currently facing.

    // Spot
    public float spotRadius = 3;                // Radius in which a player can be spotted
    public bool drawSpotRadiusGismo = true;     // Visual aid in determening if the spot radius
    private Collider2D[] collisionObjects;
    private bool playerSpotted = false;         // Has the friendly spotted the player?

    void Start()
    {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");

        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case FriendlyState.idle:
                if (type == FriendlyType.stationary)
                {
                    Idle();
                }
                else if (type == FriendlyType.patrol)
                {
                    Patrol();
                }
                break;
            case FriendlyState.wait:
                Wait();
                break;
        }

        if (currentHealth <= 0)
        {
            FriendlyDeath();
        }
    }

    /**
     * Idle state
     *
     * In this state, the Friendly will wait to spot a player, and then it will go to its attack state.
     * Patroling Friendlys will resume to patrol after the player is out of reach
     */
    private void Idle()
    {
        // Will set 'playerSpotted' to true if spotted
        IsPlayerInRange();
        if (playerSpotted)
        {
            _state = FriendlyState.wait;
        }
    }

    /**
     * Patrol script for Friendly,
     * will walk untill the collidingWithWall linecast hits a collider, then walk the other way
     * or (if checked) will detect if the Friendly is to hit the edge of a platform
     */
    private void Patrol()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(walkSpeed, GetComponent<Rigidbody2D>().velocity.y);

        FaceDirectionOfWalking();

        collidingWithWall = Physics2D.Linecast(
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 4))),
            new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y / 2))),
            ~(
                (1 << this.gameObject.layer) +
                (1 << LayerMask.NameToLayer("UI")) +
                (1 << LayerMask.NameToLayer("EnemeyProjectile")) +
                (1 << LayerMask.NameToLayer("PlayerProjectile"))
            ) // Collide with all layers, except itself
        );

        if (edgeDetection)
        {
            collidingWithGround = Physics2D.Linecast(
                new Vector2(this.transform.position.x, this.transform.position.y),
                new Vector2((this.transform.position.x + collideDistance), (this.transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y))),
                ~(
                    (1 << this.gameObject.layer) +
                    (1 << LayerMask.NameToLayer("UI")) +
                    (1 << LayerMask.NameToLayer("EnemeyProjectile")) +
                    (1 << LayerMask.NameToLayer("PlayerProjectile"))
                ) // Collide with all layers, except itself
            );
        }
        else
        {
            collidingWithGround = true;
        }

        if (collidingWithWall || !collidingWithGround)
        {
            // Debug.Log(this.name + " hit a wall, now walking the other way.");
            walkSpeed *= -1;
            collideDistance *= -1;
        }

        if (!isBlinded)
        {
            // Will set 'playerSpotted' to true if spotted
            IsPlayerInRange();
            if (playerSpotted)
            {
                _state = FriendlyState.wait;
            }
        }
    }

    /**
     * Friendly will stop to face the player.
     *
     * StopPatrolDelay is to prevent the friendly patrol to stop moving
     * every time the player enters its spotrange.
     * This is because in some occations it would cause the friendly patrol
     * to 'creep up' to players. Hopefully it looks a bit more natural now.
     */
    private void Wait()
    {
        if (type == FriendlyType.patrol)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        FacePlayer();

        // Will set 'playerSpotted' to true if spotted
        IsPlayerInRange();
        if (!playerSpotted)
        {
            _state = FriendlyState.idle;
            if (type == FriendlyType.patrol)
            {
                if (!delayCoroutineStarted)
                    StartCoroutine(StopPatrolDelay());
            }
        }
    }

    /**
     * Sets the amount of time the patrol won't check if the player is nearby.
     */
    IEnumerator StopPatrolDelay()
    {
        delayCoroutineStarted = true;
        isBlinded = true;
        yield return new WaitForSeconds(blindedDelay);
        isBlinded = false;
        delayCoroutineStarted = false;

    }

    /**
     * Take damage when hit with an enemy projectile. When this entity gets hit
     * it will get a period in which it can not be hurt ('onCoolDown'), granting
     * it invincibility for a short period of time.
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            if (!onCoolDown && currentHealth > 0)
            {
                StartCoroutine(coolDownDMG());
                Debug.Log(this.gameObject.name + ": Au!");
                currentHealth -= 1;
            }
        }
    }

    /**
     * Sets the delay when this entity can get hurt again.
     */
    IEnumerator coolDownDMG()
    {
        onCoolDown = true;
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(invincibilityDuration);
        onCoolDown = false;
        anim.SetBool("isHit", false);
    }

    /**
     * Friendly death
     *
     * When an friendly dies, it will be replaced with an enemy of the same type.
     */
    private void FriendlyDeath()
    {
        Debug.Log(this.gameObject.name + ": 'Oh nee, ik ben slecht geworden!'");
        if (type == FriendlyType.patrol)
        {
            spawn = Instantiate(enemyPatrol, this.transform.position, this.transform.rotation) as GameObject;
        }
        else if (type == FriendlyType.stationary)
        {
            spawn = Instantiate(enemyStationary, this.transform.position, this.transform.rotation) as GameObject;
        }

        spawn.SendMessage("GetMessage", message);

        Destroy(this.gameObject);
    }

    /**
     * This method makes sure the friendly will be facing the direction it is going in
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

    /**
     * Checks to see if an entity of the "Player" layer has entered the range of the Friendly.
     *
     * Gets a list colliders that collided with the overlapcircle and uses the first result to
     * become the target of the Friendly. This is so that you don't have to manually add the target to every Friendly
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

    /**
     * Script to make the friendly face the player
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

    /**
     * Flips the sprite the other way around so it will face left/right.
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

        // Flips message as well
        Vector3 messageScale = messageObject.transform.localScale;
        messageScale.x *= -1;
        messageObject.transform.localScale = messageScale;
    }

    /**
     * If the enemy had a message on it, it will send it to this method
     */
    public void GetMessage(string messageGet)
    {
        message = messageGet;
    }

    /**
     * If the player is close to the friendly, it will display a message
     */
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = message;
        }
    }

    /**
     * If the player moves away from the friendly, it will clear the message
     */
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = "";
        }
    }

    /**
     * Draws a circle gizmo to show the field of view or 'agro' range of an friendly
     */
    private void OnDrawGizmos()
    {
        if (drawSpotRadiusGismo)
        {
            Gizmos.color = Color.green;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(this.transform.position, spotRadius);
        }

        // Draws the collision for the patrol friendlies
        if (type == FriendlyType.patrol)
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
