using UnityEngine;
using System.Collections;

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
    public GameObject bossDeathFX = null;

    // MESSAGE
    [Header("MESSAGE")]
    public GameObject messageObject;        // TextMesh object that will display our message
    public string endMessage1 = "";
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
    public int bouncePoints = 4;
    public float bounceDistance = 2f;
    public float bounceHeight = 2f;
    public float bounceSpeed = 1f;
    public float bounceFirepointHeight = -1f;
    private bool goingDown = true;          // bouncy projectile going down?
    private Vector2[] bouncyPath;

    // ROAR ATTACK
    [Header("ROAR")]
    private bool wasHit = false;

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
    public void setPlayerObject(GameObject passedObject)
    {
        player = passedObject;
    }

    /**
     * Makes the boss roar at the start of the battle.
     * 
     * Also sets the currenthealth to the set StartHealth
     */
    public void beginBossBattle()
    {
        isActive = true;
        currentHealth = startHealth;
    }

    /**
     * Every action calls this method at the end to initiate the next action
     * according to the selected bossSequence.
     * 
     * It also ends the coroutineStarted check so it will start the next
     * action in Update()
     */
    private void setNextAction()
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
        setNextAction();
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
            FireProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }

        anim.SetBool("IsShooting", false);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        //Debug.Log("SHOOT DONE");
        setNextAction();
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
        Vector3 firepointResetlocation = firePoint.transform.position;

        for (int fired = 0; fired < fireAmount; fired++)
        {
            if (firePointUp)
            {
                firePoint.transform.position = firepointResetlocation;
                goingDown = true;
                firePointUp = false;
            }
            else
            {
                firePoint.transform.position = new Vector2(firePoint.transform.position.x, firePoint.transform.position.y + bounceFirepointHeight);
                goingDown = false;
                firePointUp = true;
            }

            BounceProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }

        anim.SetBool("IsShooting", false);
        yield return new WaitForSeconds(1f); // Wait for the animation to end
        //Debug.Log("SHOOT DONE");

        firePoint.transform.position = firepointResetlocation;
        setNextAction();
    }

    private void BounceProjectile()
    {
        shot = (GameObject)Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        SetBouncyPath();
        Vector2[] savedPath = bouncyPath; // might change during fire
        StartCoroutine(MoveToBouncePoint(shot, savedPath, 1));
        Destroy(shot, projectileLifeTime);
    }

    /**
     * Set the path the bouncing projectile will make.
     * 
     * Y position of the boss is set to bottom. Provided the boss is standing on the floor this will give us the "floor" height.
     */
    private void SetBouncyPath()
    {
        Vector2 bouncy1 = firePoint.transform.position;
        bouncyPath = new Vector2[bouncePoints+1];

        bouncyPath[0] = bouncy1;

        for (int i = 1; i <= bouncePoints; i++)
        {
            Vector2 bouncyI;
            if (goingDown)
            {
                bouncyI = new Vector2((bouncyPath[i-1].x - bounceDistance), this.transform.position.y);
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
            shot.transform.position = Vector3.Lerp(startposition, savedPath[bouncePoint], (elapsedTime/bounceSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bouncePoint <= bouncePoints || shot != null)
        {
            StartCoroutine(MoveToBouncePoint(shot, savedPath, bouncePoint + 1));
        }
        else
        {
            Destroy(shot);
        }
    }

    /**
     * Roar attack (laughs at player)
     * 
     * Triggers the roar animation and activates the circleCollider.
     * The player can then shoot the boss in the mouth.
     */
    IEnumerator RoarAttack()
    {
        if (wasHit)
        {
            wasHit = false;
            anim.SetBool("IsHit", false);
            anim.SetBool("RoarIdle", true);
            yield return new WaitForSeconds(1f);
            StartCoroutine(PushPlayerLeft());
            yield return new WaitForSeconds(2.17f);
            anim.SetBool("RoarIdle", false);
            yield return new WaitForSeconds(1f);
            //Debug.Log("ROAR DONE");
        }
        setNextAction();
    }

    IEnumerator PushPlayerLeft()
    {
        //player.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x - 100, player.transform.position.y));
        float elapsedTime = 0f;

        Vector2 moveToLocation = new Vector2(player.transform.position.x - 5, player.transform.position.y);

        while (elapsedTime < 1)
        {
            //player.transform.position = Vector3.Lerp(player.transform.position, moveToLocation, (elapsedTime / 2));
            player.GetComponent<Rigidbody2D>().AddForce(transform.right * -800);
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
        anim.SetBool("RoarIdle", true);
        yield return new WaitForSeconds(1f);
        circleCollider.SetActive(true);
        yield return new WaitForSeconds(2.17f);
        circleCollider.SetActive(false);
        anim.SetBool("RoarIdle", false);
        yield return new WaitForSeconds(1f);
        //Debug.Log("ROAR DONE");
        setNextAction();
    }

    /**
     * Boss is hit by player.
     * 
     * Called by BossCirlceCollider.OnCollisionEnter2D() 
     */
    public void hitByPlayerProjectile()
    {
        circleCollider.SetActive(false);
        anim.SetBool("IsHit", true);
        currentHealth--;
        wasHit = true; // can trigger roarAttack

        if (currentHealth <= 0)
        {
            StartCoroutine(Defeated());
            isActive = false; // Makes sure the next action isn't accidentally called in Update()
        }
    }

    /**
     * Called by hitByPlayerProjectile() when the boss ran out of health.
     */
    IEnumerator Defeated()
    {
        // TODO: Disable player controlls without stopping time
        anim.SetBool("IsDefeated", true);

        messageObject.SetActive(true);
        messageObject.GetComponent<TextMesh>().text = endMessage1;
        yield return new WaitForSeconds(4f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);
        messageObject.GetComponent<TextMesh>().text = endMessage2;
        yield return new WaitForSeconds(4f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("StandAndFlee");
        
        yield return new WaitForSeconds(1.1f);
        Instantiate(bossDeathFX);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        PlayerVictory();
    }

    /**
     * Initiates the victory screen
     */
    void PlayerVictory()
    {
        GameObject g = GameObject.Find("HUD");
        WinMenuScript wScript = g.GetComponent<WinMenuScript>();
        wScript.WinActive = true;
    }

    private void OnDrawGizmos()
    {
        if (bouncyPath != null && bouncyPath.Length > 0)
        {
            for (int i = 1; i < bouncePoints; i++)
            {
                Debug.DrawLine(bouncyPath[i - 1], bouncyPath[i]);
            }
        }
    }
}
