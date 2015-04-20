using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class Deur : MonoBehaviour {

	// Use this for initialization
	public static bool open = true;
	public GameObject spawnPoint;
	public Camera mainCamera;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			if(CrossPlatformInputManager.GetButtonDown ("Jump"))
			{

				mainCamera.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
				collision.transform.position = spawnPoint.transform.position;
			}
		}
	}
}