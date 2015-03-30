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

	public bool isActive = false;

	public Transform firePoint;  
	private GameObject projectile; 
	public GameObject projectilePrefab;     
	public float projectileSpeed = 5;      
	public float projectileLifeTime = 2;    

	public float eventWaitDelay = 3f;

	public GameObject polygonCollider;
	public GameObject circleCollider;
	private Animator anim;

	public float fireDelay = 1;
	private float timeToFire = 0;  


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
			if(isActive)
			StartCoroutine(Idle());

			break;
			case bossEvents.shoot:
				anim.SetBool("IsHit", false);
				StartCoroutine(WaitForShooting());
				break;
				case bossEvents.roarIdle:
				anim.SetBool("IsHit", false);
				circleCollider.SetActive(true);
				StartCoroutine(Wait());
				break;
		}
	}

	public void Shoot(){
		if (bossProjectile != null && bossProjectile.CanShoot) {
			bossProjectile.Shoot ();
		}
	}
	public void EndShoot(){
		anim.SetBool("IsShooting", false);	
		currentEvent = bossEvents.roarIdle;
	}

	public void EndVulnerable()
	{
		circleCollider.SetActive(false);
		anim.SetBool("RoarIdle", false);
		currentEvent =  bossEvents.inactive;
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
			anim.SetBool("IsDefeated", true);
			StartCoroutine(WaitVariable(1.1f));
			StartCoroutine(Defeated());
		}

		isDefeated = true;
		timeForNextEvent = 0.0f;
	}

	void Flee()
	{
		Destroy (gameObject);
		Application.LoadLevel ("MainMenu");
	}

	IEnumerator WaitForShooting(){
			anim.SetBool("IsShooting", true);
			yield return new WaitForSeconds (1f);
			Shoot ();
			yield return new WaitForSeconds (4f);
			EndShoot ();
		}

	IEnumerator Wait(){
		anim.SetBool("RoarIdle", true);
		yield return new WaitForSeconds (3.8f);
		EndVulnerable ();
	}
	IEnumerator Idle(){
		yield return new WaitForSeconds (1f);
		anim.SetBool("IsHit", false);
		yield return new WaitForSeconds (3f);
		currentEvent =  bossEvents.shoot;
	}

	IEnumerator WaitVariable(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		Instantiate (bossDeathFX);
	}

	IEnumerator Defeated()
	{
		yield return new WaitForSeconds (3f);
		Flee ();
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
				if (collider.gameObject.tag == "PlayerProjectile")
				{
					hitByPlayerProjectile();
				}
			}

		}

