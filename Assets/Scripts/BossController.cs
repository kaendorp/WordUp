using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class BossController : MonoBehaviour
{
    public bool isActive = false;

    public enum bossSequence
    {
        tutorial,
        level1,
        level2,
        level3
    }

    public enum bossEvents
    {
        roarAttack,
        inactive,
        shoot,
        bounce,
        arc,
        stomp,
        roarIdle,
    }

    public bossSequence currentSequence = bossSequence.tutorial;
    public bossEvents currentEvent = bossEvents.inactive;

    private BetterList<bossEvents> bossSequenceList;
    private int bossSequenceListValue = 0;
    private bool coroutineStarted = false;

    // COLLIDERS
    [Header("COLLIDERS")]
    public GameObject polygonCollider;
    public GameObject circleCollider;
    private Animator anim;

    // HEALTH
    [Header("HEALTH")]
    public int startHealth = 3;
    public int currentHealth = 3;

    // DEATH
    [Header("DEATH")]
    public GameObject bossDeathFX = null;
    public GameObject bossDoves;
    public bool finalFight;

    // MESSAGE
    [Header("MESSAGE")]
    public GameObject messageObject;        // TextMesh object that will display our message
    [TextArea(1, 2)]
    public string endMessage1 = "";
    [TextArea(1, 2)]
    public string endMessage2 = "";

    // SHOOT
    [Header("SHOOT")]
    public int fireAmount = 3;
    public float cooldownTime = 1f; //amount of time (in secs.) between two shots
    private GameObject player;
    private GameObject shot;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifeTime = 2f;

    // BOUNCE
    [Header("BOUNCE")]
    public GameObject bouncePrefab;
    public int bouncePoints = 4;
    public float bounceDistance = 2f;
    public float bounceHeight = 2f;
    public float bounceSpeed = 1f;
    public float bounceFirepointHeight = -1f;
    private bool goingDown = true;          // bouncy projectile going down?
    private Vector2[] bouncyPath;

    // ARC
    [Header("ARC")]
    public GameObject arcPrefab;
    public float timeToTarget = 1f;

    // ROAR ATTACK
    [Header("ROAR")]
    private bool wasHit = false;

    // ICE
    [Header("ICE")]
    public Transform iceSpawn;
    public GameObject icicles;
    public GameObject icePlatform;
    public float platformHeight = 2f;
    private GameObject iceInit;

	[Header("MUSIC")]
	public GameObject battleMusic;
	public GameObject bossIsDefeatedMusic;
	public GameObject bossIntroMusic;

	[Header("SOUNDS")]
	public AudioClip evilLaugh;
	public AudioClip shoot;
	public AudioClip shootUp;
	public AudioClip shootDown;
	public AudioClip woosh;
	public AudioClip stomp;
	public AudioClip bossIsHit;
	private AudioSource bossSource;

	[Header("VOICE")]
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;
	public float speed;
	private byte[] low;

    // Healthbar
    private GameObject HUD;
    private GameObject BossBar;
    private GameObject BossFill; // healthbar in UI

    /**
     * Awake
     *
     * Sets the animator, initiates the bossSequenceList and fills the list
     * with the sequence of events.
     */
    private void Awake()
    {
        anim = GetComponent<Animator>();

        bossSequenceList = new BetterList<bossEvents>();

        HUD = GameObject.Find("HUD");
        BossBar = HUD.transform.FindChild("BossHud").gameObject;
        BossFill = BossBar.transform.FindChild("BossFill").gameObject;

        switch (currentSequence)
        {
            case bossSequence.tutorial:
                bossSequenceList.Add(bossEvents.inactive);
                bossSequenceList.Add(bossEvents.shoot);
                bossSequenceList.Add(bossEvents.roarIdle);
                break;
            case bossSequence.level1:
                bossSequenceList.Add(bossEvents.inactive);
                bossSequenceList.Add(bossEvents.bounce);
                bossSequenceList.Add(bossEvents.roarIdle);
                bossSequenceList.Add(bossEvents.roarAttack);
                break;
            case bossSequence.level2:
                bossSequenceList.Add(bossEvents.inactive);
                bossSequenceList.Add(bossEvents.arc);
                bossSequenceList.Add(bossEvents.stomp);
                bossSequenceList.Add(bossEvents.roarIdle);
                break;
            case bossSequence.level3:
                bossSequenceList.Add(bossEvents.inactive);
                bossSequenceList.Add(bossEvents.arc);
                bossSequenceList.Add(bossEvents.shoot);
                bossSequenceList.Add(bossEvents.bounce);
                bossSequenceList.Add(bossEvents.stomp);
                bossSequenceList.Add(bossEvents.roarIdle);
                break;
            default:
                bossSequenceList.Add(bossEvents.inactive);
                bossSequenceList.Add(bossEvents.shoot);
                bossSequenceList.Add(bossEvents.roarIdle);
                break;
        }
    }

    /**
     * A message set in the inspector will always have escaped newlines.
     *
     * This will unescape the escaped newlines.
     */
    private void Start()
    {
		bossIsDefeatedMusic.SetActive (false);
		bossSource = gameObject.GetComponent<AudioSource>();
        if (!string.IsNullOrEmpty(endMessage1))
            endMessage1 = endMessage1.Replace("\\n", "\n");
        if (!string.IsNullOrEmpty(endMessage2))
            endMessage2 = endMessage2.Replace("\\n", "\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && !coroutineStarted)
        {
            coroutineStarted = true;    // coroutineStarted is set to false in setNextAction();

            switch (currentEvent)
            {
                case bossEvents.inactive:
                    StartCoroutine(Idle());
                    break;
                case bossEvents.shoot:
                    StartCoroutine(Shoot());
                    break;
                case bossEvents.bounce:
                    StartCoroutine(Bounce());
                    break;
                case bossEvents.arc:
                    StartCoroutine(Arc());
                    break;
                case bossEvents.stomp:
                    StartCoroutine(Stomp());
                    break;
                case bossEvents.roarAttack:
                    StartCoroutine(RoarAttack());
                    break;
                case bossEvents.roarIdle:
                    StartCoroutine(Roar());
                    break;
            }
        }
    }

    /**
     * Sets the playerobject.
     *
     * Value is passed from TriggerBossBattle.OnTriggerEnter2D()
     */
    public void SetPlayerObject(GameObject passedObject)
    {
        player = passedObject;
    }

    /**
     * Makes the boss roar at the start of the battle.
     *
     * Also sets the currenthealth to the set StartHealth
     */
    public void BeginBossBattle()
    {
        GameControl.control.bossBattleStartTime = Time.timeSinceLevelLoad; // Analytics
        GameControl.control.bossBattleStarted = true; // Analytics
        isActive = true;
        currentHealth = startHealth;
        BossBar.SetActive(true);
        StartCoroutine(FillHealthBar());
    }

    /**
     * Slowly fills the healthbar of the boss, from 0 to full. For aesthetics.
     * Uses the image fill method, the BossFill should be an UI Image with it's 
     * type set to 'Filled', method to Horizontal, orgin Left.
     * 
     * Called by beginBossBattle();
     */
    IEnumerator FillHealthBar()
    {
        float elapsedTime = 0f;
        float setWidth;

        while (elapsedTime < 5)
        {
            float current = (float)currentHealth / (float)startHealth;
            setWidth = Mathf.Lerp(0, current, ((elapsedTime / 5)));
            BossFill.GetComponent<Image>().fillAmount = setWidth;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /**
     * Every action calls this method at the end to initiate the next action
     * according to the selected bossSequence.
     *
     * It also ends the coroutineStarted check so it will start the next
     * action in Update()
     */
    private void SetNextAction()
    {
        // Debug.Log("Setting Action to : " + bossSequenceList[bossSequenceListValue].ToString());
        currentEvent = bossSequenceList[bossSequenceListValue];
        bossSequenceListValue++;

        if (bossSequenceListValue >= bossSequenceList.size) // ListValue starts at 0, List.size starts at 1
        {
            bossSequenceListValue = 0;
        }
        coroutineStarted = false;
    }

    /**
     * Idle animation
     *
     * Disables the circleCollider and makes sure the IsHit animation
     * is disabled.
     */
    IEnumerator Idle()
    {
        circleCollider.SetActive(false);
        anim.SetBool("IsHit", false);
        yield return new WaitForSeconds(2f);
        //Debug.Log("IDLE DONE");
        SetNextAction();
    }

    /**
     * Shooting projectiles action
     *
     * Triggers the shooting animation and fires 'fireAmount' of projectiles.
     */
    IEnumerator Shoot()
    {
        anim.SetBool("IsShooting", true);
        yield return new WaitForSeconds(1f); // Wait for the animation to end

        for (int fired = 0; fired < fireAmount; fired++)
        {
			bossSource.clip = shoot;
			bossSource.loop = false;
			bossSource.volume = 0.5f;
			bossSource.Play();
			FireProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }

        anim.SetBool("IsShooting", false);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        //Debug.Log("SHOOT DONE");
        SetNextAction();
    }

    /**
     * Spawns a projectile and fires it towards the player
     *
     * Player value is set in setPlayerObject()
     */
    private void FireProjectile()
    {
        shot = (GameObject)Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        Vector2 force = (Vector2)player.transform.position - (Vector2)firePoint.transform.position;
        shot.GetComponent<Rigidbody2D>().AddForce(force.normalized * (projectileSpeed * 30));

		Destroy(shot, projectileLifeTime);

        Debug.DrawRay(firePoint.transform.position, force, Color.yellow);
    }

    /**
     * Shooting bouncing projectiles action
     *
     * Triggers the shooting animation and fires 'fireAmount' of projectiles.
     */
    IEnumerator Bounce()
    {
        anim.SetBool("IsShooting", true);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        bool firePointUp = true;

        for (int fired = 0; fired < fireAmount; fired++)
        {
			//play fired sound
            if (firePointUp)
            {
				bossSource.clip = shootDown;
				bossSource.loop = false;
				bossSource.volume = 0.5f;
				bossSource.Play();

                goingDown = true;
                firePointUp = false;
            }
            else
            {
				bossSource.clip = shootDown;
				bossSource.loop = false;
				bossSource.volume = 0.5f;
				bossSource.Play();

                goingDown = false;
                firePointUp = true;
            }

            BounceProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }

        anim.SetBool("IsShooting", false);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        //Debug.Log("SHOOT DONE");

        SetNextAction();
    }

    private void BounceProjectile()
    {
        shot = (GameObject)Instantiate(bouncePrefab, firePoint.transform.position, firePoint.transform.rotation);
        SetBouncyPath();
        Vector2[] savedPath = bouncyPath; // might change during fire
        StartCoroutine(MoveToBouncePoint(shot, savedPath, 1));
        Destroy(shot, projectileLifeTime);
    }

    /**
     * Set the path the bouncing projectile will make.
     *
     * Y position of the boss is set to bottom. Provided the boss is standing on the floor this will give us the "floor" height.
     * 
     * Called by BouncyProjectile()
     */
    private void SetBouncyPath()
    {
        Vector2 bouncy1 = firePoint.transform.position;
        bouncyPath = new Vector2[bouncePoints + 1];

        bouncyPath[0] = bouncy1;

        for (int i = 1; i <= bouncePoints; i++)
        {
            Vector2 bouncyI;
            if (goingDown)
            {
                bouncyI = new Vector2((bouncyPath[i - 1].x - bounceDistance), this.transform.position.y);
                goingDown = false;
            }
            else
            {
                bouncyI = new Vector2((bouncyPath[i - 1].x - bounceDistance), this.transform.position.y + bounceHeight);
                goingDown = true;
            }

            bouncyPath[i] = bouncyI;
        }
    }

    /**
     * Moves the bouncy projectile from one point to the next.
     * This method is an iteration of itself so that the projectile will go
     * from one point in savedPath to the next point in savedPath.
     * 
     * savedPath is set in SetBouncyPath(), but given in BouncyProjectile()
     * 
     * Called in BouncyProjectile()
     */
    IEnumerator MoveToBouncePoint(GameObject shot, Vector2[] savedPath, int bouncePoint)
    {
        float elapsedTime = 0f;
        Vector3 startposition = new Vector3();

        if (shot != null)
            startposition = shot.transform.position;

        while (elapsedTime < bounceSpeed)
        {
            if (shot == null)
                break;
            shot.transform.position = Vector3.Lerp(startposition, savedPath[bouncePoint], (elapsedTime / bounceSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bouncePoint + 1 <= bouncePoints && shot != null)
        {
            StartCoroutine(MoveToBouncePoint(shot, savedPath, bouncePoint + 1));
        }
        else
        {
            Destroy(shot);
        }
    }

    /**
     * Shooting projectiles action
     *
     * Triggers the shooting animation and fires 'fireAmount' of projectiles.
     */
    IEnumerator Arc()
    {
        anim.SetBool("IsShooting", true);
        yield return new WaitForSeconds(1f); // Wait for the animation to end

        for (int fired = 0; fired < fireAmount; fired++)
        {
			bossSource.clip = shootUp;
			bossSource.loop = false;
			bossSource.volume = 1f;
			bossSource.Play();

            ArcProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }

        anim.SetBool("IsShooting", false);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        //Debug.Log("SHOOT DONE");
        SetNextAction();
    }

    private void ArcProjectile()
    {
        shot = (GameObject)Instantiate(arcPrefab, firePoint.transform.position, firePoint.transform.rotation);
        shot.GetComponent<Rigidbody2D>().gravityScale = 1f;
        shot.GetComponent<Rigidbody2D>().angularDrag = 0;
        Vector3 velocity = CalculateThrowSpeed(shot.transform.position, player.transform.position, timeToTarget);

        if (float.IsNaN(velocity.y) || float.IsNaN(velocity.x)) // if calculation fails, give the projectile a weak nudge
            velocity = new Vector3(-2f, 1f, 0);
        shot.GetComponent<Rigidbody2D>().velocity = velocity;
        Destroy(shot, projectileLifeTime);
    }

    /**
     * Calculate the velocity of the projectile depending on the firepoint and player location.
     * Returns a Vector3 to be used as the velocity of the projectiles RigidBody2D.
     * 
     * http://answers.unity3d.com/questions/248788/calculating-ball-trajectory-in-full-3d-world.html
     * Based of an awnser by Tomer Barkan, May 14, 2013
     */
    private Vector3 CalculateThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = timeToTarget;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }

    IEnumerator Stomp()
    {
        if (iceInit != null)
        {
            Destroy(iceInit);
        }
		bossSource.clip = stomp;
		bossSource.loop = false;
		bossSource.volume = 0.5f;
		bossSource.Play();
        anim.SetTrigger("StompAttack");

        yield return new WaitForSeconds(0.3f);
        Vector3 startPosition = icePlatform.transform.position;
        Vector3 endPosition = new Vector3(icePlatform.transform.position.x, icePlatform.transform.position.y + platformHeight, icePlatform.transform.position.z);
        StartCoroutine(IcePlatform(startPosition, endPosition));

        iceInit = (GameObject)Instantiate(icicles, iceSpawn.transform.position, iceSpawn.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        SetNextAction();
    }

    IEnumerator IcePlatform(Vector3 from, Vector3 to)
    {
        float iceUpTime = 0.1f;

        float startTime = Time.time;
        while (Time.time < startTime + iceUpTime)
        {
            icePlatform.transform.position = Vector3.Lerp(from, to, (Time.time - startTime) / iceUpTime);
            yield return null;
        }
        iceUpTime = 6f;
        startTime = Time.time;
        while (Time.time < startTime + iceUpTime)
        {
            icePlatform.transform.position = Vector3.Lerp(to, from, (Time.time - startTime) / iceUpTime);
            yield return null;
        }
    }

    /**
     * Roar attack: boss is hit by the player
     *
     * Trigger an animation and push the player back
     */
    IEnumerator RoarAttack()
    {
        if (wasHit)
        {
            wasHit = false;
            anim.SetBool("IsHit", false);
            anim.SetTrigger("RoarAttack");
            StartCoroutine(PushPlayerLeft());
            yield return new WaitForSeconds(2f);
            //Debug.Log("ROAR DONE");
        }
        SetNextAction();
    }

    IEnumerator PushPlayerLeft()
    {
        float elapsedTime = 0f;
		bossSource.clip = woosh;
		bossSource.loop = false;
		bossSource.volume = 0.5f;
		bossSource.Play();
        while (elapsedTime < 1)
        {
            player.GetComponent<Rigidbody2D>().AddForce(transform.right * -600);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /**
     * Roar action (laughs at player)
     *
     * Triggers the roar animation and activates the circleCollider.
     * The player can then shoot the boss in the mouth.
     */
    IEnumerator Roar()
    {
		//sound of evil laughing start
		bossSource.clip = evilLaugh;
		bossSource.loop = true;
		bossSource.volume = 1f;
		bossSource.Play();

        anim.SetBool("RoarIdle", true);
        yield return new WaitForSeconds(1f);
        circleCollider.SetActive(true);
        yield return new WaitForSeconds(2.17f);
        circleCollider.SetActive(false);
        anim.SetBool("RoarIdle", false);

		//stop laughing you ass
		bossSource.Stop ();
        yield return new WaitForSeconds(1f);
		//Debug.Log("ROAR DONE");
        SetNextAction();
    }

    /**
     * Boss is hit by player.
     *
     * Called by BossCirlceCollider.OnCollisionEnter2D()
     */
    public void HitByPlayerProjectile()
    {
        circleCollider.SetActive(false);
        anim.SetBool("IsHit", true);

        GameControl.control.bossDamageTaken++; // Analytics

        int healthBefore = currentHealth;
		currentHealth--;
        int healthAfter = currentHealth;
       
        StartCoroutine(BossHealthBarTakeDamage(healthBefore, healthAfter));

        wasHit = true; // can trigger roarAttack

        if (currentHealth <= 0) {
            bossSource.Stop();
			StartCoroutine (Defeated ());
			isActive = false; // Makes sure the next action isn't accidentally called in Update()
		} else {
			bossSource.clip = bossIsHit;
			bossSource.loop = false;
			bossSource.volume = 0.25f;
			bossSource.Play ();
		}
    }

    /**
     * Drains the healthbar of the boss when taking damage. 
     * Uses the image fill method, the BossFill should be an UI Image with it's 
     * type set to 'Filled', method to Horizontal, orgin Left.
     * 
     * The first value given to this method should be the so called, 'from' and 
     * 'to' values, as you usually want the healthbar to go 'from' 3 hp 'to' 2 hp,
     * for example.
     * 
     * Called by hitByPlayerProjectile();
     */
    IEnumerator BossHealthBarTakeDamage(int healthBefore, int healthAfter)
    {
        float elapsedTime = 0f;
        float setWidth;

        float before = (float)healthBefore / (float)startHealth;
        float after = (float)healthAfter / (float)startHealth;

        while (elapsedTime < 1f)
        {
            setWidth = Mathf.Lerp(before, after, ((elapsedTime / 1f)));
            BossFill.GetComponent<Image>().fillAmount = setWidth;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    /**
     * Called by hitByPlayerProjectile() when the boss ran out of health.
     */
    IEnumerator Defeated()
    {
		// Stop battle music, cue evil discussion music
		StartCoroutine(ChangeMusic());

        // Kill boss minions when defeated
        if (iceInit != null)
        {
            Destroy(iceInit);
        }

        // TODO: Disable player controlls without stopping time
        anim.SetBool("IsDefeated", true);
        yield return new WaitForSeconds(3f);
        BossBar.SetActive(false);
        messageObject.SetActive(true);
        messageObject.GetComponent<TextMesh>().text = endMessage1;
		yield return new WaitForSeconds(1f);
		StartCoroutine(PlaySound (endMessage1));
        yield return new WaitForSeconds(3f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);
        messageObject.GetComponent<TextMesh>().text = endMessage2;
		StartCoroutine(PlaySound (endMessage2));
        yield return new WaitForSeconds(4f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);

        if (!finalFight)
        {
            anim.SetTrigger("StandAndFlee");

            yield return new WaitForSeconds(1.1f);
            Instantiate(bossDeathFX, this.transform.position, Quaternion.identity);
            Instantiate(bossDoves, this.transform.position, Quaternion.identity);
        }
        else
        {
            anim.SetTrigger("FallAndDie");
            polygonCollider.SetActive(false);
        }
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        PlayerVictory();
    }

	/**
     * Converts any string in message to sound
     */
	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			//Debug.Log (b);
			if(b < 65)
			{
				bossSource.clip = number1;
				bossSource.volume = 0.2f;
				bossSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 65 && b < 105)
			{
				bossSource.clip = number2;
				bossSource.volume = 0.4f;
				bossSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else if(b > 105 && b < 115)
			{	
				bossSource.clip = number3;
				bossSource.volume = 0.6f;
				bossSource.Play ();
				yield return new WaitForSeconds(speed);
			}
			else
			{	
				bossSource.clip = number4;
				bossSource.volume = 0.8f;
				bossSource.Play ();
				yield return new WaitForSeconds(speed);
			}
		}
	}


	/**
     * change from battle music to ominous music for dialogue
     */
	private IEnumerator ChangeMusic()
	{
		float fTimeCounter = 0f;
		bossIsDefeatedMusic.SetActive (true);
		AudioSource _audioSource = battleMusic.GetComponent<AudioSource> ();
		AudioSource _audioSource2 = bossIsDefeatedMusic.GetComponent<AudioSource> ();
		while(!(Mathf.Approximately(fTimeCounter, 0.5f)))
		{
			fTimeCounter = Mathf.Clamp01(fTimeCounter + Time.deltaTime);
			_audioSource.volume = 0.5f - fTimeCounter;
			_audioSource2.volume = fTimeCounter;
			yield return new WaitForSeconds(0.02f);
		}
		StopCoroutine("ChangeMusic");
	}

    /**
     * Initiates the victory screen
     */
    void PlayerVictory()
    {
        StartGameAnalytics(); // analytics

        GameObject g = GameObject.Find("HUD");
        WinMenuScript wScript = g.GetComponent<WinMenuScript>();
        wScript.WinActive = true;
    }

	private void OnDrawGizmos()
    {
        if (bouncyPath != null && bouncyPath.Length > 0)
        {
            for (int i = 1; i <= bouncePoints; i++)
            {
                Debug.DrawLine(bouncyPath[i - 1], bouncyPath[i]);
            }
        }
    }

    /**
     * Sends the selected player and level to analytics
     * 
     * Called by PlayerVictory()
     */
    void StartGameAnalytics()
    {
        string customEventName = "BossBattleWon" + Application.loadedLevelName;

        float bossBattleDuration = (Time.timeSinceLevelLoad - GameControl.control.bossBattleStartTime);

        UnityAnalytics.CustomEvent(customEventName, new Dictionary<string, object>
        {
            { "runningTime", Time.timeSinceLevelLoad },
            { "damageTaken", GameControl.control.damageTaken },
            { "projectile1Shot", GameControl.control.projectile1Shot },
            { "projectile2Shot", GameControl.control.projectile2Shot },
            { "projectile3Shot", GameControl.control.projectile3Shot },
            { "kidsFound", GameControl.control.kidsFound },
            { "lettersFound", GameControl.control.lettersFound },
            { "enemyDefeated", GameControl.control.enemiesDefeated },
            { "bossBattleDuration", bossBattleDuration },
            { "respawns", GameControl.control.respawns },
            { "timesPaused", GameControl.control.timesPaused },
            { "pauseDuration", GameControl.control.pauseDuration },
        });
    }
}
