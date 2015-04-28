using UnityEngine;
using System.Collections;

public class PlayerRope : MonoBehaviour {

	float climbingSpeed = 3.0f;
	float slippingSpeed = 3.0f;
	float horizontalSpeed = 3.0f;

	bool slipsDownABit = true;
	bool slipsUpABit = false;

	bool canMoveHorizontally = false;
	bool canShakeCarrier = false;

    [System.NonSerialized] float climbingSwitch = 0;
    [System.NonSerialized] float slippingSwitch = 0;

	public Hashtable segmentHashTable;
	public GameObject previousParent;

	private float xOffset = 0.0f;
	[System.NonSerialized] float directionToFace = 1;

	void Awake () {
		segmentHashTable = new Hashtable ();
	}

	void Update () {

		float rawVerticalAxis = Input.GetAxisRaw("Vertical");
		float smoothVerticalAxis = Input.GetAxis("Vertical");
		float rawHorizontalAxis = Input.GetAxisRaw("Horizontal");
		float smoothHorizontalAxis = Input.GetAxis("Horizontal");
		
		if (smoothVerticalAxis > 0){
			climbingSwitch = 1;
			slippingSwitch = 0;
		} else if (smoothVerticalAxis < 0){
			climbingSwitch = 0;
			slippingSwitch = 1;
		} else{
			climbingSwitch = 0;
			slippingSwitch = 0;
		}

        if (rawVerticalAxis != 0 || (smoothVerticalAxis < 0 && slipsDownABit) || (smoothVerticalAxis > 0 && slipsUpABit))
        {
            transform.Translate(Vector2.up * Mathf.Pow(climbingSpeed * smoothVerticalAxis, climbingSwitch) * Mathf.Pow(slippingSpeed * smoothVerticalAxis, slippingSwitch) * Time.deltaTime);
        }
		if(smoothHorizontalAxis != 0 && canMoveHorizontally)
        {
			transform.Translate(Vector3.right * horizontalSpeed * smoothHorizontalAxis * Time.deltaTime);
        }
       
        if (canShakeCarrier)
            transform.parent.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * smoothHorizontalAxis * 10);

        if (Input.GetButtonDown("Jump"))
            transform.parent.gameObject.GetComponent<RopeSegment>().JumpOff(rawHorizontalAxis, gameObject);
        if (rawHorizontalAxis != 0)
            directionToFace = rawHorizontalAxis;
       }

	public void RegisterSegment(GameObject segment)
	{
		segmentHashTable.Add (segment.name, segment);
	}

	public void UnregisterSegment(GameObject segment)
	{
		segmentHashTable.Remove (segment.name);
	}

	public void SetXOffset(float offset)
	{
		xOffset = offset;
	}

	public void SetUpController(float newClimbingSpeed, float newSlippingSpeed, float newHorizontalSpeed, bool slipUp, bool slipDown, bool moveHorizontally, bool shake)
	{
		if (newClimbingSpeed != 0.0f)
			climbingSpeed = newClimbingSpeed;
		if ( newSlippingSpeed != 0.0f)
			slippingSpeed = newSlippingSpeed;
		if ( newHorizontalSpeed != 0.0f)
			horizontalSpeed = newHorizontalSpeed;
		slipsDownABit = slipUp;
		slipsUpABit = slipDown;
		canMoveHorizontally = moveHorizontally;
		canShakeCarrier = shake;

		if(canShakeCarrier)
			canMoveHorizontally = false;
	}
}
