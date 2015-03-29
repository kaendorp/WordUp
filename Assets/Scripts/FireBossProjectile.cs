using UnityEngine;
using System.Collections;

public class FireBossProjectile : MonoBehaviour {

	public GameObject bossProjectilePrefab;
	public float cooldownTimeThreshold = 0.25f; //amount of time (in secs.) between two shots
	private float cooldownTime;
	private Transform player;
	public GameObject firePoint;

	// Use this for initialization
	private void Start () {
		this.cooldownTime = 0.0f;
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	private void Update () {
		if(this.cooldownTime > 0) {
			this.cooldownTime -= Time.deltaTime;
		}
	}

	public void Shoot(bool isEnemy)
	{
		if (this.CanShoot) {
			this.cooldownTime = this.cooldownTimeThreshold;
			GameObject shot = Instantiate(bossProjectilePrefab, firePoint.transform.position, Quaternion.identity) as GameObject;
			shot.GetComponent<Rigidbody2D>().AddForce(((Vector2)(player.position - shot.transform.position)).normalized * 500);
				
		}
		
	}
	
	public bool CanShoot
	{
		get 
		{
			return this.cooldownTime <= 0.0f;
		}
	}
}
