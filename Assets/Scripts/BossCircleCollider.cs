using UnityEngine;
using System.Collections;

public class BossCircleCollider : MonoBehaviour {

	public BossController bossController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		Debug.Log ("HIT");
		if (collider.gameObject.tag == "PlayerProjectile")
		{
			bossController.hitByPlayerProjectile();
		}
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("HIT");
        if (collider.gameObject.tag == "PlayerProjectile")
        {
            bossController.hitByPlayerProjectile();
        }
    }
}
