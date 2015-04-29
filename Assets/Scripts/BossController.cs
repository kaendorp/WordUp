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
        inactive,
        shoot,
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
    public float fireAmount = 3;
    public float cooldownTime = 1f; //amount of time (in secs.) between two shots
    private GameObject player;
    private GameObject shot;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifeTime = 2f;

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
        bossSequenceListValue = bossSequenceList.IndexOf(bossEvents.roarIdle);
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
        Debug.Log("Setting Action to : " + bossSequenceList[bossSequenceListValue].ToString());
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
        Debug.Log("IDLE DONE");
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
        Debug.Log("SHOOT DONE");
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
     * Roar action (laughs at player)
     * 
     * Triggers the roar animation and activates the circleCollider.
     * The player can then shoot the boss in the mouth.
     */
    IEnumerator Roar()
    {
        anim.SetBool("RoarIdle", true);
        circleCollider.SetActive(true);
        yield return new WaitForSeconds(3.17f);
        circleCollider.SetActive(false);
        anim.SetBool("RoarIdle", false);
        yield return new WaitForSeconds(1f);
        Debug.Log("ROAR DONE");
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
        messageObject.SetActive(true);
        // TODO: Disable player controlls without stopping time
        messageObject.GetComponent<TextMesh>().text = endMessage1;
        yield return new WaitForSeconds(4f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);
        messageObject.GetComponent<TextMesh>().text = endMessage2;
        yield return new WaitForSeconds(4f);
        messageObject.GetComponent<TextMesh>().text = "";
        yield return new WaitForSeconds(1f);


        anim.SetBool("IsDefeated", true);
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
}
