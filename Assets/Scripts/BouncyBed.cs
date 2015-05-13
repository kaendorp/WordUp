using UnityEngine;
using System.Collections;

public class BouncyBed : MonoBehaviour {

	public Vector2 velocity;

	private AudioClip _audioSource;
	private Vector3 positie;
	private bool playSound;

    public bool canJump = true;

	private GameObject player;
	// Use this for initialization
	void Start () 
	{
		playSound = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && canJump)
        {
            canJump = false;
            player = collision.gameObject;
            // Make sure we send the parent object
            // root never returns null, if this Transform doesn't have a parent it returns itself.
            if (player.transform.root.gameObject != player)
            {
                player = player.transform.root.gameObject;
            }

            playSound = true;
            player.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
        canJump = true;
		if(playSound)
		{
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			playSound = false;
		}
	}
}
