using UnityEngine;
using System.Collections;

public class RopeSegment : MonoBehaviour {

	public int segmentNumber;
    private Rigidbody2D myRigidBody;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		collider.gameObject.transform.parent.gameObject.SendMessage ("OnEnterRope", gameObject, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		collider.gameObject.transform.parent.gameObject.SendMessage("OnExitRope", gameObject, SendMessageOptions.DontRequireReceiver);
	}

	public void JumpOff(float theDirection, GameObject jumper)
	{
        myRigidBody = GetComponent<Rigidbody2D>();

		if (theDirection == 0) {
            theDirection = Mathf.Sign(myRigidBody.velocity.x);
		}

		var PlayerController = jumper.GetComponent<PlatformerCharacter2D>();
        var PlayerControls = jumper.GetComponent<Platformer2DUserControl>();
		var PlayerRopeScript = jumper.GetComponent<PlayerRope>();

        PlayerControls.jump = false;
		PlayerController.moveVelocity = myRigidBody.velocity.x;

		if(Mathf.Abs(myRigidBody.velocity.x) >= PlayerController.moveVelocity){
            PlayerController.moveVelocity = PlayerController.moveVelocity * Mathf.Sign(myRigidBody.velocity.x);
		} else{
            PlayerController.moveVelocity = myRigidBody.velocity.x;
		}

        jumper.transform.parent = null;
        PlayerRopeScript.segmentHashTable.Clear();

        PlayerController.jumpForce = 400f;
	}
}
