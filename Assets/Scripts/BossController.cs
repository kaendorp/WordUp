using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {

	public delegate void BossEventHandler(int scoremod);
	public static event BossEventHandler bossDied;

	public GameObject inActiveNode = null;
	public GameObject dropToStartNode = null;

	public GameObject dropFXSpawnPoint = null;
	public GameObject bossDeathFX = null;
	public GameObject bossDropFX = null;

	public Transform firePoint;  
	private GameObject projectile; 
	public GameObject projectilePrefab;     
	public float projectileSpeed = 5;      
	public float projectileLifeTime = 2;    

	public float eventWaitDelay = 3f;

	public GameObject polygonCollider;
	public GameObject circleCollider;
	private Animator anim;


	private FireBossProjectile bossProjectile;

	public enum bossEvents
	{
		inactive = 0,
		shoot,
		roarIdle,
	}

	public bossEvents currentEvent = bossEvents.inactive;
	private float timeForNextEvent = 0.0f;
	private GameObject targetNode = null;
	private Vector3 targetPosition = Vector3.zero;

	public int health = 6;
	private int startHealth = 6;
	private bool isDefeated = false;

	void onEnable()
	{
	}
	void onDisable()
	{
	}

	private void Awake() {
		bossProjectile = GetComponent<FireBossProjectile> ();
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentEvent) {
			case bossEvents.inactive:
			StartCoroutine(Idle());

			break;
			case bossEvents.shoot:
				anim.SetBool("IsHit", false);
				if(bossProjectile != null && bossProjectile.CanShoot)
				{
					for(int i = 0; i < 5; i++)
					{
						anim.SetBool("IsShooting", true);
						bossProjectile.Shoot(true);	
						StartCoroutine(WaitForShooting());
						
					}
					currentEvent = bossEvents.roarIdle;
				}
				break;
			case bossEvents.roarIdle:
				anim.SetBool("IsHit", false);
				circleCollider.SetActive(true);
				StartCoroutine(Wait());

				break;
		}
	}

	public void beginBossBattle()
	{
		targetNode = dropToStartNode;
		currentEvent = bossEvents.roarIdle;

		timeForNextEvent = 0.0f;
		health = startHealth;
		isDefeated = false;
	}

	public void hitByPlayerProjectile()
	{
		anim.SetBool("IsHit", true);
		health -= 1;
		if (health <= 0) 
		{
			defeated ();
		}
		currentEvent = bossEvents.inactive;
	}

	void defeated()
	{
		if (isDefeated) 
		{
			Instantiate (bossDeathFX);
			Destroy (gameObject);

		}

		isDefeated = true;
		timeForNextEvent = 0.0f;
	}

	IEnumerator WaitForShooting(){
		yield return new WaitForSeconds (1);
		anim.SetBool("IsShooting", false);
	}

	IEnumerator Wait(){
		anim.SetBool("RoarIdle", true);
		yield return new WaitForSeconds (4);
		anim.SetBool("RoarIdle", false);
		circleCollider.SetActive(false);
		currentEvent =  bossEvents.shoot;
	}
	IEnumerator Idle(){
		yield return new WaitForSeconds (1);
		anim.SetBool("IsHit", false);
		yield return new WaitForSeconds (3);
		currentEvent =  bossEvents.shoot;
	}
	
	void OnCollisionEnter2D(Collision2D collider)
	{
				if (collider.gameObject.tag == "PlayerProjectile")
				{
					hitByPlayerProjectile();
				}
			}
		}

