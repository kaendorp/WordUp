using UnityEngine;
using System.Collections;

public class Letter2ProjectileController : MonoBehaviour {

	public float speed = 2.0f;
	public PlatformerCharacter2D player;
	public bool bounceUp = false;
	public float bounceHeight = .25f;
	public float heightDifference = 0.0f;
	public float hitPosition = 0.0f;

	public GameObject enemyDeathEffect;
	public GameObject impactEffect;
	public GameObject projectile;
	private RaycastHit2D hit = new RaycastHit2D();

	private AudioClip _audioClip;
	private Vector3 position;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlatformerCharacter2D>();
		position = new Vector3 (0, 0, 0);
		if (player.transform.localScale.x < 0)
		{
			speed = -speed;
		}
		if (player.transform.localScale.x > 0) {
            speed = 2.0f;
		}

        if (this.gameObject != null)
            Destroy(this.gameObject, 3.5f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (bounceUp == true) {
			transform.Translate(speed * Time.deltaTime, 3f * Time.deltaTime, 0);	
			heightDifference = transform.position.y - hitPosition;
			if(bounceHeight <= heightDifference) {
				bounceUp = false;
			}
		} else {
			transform.Translate(speed * Time.deltaTime, -3f * Time.deltaTime, 0);	
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.transform.tag == "Untagged")
        {
            if (Physics2D.Raycast(transform.position, new Vector2(1, 0), hit.distance - 0.5f, 1, -0.1f, 0.1f) || Physics2D.Raycast(transform.position, new Vector2(-1, 0), hit.distance - 0.5f, 1, -0.1f, 0.1f))
			{
				Destroy(this.gameObject);
			}
			else
            {
				bounceUp = true;
                _audioClip = gameObject.GetComponent<AudioSource>().clip;
				AudioSource.PlayClipAtPoint(_audioClip, position, 0.2f);
				hitPosition = transform.position.y;
			}
		}
        if (col.gameObject.tag == "Enemy")
        {
            Instantiate(enemyDeathEffect, col.transform.position, col.transform.rotation);
            EnemyController enemyController = col.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
                enemyController.TakeDamage();
            else
                Debug.LogError(this.gameObject.name + ": Could not find EnemyController on Enemy target");

            Destroy(this.gameObject);
        }
        else if (col.gameObject.tag == "Boss")
        {
            Instantiate(enemyDeathEffect, col.transform.position, col.transform.rotation);
            Transform bossTransform = col.transform.parent.transform;
            if (bossTransform.name != "Boss")
            {
                Debug.LogError("BossParent Not Found! Returned parent : " + bossTransform.name);
            }
            BossController bossController = bossTransform.GetComponent<BossController>();
            if (bossController != null)
                bossController.HitByPlayerProjectile();
            else
                Debug.LogError(this.gameObject.name + ": Could not find BossController on Boss target");

            Destroy(this.gameObject);
        }
	}
}
