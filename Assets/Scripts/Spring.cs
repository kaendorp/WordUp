using UnityEngine;
using System;
using System.Collections;

public class Spring : MonoBehaviour
{
    public float springForce = 1200.0f;
    public bool canJump;

    private Animator animator;
    private GameObject player;

    public float bounceDelay = 0.2f;
    private float savePlayerForce;

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
        canJump = true;
        position = new Vector3(0, 0, 0);
    }

    IEnumerator Bounce()
    {
        canJump = false;
        animator.SetBool("Pressing", true);
        animator.SetBool("Releasing", false);
        yield return new WaitForSeconds(bounceDelay);
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, springForce));
        animator.SetBool("Pressing", false);
        animator.SetBool("Releasing", true);
        canJump = true;
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            // Make sure we send the parent object
            // root never returns null, if this Transform doesn't have a parent it returns itself.
            if (player.transform.root.gameObject != player)
            {
                player = player.transform.root.gameObject;
            }

            if (isPlayed == true)
            {
                isPlayed = false;
            }

            if (canJump)
                StartCoroutine(Bounce());
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