using UnityEngine;
using System.Collections;

public class Letter2ProjectileController : MonoBehaviour {

	public float speed = 2.0f;
	public PlatformerCharacter2D player;
	public bool bounceUp = false;
	public float bounceHeight = .25f;
	public float heightDifference = 0.0f;
	public float hitPosition = 0.0f;

	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;
	private RaycastHit2D hit = new RaycastHit2D();

	private AudioClip _audioClip;
	private Vector3 position;

	// Use this for initialization
	void Start () {
		_audioClip = gameObject.GetComponent<AudioSource>().clip;
		player = FindObjectOfType<PlatformerCharacter2D>();
		position = gameObject.transform.position;
		if (player.transform.localScale.x < 0)
		{
			speed = -speed;
		}
		if (player.transform.localScale.x > 0) {
            speed = 2.0f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (bounceUp == true) {
			transform.Translate(speed * Time.deltaTime, 3f * Time.deltaTime, 0);	
			heightDifference = transform.position.y - hitPosition;
			if(bounceHeight <= heightDifference) {
				bounceUp = false;
			}
		} else {
			transform.Translate(speed * Time.deltaTime, -3f * Time.deltaTime, 0);	
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Untagged")
        {
            if (Physics2D.Raycast(transform.position, new Vector2(1, 0), hit.distance - 0.5f, 1, -0.1f, 0.1f) || Physics2D.Raycast(transform.position, new Vector2(-1, 0), hit.distance - 0.5f, 1, -0.1f, 0.1f))
			{
				Destroy(this.gameObject);
			}
			else
            {
				bounceUp = true;
				AudioSource.PlayClipAtPoint(_audioClip, position, 0.2f);
				hitPosition = transform.position.y;
			}
		}
        if (col.transform.tag == "Boss")
        {
            StartCoroutine(ToggleKinematic());
        }
		if (col.transform.tag == "Enemy") {
			Destroy(this.gameObject);
		}
		Destroy(this.gameObject, 3.5f);
	}
	
    /**
     * Needed for the boss to register a hit. This projectile can not 
     * be kinematic if it needs to trigger a collider.
     */
    IEnumerator ToggleKinematic()
    {
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
