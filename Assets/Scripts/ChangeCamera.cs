using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour {

	public GameObject defaultCamera;
	public GameObject bossCamera;
	public GameObject invWallLeft;
	public GameObject invWallRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player") {
			defaultCamera.gameObject.active = false;
			bossCamera.gameObject.active = true;
			invWallLeft.SetActive (true);
			invWallRight.SetActive (true);

		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player") {
			bossCamera.gameObject.active = false;
			defaultCamera.gameObject.active = true;
			invWallLeft.SetActive (false);
			invWallRight.SetActive (false);
		}
	}

}
