using UnityEngine;
using System.Collections;

public class SageController : MonoBehaviour {

    public enum kindType
    {
        Fynn,
        Fyiona
    }

    public kindType _kindType;
    private Animator anim;

    // Bericht
    public string succesMessage = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh    

    // Kind gevonden?    
    private GameObject Stats;
    private int collectedLetters;
    private int lettersNeeded;

    public bool lookAtPlayer = true;

    // Target (usually the player)
    public string targetLayer = "Player";       // TODO: Make this a list, for players and friendly NPC's
    private GameObject target;

    // Shoot
    private bool playerIsLeft;                  // Simple check to see if the player is left to the friendly, important for facing.
    private bool facingLeft = true;             // For determining which way the player is currently facing.

    // Spot
    public float spotRadius = 3;                // Radius in which a player can be spotted
    public bool drawSpotRadiusGismo = true;     // Visual aid in determening if the spot radius
    private Collider2D[] collisionObjects;
    private bool playerSpotted = false;         // Has the friendly spotted the player?

	//voice
	[Header("SPEECH")]
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;
	
	public float speed;
	private byte[] low;
	private AudioSource _audioSource;

    // Use this for initialization
    void Start()
    {
        // Vind het KindObject
        Stats = GameObject.Find("Stats");
        lettersNeeded = Stats.GetComponent<Player>().maxLetters;

        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        succesMessage = succesMessage.Replace("\\n", "\n");       

        if (this.transform.localScale.x >= 0)
            facingLeft = false;

        anim = GetComponent<Animator>();
        if (_kindType == kindType.Fynn)
            anim.SetBool("IsFynn", true);
        else
            anim.SetBool("IsFynn", false);

		_audioSource = gameObject.GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer)
        {
            IsPlayerInRange();
            if (playerSpotted)
                FacePlayer();
        }
        else
        {
            drawSpotRadiusGismo = false;
        }        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            // Zet aantal gevonden letters
            collectedLetters = Stats.GetComponent<Player>().countLetters;          

            if (collectedLetters == lettersNeeded)
            {
                messageObject.GetComponent<TextMesh>().text = succesMessage;
                Destroy(GameObject.Find("WoordMuur"));
            }
            else
            {
                messageObject.GetComponent<TextMesh>().text = "Je hebt nog niet voldoende \n letters verzameld jongeling. \n Ga terug en wederkeer als \n je alle " + lettersNeeded + " letters gevonden hebt.";
            }
			StartCoroutine(PlaySound (succesMessage));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = "";
        }
    }

	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			//Debug.Log (b);
			if(b < 65)
			{
				_audioSource.clip = number1;
				_audioSource.volume = 0.2f;
				_audioSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 65 && b < 105)
			{
				_audioSource.clip = number2;
				_audioSource.volume = 0.4f;
				_audioSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 105 && b < 115)
			{	
				_audioSource.clip = number3;
				_audioSource.volume = 0.6f;
				_audioSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else
			{	
				_audioSource.clip = number4;
				_audioSource.volume = 0.8f;
				_audioSource.Play ();
				yield return new WaitForSeconds(speed);
			}
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
     * Draws a circle gizmo to show the field of view or 'agro' range of an friendly
     */
    private void OnDrawGizmos()
    {
        if (drawSpotRadiusGismo)
        {
            Gizmos.color = Color.yellow;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(this.transform.position, spotRadius);
        }
    }
}
