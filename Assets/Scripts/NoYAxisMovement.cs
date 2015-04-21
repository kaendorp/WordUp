using UnityEngine;
using System.Collections;

public class NoYAxisMovement : MonoBehaviour {

	public float y = 0.5f;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = transform.position;
		pos.y = y;
		transform.position = pos;
	}
}
