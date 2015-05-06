using UnityEngine;
using System.Collections;

public class RopeSegment : MonoBehaviour {

    //Player player; (not used)
    //Rigidbody2D rigidbody; (not used)
	public int segmentNumber;
	private Vector3 position;
    private Rigidbody2D myRigidBody;
	private bool isPlayed = false;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (segmentNumber == 2 && !isPlayed) 
		{
			AudioClip _audioSource = this.gameObject.GetComponent<AudioSource> ().clip;
			position = new Vector3(0, 0, 0);
			AudioSource.PlayClipAtPoint(_audioSource, position);
			isPlayed = true;
		}

        if(collider.gameObject.transform.parent != null)
        collider.gameObject.transform.parent.gameObject.SendMessage("OnEnterRope", gameObject, SendMessageOptions.DontRequireReceiver);

	}

	void OnTriggerExit2D(Collider2D collider)
	{
        if (collider.gameObject.transform.parent != null)
		collider.gameObject.transform.parent.gameObject.SendMessage("OnExitRope", gameObject, SendMessageOptions.DontRequireReceiver);
		isPlayed = false;
	}

	public void JumpOff(float theDirection, GameObject jumper)
	{
        myRigidBody = GetComponent<Rigidbody2D>();

		if (theDirection == 0) {
            theDirection = Mathf.Sign(myRigidBody.velocity.x);
		}

		var PlayerController = jumper.GetComponent<PlatformerCharacter2D>();
		//var PlayerRopeScript = jumper.GetComponent<PlayerRope>(); (not used)

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
}

