using UnityEngine;
using System.Collections;

public class BossPolygonCollider : MonoBehaviour {

	public BossController bossController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		Debug.Log ("NO HIT");
	}

}
