using UnityEngine;
using System;
using System.Collections;

public class Spring : MonoBehaviour {

	public LayerMask playerLayerMask;
	public Transform rayCastStart;
	public Transform rayCastEnd;
	public float springForce = 1200.0f;

	private Animator animator;
	private float rayCastDistance;
	private GameObject player;

	private bool JumpInputActive {
		get {
			return Input.GetKeyDown(KeyCode.Space);
		}
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		if (rayCastStart != null && rayCastEnd != null) {
			rayCastDistance = rayCastEnd.position.x - rayCastStart.position.x;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (rayCastStart.position, rayCastEnd.position, Color.green);
		RaycastHit2D hit = Physics2D.Raycast(rayCastStart.position, Vector2.right, rayCastDistance, playerLayerMask);
		if (hit.collider != null && !animator.GetBool("Pressing"))
		{
			animator.SetBool("Pressing", true);
			animator.SetBool("Releasing", false);
			player = hit.collider.gameObject;
		}
		else if (hit.collider != null && animator.GetBool("Pressing") && JumpInputActive) {
			player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, springForce));
		}
		else if (hit.collider == null) {
			animator.SetBool("Pressing", false);
			animator.SetBool("Releasing", true);
		}
	}
}