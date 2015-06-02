using UnityEngine;
using System.Collections;

public class RopeJointController : MonoBehaviour {
    public float horizontalSpeed = 2f;
    public float verticalReleaseBoost = 2f;
    public float horizontalReleaseBoost = 0f;
    private GameObject player;
    private bool isStickied = false;
    private bool manditoryStickDelayOver = true;
    private float manidtoryStickDelay = 0.5f;

    private Vector3 position;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isStickied)
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            this.transform.Translate(Vector2.right * horizontalSpeed * horizontalAxis * Time.deltaTime);

            if (player != null)
            {
                player.transform.position = this.transform.position;
                player.GetComponent<PlatformerCharacter2D>().anim.SetBool("Swinging", true);

                if (manditoryStickDelayOver)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        player.GetComponent<Rigidbody2D>().velocity =
                            new Vector2(
                                this.gameObject.GetComponent<Rigidbody2D>().velocity.x + horizontalReleaseBoost,
                                this.gameObject.GetComponent<Rigidbody2D>().velocity.y + verticalReleaseBoost
                                );
                        player.GetComponent<PlatformerCharacter2D>().anim.SetBool("Swinging", false);
                        player = null;

                        StartCoroutine(GetAwayTime());
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isStickied && manditoryStickDelayOver)
        {
            if (collider.tag != "Player")
                return;

            player = collider.gameObject;

            // Make sure we send the parent object
            // root never returns null, if this Transform doesn't have a parent it returns itself.
            if (player.transform.root.gameObject != player)
            {
                player = player.transform.root.gameObject;
            }
            this.gameObject.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
            isStickied = true;

            //Audio
            AudioClip _audioSource = this.gameObject.GetComponent<AudioSource>().clip;
            position = new Vector3(0, 0, 0);
            AudioSource.PlayClipAtPoint(_audioSource, position);

            StartCoroutine(ManitoryStickDelay());
        }
    }

    private IEnumerator ManitoryStickDelay()
    {
        manditoryStickDelayOver = false;
        yield return new WaitForSeconds(manidtoryStickDelay);
        manditoryStickDelayOver = true;
    }


    /**
     * There needs to be a delay before the OnTriggerEnter should register
     * again. Else the player will forever be stuck on the rope.
     */
    private IEnumerator GetAwayTime()
    {
        yield return new WaitForSeconds(manidtoryStickDelay);
        isStickied = false;
    }
}
