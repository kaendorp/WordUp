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
    public bool grounded = false; // Whether or not the player is grounded.
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
    public bool climbingSwitch = false;
    public bool climbing = false;
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
    public float shotDelay;
	private GameObject CurrentLetters;
    private float shotDelayCounter;

    //Camera
    public GameObject target;
    public GameObject camera;

    public bool jump;
    private bool crouched = true;
    private bool shoot;
    //</VARIABLES>

    private PlatformerCharacter2D character;

	//Audio
	[Header("SPEECH")]
	//voice
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;
	//effects
	public AudioClip playerFire;
	public AudioClip playerJumpSound;
	public float speed; //for talking (voice speed)

	private byte[] low;
	private AudioSource _playerSource; //get the audiosource to attach any clips
	private bool isPlayed; //for playing any sound only once

    private void Awake()
    {
        //References
        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
        anim = GetComponent<Animator>();
        character = GetComponent<PlatformerCharacter2D>();
        CurrentLetters = Projectile1;
    }

	private void Start()
	{
		//audio
		_playerSource = gameObject.GetComponent<AudioSource> ();
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
		if (climbingSwitch) {
			grounded = false;
		}
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
        //If crouching and button pressed, check to see if the character can stand up
        if (!crouch && anim.GetBool("Crouch"))
        {
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                crouch = true;
        }

		// Check whether you may jump (grounded = true, button pressed, proper animation)
        if (grounded && jump && anim.GetBool("Ground"))
        {
            anim.SetBool("Ground", false);
            grounded = false;
			//play jump sound here
			_playerSource.clip = playerJumpSound;
			_playerSource.volume = 0.25f;
			_playerSource.loop = false;
			_playerSource.Play ();

            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
        }
	
		/*
		* Checks when it's legit to use the shield, also, stop climbing when grounded 
		*/
		if (grounded) {
			anim.SetBool ("Climbing", false);
			canUseShield = true;
		} else {
			canUseShield = false;
		}

		// Move your body every body!
        moveVelocity = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            moveVelocity = CrossPlatformInputManager.GetAxis("Horizontal") + 2f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVelocity = CrossPlatformInputManager.GetAxis("Horizontal") - 2f;
        }

        // Facing position left/right
        if (move > 0 && !facingRight)
            Flip();

        else if (move < 0 && facingRight)
            Flip();

        // Only control the player if grounded or airControl is turned on
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

		// Fire chosen projectile
        if (Input.GetKeyDown(KeyCode.Space))
        {
			_playerSource.clip = playerFire;
			_playerSource.volume = 0.5f;
			_playerSource.loop = false;
			_playerSource.Play ();

            Instantiate(CurrentLetters, firePoint.position, firePoint.rotation);
            anim.SetBool("Shoot", true);
            StartCoroutine(Wait());
        }

		// Shield up!
        if (!climbing) {
			if (Input.GetKey (KeyCode.S)) {
				if (canUseShield) {
					anim.SetBool ("Crouch", true);
					shield.SetActive (true);
					jumpForce = 100f;
					maxSpeed = 1f;
				} else {
				}
			} else {
				anim.SetBool ("Crouch", false);
				shield.SetActive (false);
				jumpForce = 450f;
				maxSpeed = 3f;
			}
		}

        // Switching projectiles
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentLetters = Projectile1;
			isPlayed = false;
			if(!isPlayed)
			{
				StartCoroutine(PlaySound("b"));
				isPlayed = true;
			}
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentLetters = Projectile2;
			isPlayed = false;
			if(!isPlayed)
			{
				StartCoroutine(PlaySound("ycn"));
				isPlayed = true;
			}
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentLetters = Projectile3;
			isPlayed = false;
			if(!isPlayed)
			{
				StartCoroutine(PlaySound("bouncy"));
				isPlayed = false;
			}
        }
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
	//properly face character left or right before moving
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	//the player triggers lots of things
    void OnTriggerEnter2D(Collider2D col)
    {
		//de ladder
        if (col.gameObject.tag == "AboveTrigger")
        {
			anim.SetBool("ClimbUp", true);
            target.SetActive(true);
        }

        if (col.gameObject.tag == "AboveLadder") {
			anim.SetBool ("ClimbUp", false);
		} else {
			climbing = false;
		}

		//boss battle triggeren
        if (col.gameObject.tag == "TriggerBossBattle")
        {
            Camera.main.transform.position = transform.position;
        }

		//knockback on collision met enemy
        if (col.gameObject.tag == "Enemy")
        {
            knockbackCount = knockbackLength;

            if (col.transform.position.x < transform.position.x)
                knockFromRight = true;
            else
                knockFromRight = false;
        }
    }

	//for colliding with the ladder, as player stays climbing
    void OnTriggerStay2D(Collider2D col)
    {
		//de ladder (als de speler in de trigger is en op W/S drukt, dan klim)
		//if (col.gameObject.tag == "Ladder" && Input.GetKey (KeyCode.W) || col.gameObject.tag == "Ladder" && Input.GetKey (KeyCode.S)) {
		if (col.gameObject.tag == "Ladder") 
		{
			if(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) )
			{
				anim.SetBool ("Climbing", true);
				anim.speed = 1f;
				canUseShield = false;
				grounded = false; //als dit anders gechecked kan worden werkt dit goed (nu grounded on stop)
			}
			else{
				canUseShield = false;
				if(climbingSwitch)
				{
					anim.speed = 0f;
				}
				grounded = false;
			}
		}

		if (col.gameObject.tag == "LadderTrigger" || col.gameObject.tag == "AboveLadder")
        {
			anim.SetBool ("ClimbUp", false);
			canUseShield = false;
			anim.speed = 1f;
			grounded = false;//als dit anders gechecked kan worden werkt dit goed (nu grounded on stop)
        }
    }

	//for colliding with the ladder, as player finishes climbing
    void OnTriggerExit2D(Collider2D col)
    {
		//de ladder (stop met klimmen/opklimmen als je niet in de ladder zit!)
		if (col.gameObject.tag == "AbovePlatform" || col.gameObject.tag == "Ladder") {
			climbing = false;
			anim.SetBool ("ClimbUp", false);
			anim.speed = 1f;
		}

		if (col.gameObject.tag == "AboveLadder")
        {
            climbing = false;
			anim.speed = 1f;
        }
    }

	/**
     * Converts any string in message to sound
     */
	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			//Debug.Log (b);
			if(b < 65)
			{
				_playerSource.clip = number1;
				_playerSource.volume = 0.2f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 65 && b < 105)
			{
				_playerSource.clip = number2;
				_playerSource.volume = 0.4f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 105 && b < 115)
			{	
				_playerSource.clip = number3;
				_playerSource.volume = 0.6f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else
			{	
				_playerSource.clip = number4;
				_playerSource.volume = 0.8f;
				_playerSource.Play ();
				yield return new WaitForSeconds(speed);
			}
		}
	}
}