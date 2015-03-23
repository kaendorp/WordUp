using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

    public class PlatformerCharacter2D : MonoBehaviour
    {
        private bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%
		bool collidingWall;
	
        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private LayerMask whatIsClimbable;

        private Transform groundCheck; // fA position marking where to check if the player is grounded.
        private float groundedRadius = .17f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
	    private bool Shoot;
		bool waitActive = false;
		public bool crouched = true;

		public bool isClimbing = false;
		public bool climbingSwitch = false;
		private bool climbing =  false;
		float moveVertical;

		//Knockback
		public float knockback;
		public float knockbackLength;
		public float knockbackCount;
		public bool knockFromRight;

		public bool enabled;

        //Firing Projectiles
        public Transform firePoint;
        public GameObject letters;
		public float shotDelay;
		private float shotDelayCounter;

		//Target = WordMiniGame
		public GameObject target;
		public GameObject camera;

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
			

            anim = GetComponent<Animator>();
	}
	
        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
			climbing = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsClimbable);
			anim.SetBool("Ground", grounded);
            // Set the vertical animation
			anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D> ().velocity.y);
			float move = Input.GetAxis ("Horizontal"); 
		if(Input.GetAxis("Vertical") != 0 )
		{
			if(climbing)
			{

				if(grounded)
					{
				moveVertical = Input.GetAxis ("Vertical");
				anim.SetBool("Climbing", true);
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
				moveVertical = Input.GetAxis ("Vertical");
				
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
		}
		if (!climbing & climbingSwitch)
		{
			anim.SetBool("Climbing", false);
			this.gameObject.layer = 8;
			SetLayerRecursively(this.gameObject, this.gameObject.layer);
			GetComponent<Rigidbody2D>().gravityScale = 3f;
			
			climbingSwitch = false;
			
		}
		if(!collidingWall || grounded){
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}else if (collidingWall && grounded){
			if(facingRight){
				GetComponent<Rigidbody2D>().velocity = new Vector2(1f, GetComponent<Rigidbody2D>().velocity.y);
			}else{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, GetComponent<Rigidbody2D>().velocity.y);
			}
			GetComponent<Rigidbody2D>().AddForce (new Vector2(0f, -5f));
		}

	}


        public void Move(float move, bool crouch, bool jump, bool crouched)
        {
		// If crouching, check to see if the character can stand up
		if (!crouch && anim.GetBool ("Crouch")) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle (ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		if (grounded && jump && anim.GetBool ("Ground")) {
			anim.SetBool ("Ground", false);
			grounded = false;
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0f, jumpForce));
		}
		
		if (move > 0 && !facingRight)
			Flip ();
		
		else if (move < 0 && facingRight)
			Flip ();

		// Set whether or not the character is crouching in the animator

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl) {

			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move * crouchSpeed : move);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat ("Speed", Mathf.Abs (move));

			if(knockbackCount <= 0) {

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (move * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);
			} else {
				if(knockFromRight) {
					anim.SetBool ("Knockback", true);
					GetComponent<Rigidbody2D> ().velocity = new Vector2(-knockback, knockback);
				} if(!knockFromRight) {
					anim.SetBool ("Knockback", true);
					GetComponent<Rigidbody2D> ().velocity = new Vector2(knockback, knockback);
				}
				knockbackCount -= Time.deltaTime;
				anim.SetBool ("Knockback", false);
			
			}

		}

		
		if (Input.GetKey (KeyCode.Space)) {

			shotDelayCounter -= Time.deltaTime;
			if (shotDelayCounter <= 0) {
				anim.SetBool ("Shoot", true);
				Instantiate (letters, firePoint.position, firePoint.rotation);
				StartCoroutine(Wait());
				Destroy(letters, 1);
				shotDelayCounter = shotDelay;
			}
		} 
		if (Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Shoot", true);
			Instantiate (letters, firePoint.position, firePoint.rotation);
			StartCoroutine(Wait());
			Destroy(letters, 1);


		}
		if(!climbing && climbingSwitch)
		if (Input.GetKey (KeyCode.S)) {
			anim.SetBool ("Crouch", true);
		} else {
			anim.SetBool ("Crouch", false);
		}

	}

	IEnumerator Wait(){
		yield return new WaitForSeconds (1);
		anim.SetBool ("Shoot", false);
	}

	void OnCollisionEnter2D(){
		if (!grounded) {
			collidingWall = true;       
		}
	}
	
	void OnCollisionStay2D(){
		if (!grounded) {
			collidingWall = true;       
		}
	}
	
	void OnCollisionExit2D(){
		collidingWall = false;
		if (grounded) {
			grounded = false;
		}
	}

	
	
	void SetLayerRecursively( GameObject obj, int layerNumber)
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
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

			
            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.tag == "Trigger") {
				target.SetActive(true);
			}
		}


	
    }