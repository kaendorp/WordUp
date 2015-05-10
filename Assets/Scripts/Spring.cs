using UnityEngine;
using System;
using System.Collections;

public class Spring : MonoBehaviour
{
    public float springForce = 1200.0f;
    public bool canJump;

    private Animator animator;
    private GameObject player;

	//voor spring geluid
	private AudioClip _audioSource;
	private Vector3 position;
	private bool isPlayed;

    // Use this for initialization
    void Start()
    {
		//voor spring geluid
		animator = gameObject.GetComponent<Animator>();
		_audioSource = gameObject.GetComponent<AudioSource> ().clip;
		isPlayed = false;

        position = new Vector3(0, 0, 0);
    }
	
	void OnCollisionStay2D(Collision2D other)
	{
        GameObject player = other.gameObject;
        // Make sure we send the parent object
        // root never returns null, if this Transform doesn't have a parent it returns itself.
        if (player.transform.root.gameObject != player)
        {
            player = player.transform.root.gameObject;
        }

        if (player.tag == "Player" && !animator.GetBool("Pressing"))
		{
			animator.SetBool("Pressing", true);
			animator.SetBool("Releasing", false);
			canJump = true;
		}
        else if (player.tag == "Player" && canJump == true)
		{
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, springForce));
			canJump = false;
		}
        else if (player.tag == "Player")
		{
			animator.SetBool("Pressing", false);
			animator.SetBool("Releasing", true);
			canJump = false;
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && isPlayed == true) 
		{
			isPlayed = false;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && isPlayed == false) 
		{
			AudioSource.PlayClipAtPoint(_audioSource, position);
			isPlayed = true;
		}
	}
}