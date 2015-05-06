using UnityEngine;
using System;
using System.Collections;

public class Spring : MonoBehaviour
{
    public LayerMask playerLayerMask;
    public Transform rayCastStart;
    public Transform rayCastEnd;
    public float springForce = 1200.0f;
    public bool canJump;

    private Animator animator;
    private float rayCastDistance;
    private GameObject player;

	//voor spring geluid
	private AudioClip _audioSource;
	private Vector3 position;
	private bool isPlayed;

    private bool JumpInputActive
    {
        get
        {
            return Input.GetButtonDown("Jump");
        }
    }

    // Use this for initialization
    void Start()
    {
		//voor spring geluid
		animator = gameObject.GetComponent<Animator>();
		_audioSource = gameObject.GetComponent<AudioSource> ().clip;
		isPlayed = false;
		position = gameObject.transform.position;

        if (rayCastStart != null && rayCastEnd != null)
        {
            rayCastDistance = rayCastEnd.position.x - rayCastStart.position.x;
        }
    }
	
	void OnCollisionStay2D(Collision2D other)
	{
		RaycastHit2D hit = Physics2D.Raycast(rayCastStart.position, Vector2.right, rayCastDistance, playerLayerMask);
		
		if (other.gameObject.tag == "Player" && !animator.GetBool("Pressing"))
		{
			animator.SetBool("Pressing", true);
			animator.SetBool("Releasing", false);
			player = hit.collider.gameObject;
			canJump = true;
		}
		else if (other.gameObject.tag == "Player" && canJump == true)
		{
			player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, springForce));
			canJump = false;
		}
		else if (other.gameObject.tag == "Player")
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