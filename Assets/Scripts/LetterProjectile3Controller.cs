using UnityEngine;
using System.Collections;

public class LetterProjectile3Controller : MonoBehaviour {

    public float speed;
    public PlatformerCharacter2D player;
	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;

	private AudioClip _audioClip;
	private Vector3 position;
	private bool isPlayed;
	// Use this for initialization
	void Start () {
		// Debug.Log ("help");
		_audioClip = gameObject.GetComponent<AudioSource>().clip;
		player = FindObjectOfType<PlatformerCharacter2D>();
		position = gameObject.transform.position;
        if (player.transform.localScale.x < 0)
        {
            speed = -1f;
        }
		if (player.transform.localScale.x > 0) {
            speed = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(speed * Time.deltaTime, 6f * Time.deltaTime, 0);	
		if (!isPlayed) {
			AudioSource.PlayClipAtPoint (_audioClip, position, 0.1f);
			isPlayed = true;
		}
        if (this.gameObject != null)
		    Destroy (this.gameObject,5);
	}

	void onTriggerEnter2D(Collision2D obj)
    {
			Instantiate(enemyDeathEffect, obj.transform.position, obj.transform.rotation);
            Destroy(this.gameObject);		
    }

	void OnCollisionEnter2D(Collision2D collider)
	{ 	
		if (collider.gameObject.tag == "Enemy") {
			Instantiate (enemyDeathEffect, collider.transform.position, collider.transform.rotation);
		}
		Instantiate (impactEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
	}
}
