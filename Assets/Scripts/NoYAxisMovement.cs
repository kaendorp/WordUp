using UnityEngine;
using System.Collections;

public class NoYAxisMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	float y = 3f;

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = transform.position;
		pos.y = y;
		transform.position = pos;
	}
}
