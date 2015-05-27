using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class PlatformerCharacter2D : MonoBehaviour
{

    [Header("GROUND")]
    [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
    private Transform groundCheck; // fA position marking where to check if the player is grounded.
    private float groundedRadius = .17f; // Radius of the overlap circle to determine if grounded
    public bool grounded = false; // Whether or not the player is grounded.

	/*these are never used?
    *[Header("CEILING")]
    *private Transform ceilingCheck; // A position marking where to check for ceilings
    *private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	*/
    [Header("MOVEMENT")]
    [SerializeField] public float maxSpeed = 400f; // A mask determining what is ground to the character
    float VerticalMovement;
    float HorizontalMovement;
    private bool facingRight = true; // For determining which way the player is currently facing.
    public float moveVelocity;
    public bool crouched = true;

    [Header("JUMPING")]
    public bool jump;
    [SerializeField] public float jumpForce = 400f; // Amount of force added when the player jumps.	
    [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;

    [Header("CLIMBING")]
    [SerializeField] private LayerMask whatIsClimbable;// A mask determining what is a ladder to the character
    public bool climbing = false;

    [Header("SHIELD")]
    public bool canUseShield = true;
    public GameObject shield;

    [Header("KNOCKBACK")]
    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

    [Header("PROJECTILES")]
    public Transform firePoint;
    public GameObject Projectile1;
    public GameObject Projectile2;
    public GameObject Projectile3;
	private GameObject CurrentProjectile;

    [Header("ANIMATOR")]
    public Animator anim; // Reference to the player's animator component.

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
        groundCheck = transform.Find("GroundCheck"); //Check for ground
        //ceilingCheck = transform.Find("CeilingCheck"); //Check for ceiling (is never used)
        anim = GetComponent<Animator>();
        CurrentProjectile = Projectile1;
    }

	private void Start()
	{
		//audio
		_playerSource = gameObject.GetComponent<AudioSource> ();
        
        // Analytics
        GameControl.control.projectile1Shot = 0;
        GameControl.control.projectile2Shot = 0;
        GameControl.control.projectile3Shot = 0;
        GameControl.control.enemiesDefeated = 0;
	}

	private void Update()
    {
        if (!jump)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }
        HorizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        // Pass all parameters to the character control script.
        Move(HorizontalMovement, crouch, jump);
        jump = false;
    }

    private void FixedUpdate()
    {
        HorizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
        climbing = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsClimbable);

        if (grounded) {
        } else if (grounded && climbing) {
          climbing = false;
        } else if (climbing) {
          canUseShield = false;
        }

        //Handle climbing
        Climb(HorizontalMovement);
        KnockBack();
        Shooting();
        Shield();

        //Set primary anim parameters
        anim.SetBool("Ground", grounded);
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
        
        //Give player better balance
        GetComponent<Rigidbody2D>().velocity = new Vector2(HorizontalMovement * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    public void Move(float HorizontalMovement, bool crouch, bool jump)
    {
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
        
        // Facing position left/right, flip player
        if (HorizontalMovement > 0 && !facingRight)
            Flip();

        else if (HorizontalMovement < 0 && facingRight)
            Flip();

        if (grounded || airControl)
        {
            HorizontalMovement = (crouch ? HorizontalMovement : HorizontalMovement);
            anim.SetFloat("Speed", Mathf.Abs(HorizontalMovement));
        }
    }

    void Shield()
    {
        if (!climbing)
        {
            if (CrossPlatformInputManager.GetButton("Shield"))
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
                    anim.SetBool("Crouch", false);
                    shield.SetActive(false);
                }
            }
            else
            {
                anim.SetBool("Crouch", false);
                shield.SetActive(false);
                jumpForce = 450f;
                maxSpeed = 3f;
            }
        }
    }

    void Shooting()
    {
        // Fire chosen projectile
        if (CrossPlatformInputManager.GetButtonDown("Shoot"))
        {
            _playerSource.clip = playerFire;
            _playerSource.volume = 0.5f;
            _playerSource.loop = false;
            _playerSource.Play();

            AnalyticsCountShotPlusOne();

            Instantiate(CurrentProjectile, firePoint.position, firePoint.rotation);
            anim.SetBool("Shoot", true);
            StartCoroutine(Wait());
        }

        // Switching projectiles
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            CurrentProjectile = Projectile1;
            isPlayed = false;
            if (!isPlayed)
            {
                StartCoroutine(PlaySound("b"));
                isPlayed = true;
            }
        }

        if (Projectile2 != null)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                CurrentProjectile = Projectile2;
                isPlayed = false;
                if (!isPlayed)
                {
                    StartCoroutine(PlaySound("ycn"));
                    isPlayed = true;
                }
            }
        }
        if (Projectile3 != null)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire3"))
            {
                CurrentProjectile = Projectile3;
                isPlayed = false;
                if (!isPlayed)
                {
                    StartCoroutine(PlaySound("bouncy"));
                    isPlayed = false;
                }
            }
        }
    }

    void KnockBack()
    {
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
    }

    void Climb(float HorizontalMovement)
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (climbing)
            {
                if (grounded)
                {
                    VerticalMovement = Input.GetAxis("Vertical");
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, VerticalMovement * maxSpeed);
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    this.gameObject.layer = 17;
                    SetLayerRecursively(this.gameObject, this.gameObject.layer);

                }
                if (!grounded)
                {
                    VerticalMovement = Input.GetAxis("Vertical");
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, VerticalMovement * maxSpeed);
                    GetComponent<Rigidbody2D>().gravityScale = 0;
                    this.gameObject.layer = 17;

                    SetLayerRecursively(this.gameObject, this.gameObject.layer);
                }
            }
            else
            {
                anim.SetBool("Climbing", false);
            }
        }
        if (!climbing)
        {
            this.gameObject.layer = 8;
            SetLayerRecursively(this.gameObject, this.gameObject.layer);
            GetComponent<Rigidbody2D>().gravityScale = 3f;
            anim.SetBool("Climbing", false);
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

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "AbovePlatform" || col.gameObject.tag == "AboveTrigger")
        {
            anim.SetBool("ClimbUp", true);
            canUseShield = false;
        }
        else
        {
            canUseShield = true; 
        }
    }

	//the player triggers lots of things
    void OnTriggerEnter2D(Collider2D col)
    {
		//de ladder
        if (col.gameObject.tag == "AboveTrigger")
        {
            canUseShield = false;
        }

        if (col.gameObject.tag == "AboveLadder") {
            canUseShield = false;
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
            if (Input.GetAxis("Vertical") != 0)
			{
				anim.SetBool ("Climbing", true);
				anim.speed = 1f;
				canUseShield = false;
				grounded = false; //als dit anders gechecked kan worden werkt dit goed (nu grounded on stop)
			}
			else{
				canUseShield = false;
				if(climbing)
				{
					anim.speed = 0f;
				}
				grounded = false;
			}
		}

        if (col.gameObject.tag == "LadderTrigger" || col.gameObject.tag == "AboveLadder")
        {
            anim.SetBool("ClimbUp", true);
            canUseShield = false;
            anim.speed = 1f;
            //grounded = false; /als dit anders gechecked kan worden werkt dit goed (nu grounded on stop)
        }
        else
        {
            anim.SetBool("ClimbUp", false);
        }
    }

	//for colliding with the ladder, as player finishes climbing
    void OnTriggerExit2D(Collider2D col)
    {
		//de ladder (stop met klimmen/opklimmen als je niet in de ladder zit!)
		if (col.gameObject.tag == "AbovePlatform" || col.gameObject.tag == "Ladder") {
			climbing = false;
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

    /**
     * Counts amount of shots fired per projectiletype
     */
    public void AnalyticsCountShotPlusOne()
    {
        if (CurrentProjectile == Projectile1)
        {
            GameControl.control.projectile1Shot++;
        }
        else if (CurrentProjectile = Projectile2)
        {
            GameControl.control.projectile2Shot++;
        }
        else if (CurrentProjectile = Projectile3)
        {
            GameControl.control.projectile3Shot++;
        }
    }
}