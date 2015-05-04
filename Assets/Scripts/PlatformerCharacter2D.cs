using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class PlatformerCharacter2D : MonoBehaviour
{
    //<VARIABLES>
    [SerializeField]
    private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
    [SerializeField]
    public float jumpForce = 400f; // Amount of force added when the player jumps.	
    [Range(0, 1)]
    [SerializeField]
    private float crouchSpeed = .36f; // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField]
    private bool airControl = false; // Whether or not a player can steer while jumping;
    [SerializeField]
    private LayerMask whatIsGround; // A mask determining what is ground to the character
    [SerializeField]
    private LayerMask whatIsClimbable;// A mask determining what is a ladder to the character

    //Enviroment Check
    private Transform groundCheck; // fA position marking where to check if the player is grounded.
    private float groundedRadius = .17f; // Radius of the overlap circle to determine if grounded
    private bool grounded = false; // Whether or not the player is grounded.
    private Transform ceilingCheck; // A position marking where to check for ceilings
    private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    public Animator anim; // Reference to the player's animator component.
    private bool facingRight = true; // For determining which way the player is currently facing.
    private bool Shoot;
    public float moveVelocity;

    //Shield
    public bool canUseShield = true;
    private Rigidbody2D myrigidbody2D;
    public GameObject shield;

    //Wall colliding
    bool collidingWall;

    //Ladders
    public bool isClimbing = false;
    public bool climbingSwitch = false;
    private bool climbing = false;
    float moveVertical;
    float h;
    //Knockback
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    //Miscellaneous
    public GameObject impactEffect;

    //Firing Projectiles
    public Transform firePoint;
    public GameObject Projectile1;
    public GameObject Projectile2;
    public GameObject Projectile3;
    public GameObject CurrentLetters;
    public float shotDelay;
    private float shotDelayCounter;

    //Camera
    public GameObject target;
    public GameObject camera;

    public bool jump;
    private bool crouched = true;
    private bool shoot;
    //</VARIABLES>

    private PlatformerCharacter2D character;

    private void Awake()
    {
        //References
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        anim = GetComponent<Animator>();
        character = GetComponent<PlatformerCharacter2D>();
        CurrentLetters = Projectile1;
    }

    private void Update()
    {
        if (!jump)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                jump = true;
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                    jump = true;
            }
        }
        if (!crouched)
        {
            if (CrossPlatformInputManager.GetButtonDown("Down"))
            {
                crouched = true;
            }
            else
            {
                if (Input.GetButtonDown("Down"))
                {

                    crouched = true;
                }
            }
        }
        moveVelocity = CrossPlatformInputManager.GetAxis("Horizontal");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        // Pass all parameters to the character control script.
        character.Move(moveVelocity, crouch, jump, crouched);
        jump = false;
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        climbing = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsClimbable);
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        //Climbing
        float move = Input.GetAxis("Horizontal");
        Climb(move);
        //Sliding down walls
        if (!collidingWall || grounded)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (collidingWall && grounded)
        {
            if (facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(1f, GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, GetComponent<Rigidbody2D>().velocity.y);
            }
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -5f));
        }
    }

    void Climb(float move)
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (climbing)
            {
                if (grounded)
                {
                    moveVertical = Input.GetAxis("Vertical");
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, moveVertical * maxSpeed);
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    this.gameObject.layer = 17;
                    if (!climbingSwitch)
                    {
                        SetLayerRecursively(this.gameObject, this.gameObject.layer);
                    }
                }
                if (!grounded)
                {
                    moveVertical = Input.GetAxis("Vertical");
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveVertical * maxSpeed);
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    this.gameObject.layer = 17;
                    if (!climbingSwitch)
                    {
                        SetLayerRecursively(this.gameObject, this.gameObject.layer);
                    }
                }
                climbingSwitch = true;

            }
            else
            {
                anim.SetBool("Climbing", false);
            }
        }
        if (!climbing & climbingSwitch)
        {
            this.gameObject.layer = 8;
            SetLayerRecursively(this.gameObject, this.gameObject.layer);
            GetComponent<Rigidbody2D>().gravityScale = 3f;
            climbingSwitch = false;

        }
    }

    public void Move(float move, bool crouch, bool jump, bool crouched)
    {

        float curSpeed = anim.GetFloat("Speed");

        // If crouching, check to see if the character can stand up
        if (!crouch && anim.GetBool("Crouch"))
        {
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                crouch = true;
        }

        if (grounded && jump && anim.GetBool("Ground"))
        {
            anim.SetBool("Ground", false);
            grounded = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }

        moveVelocity = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            moveVelocity = CrossPlatformInputManager.GetAxis("Horizontal") + 2f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVelocity = CrossPlatformInputManager.GetAxis("Horizontal") - 2f;
        }

        // Facing position fix
        if (move > 0 && !facingRight)
            Flip();

        else if (move < 0 && facingRight)
            Flip();

        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            move = (crouch ? move * crouchSpeed : move);
            anim.SetFloat("Speed", Mathf.Abs(move));
        }

        if (knockbackCount <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            if (knockFromRight)
            {
                anim.SetBool("Knockback", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(-knockback, knockback);
            }
            if (!knockFromRight)
            {
                anim.SetBool("Knockback", true);
                GetComponent<Rigidbody2D>().velocity = new Vector2(knockback, knockback);
            }
            knockbackCount -= Time.deltaTime;
            StartCoroutine(knockbackWait());
        }

        //Player actions
        if (Input.GetKey(KeyCode.Space))
        {
            shotDelayCounter -= Time.deltaTime;
            if (shotDelayCounter <= 0)
            {
                Instantiate(CurrentLetters, firePoint.position, firePoint.rotation);
                anim.SetBool("Shoot", true);
                StartCoroutine(Wait());
                // Destroy(letters, 2);
                shotDelayCounter = shotDelay;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(CurrentLetters, firePoint.position, firePoint.rotation);
            anim.SetBool("Shoot", true);
            StartCoroutine(Wait());
            // Destroy(letters, 2);
        }

        if (!climbing)
            if (Input.GetKey(KeyCode.S))
            {
                if (canUseShield)
                {
                    anim.SetBool("Crouch", true);
                    shield.SetActive(true);
                    jumpForce = 100f;
                    maxSpeed = 1f;
                }
                else
                {
                }
            }
            else
            {
                anim.SetBool("Crouch", false);
                shield.SetActive(false);
                jumpForce = 450f;
                maxSpeed = 3f;
            }

        //Switching Weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentLetters = Projectile1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentLetters = Projectile2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentLetters = Projectile3;
        }
    }

    public void getSpeed(float speed)
    {
        speed = anim.GetFloat("Speed");
    }

    public void getVSpeed(float vSpeed)
    {
        vSpeed = anim.GetFloat("vSpeed");
    }

    //Timer for shoot animation
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        anim.SetBool("Shoot", false);
    }

    IEnumerator knockbackWait()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Knockback", false);

    }

    //Collisions
    void OnCollisionEnter2D()
    {
        if (!grounded)
        {
            collidingWall = true;
        }
    }

    void OnCollisionStay2D()
    {
        if (!grounded)
        {
            collidingWall = true;
        }
    }

    void OnCollisionExit2D()
    {
        collidingWall = false;
        if (grounded)
        {
            grounded = false;
        }
    }

    void SetLayerRecursively(GameObject obj, int layerNumber)
    {
        if (null == obj)
        {
            return;
        }
        obj.layer = layerNumber;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, layerNumber);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Trigger")
        {
            target.SetActive(true);
        }
        if (col.gameObject.tag == "AboveLadder")
        {
            anim.SetBool("ClimbUp", true);
        }
        else
        {
            anim.SetBool("ClimbUp", false);
            climbing = false;
            canUseShield = true;

        }

        if (col.gameObject.tag == "Ladder")
        {
            anim.SetBool("Climbing", true);
        }

        if (col.gameObject.tag == "TriggerBossBattle")
        {
            Camera.main.transform.position = transform.position;
        }
        if (col.gameObject.tag == "Enemy")
        {
            knockbackCount = knockbackLength;

            if (col.transform.position.x < transform.position.x)
                knockFromRight = true;
            else
                knockFromRight = false;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "LadderTrigger" || col.gameObject.tag == "AboveLadder")
        {
            canUseShield = false;
        }
        else
        {
            climbing = false;
            canUseShield = true;
        }


    }
    void OnTriggerExit2d(Collider2D col)
    {
        if (col.gameObject.tag == "AboveLadder")
        {
            climbing = false;
            canUseShield = true;
        }
    }
}