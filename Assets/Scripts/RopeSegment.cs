using UnityEngine;
using System.Collections;

public class RopeSegment : MonoBehaviour {

    Player player;
    Rigidbody2D rigidbody;
	public int segmentNumber;
    private Rigidbody2D myRigidBody;

	void OnTriggerEnter2D(Collider2D collider)
	{
        collider.gameObject.transform.parent.gameObject.SendMessage("OnEnterRope", gameObject, SendMessageOptions.DontRequireReceiver);
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
		var PlayerRopeScript = jumper.GetComponent<PlayerRope>();

        PlayerController.jump = false;
		PlayerController.moveVelocity = myRigidBody.velocity.x;

		if(Mathf.Abs(myRigidBody.velocity.x) >= PlayerController.moveVelocity){
            PlayerController.moveVelocity = PlayerController.moveVelocity * Mathf.Sign(myRigidBody.velocity.x);
		} else{
            PlayerController.moveVelocity = myRigidBody.velocity.x;
		}

        jumper.transform.parent = null;
        jumper.GetComponent<PlayerRope>().segmentHashTable.Clear();

        PlayerController.enabled = true;
        jumper.GetComponent<PlayerRope>().enabled = false;


        PlayerController.jumpForce = 400f;
	}

    //void OnTriggerExit2D()
    //{
    //    rigidbody.gravityScale = 3f;
    //}
}

