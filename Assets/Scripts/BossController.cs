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

    public GameObject bossDeathFX = null;

    public GameObject polygonCollider;
    public GameObject circleCollider;
    private Animator anim;

    public float fireAmount = 3;

    public int health = 3;
    private int startHealth = 3;

    // SHOOT
    public float cooldownTime = 0.25f; //amount of time (in secs.) between two shots
    private GameObject player;
    private GameObject shot;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float projectileLifeTime = 2f;

    private void Awake()
    {
        //bossProjectile = GetComponent<FireBossProjectile>();
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

    public void setPlayerObject(GameObject passedObject)
    {
        player = passedObject;
    }

    private void setNextAction()
    {
        Debug.Log("Going to : " + bossSequenceList[bossSequenceListValue].ToString());
        Debug.Log("bossSequenceListValue : " + bossSequenceListValue);
        Debug.Log("Buffersize : " + bossSequenceList.size.ToString());
        coroutineStarted = false;
        currentEvent = bossSequenceList[bossSequenceListValue];
        bossSequenceListValue++;

        if (bossSequenceListValue > bossSequenceList.size)
        {
            bossSequenceListValue = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
            switch (currentEvent)
            {
                case bossEvents.inactive:
                    if (!coroutineStarted)
                    {
                        StartCoroutine(Idle());
                        coroutineStarted = true;
                    }
                    break;
                case bossEvents.shoot:
                    if (!coroutineStarted)
                    {
                        StartCoroutine(Shoot());
                        coroutineStarted = true;
                    }
                    break;
                case bossEvents.roarIdle:
                    if (!coroutineStarted)
                    {
                        StartCoroutine(Roar());
                        coroutineStarted = true;
                    }
                    break;
            }
    }

    public void beginBossBattle()
    {
        currentEvent = bossEvents.roarIdle;

        health = startHealth;
    }

    IEnumerator Idle()
    {
        circleCollider.SetActive(false);
        yield return new WaitForSeconds(1f);
        anim.SetBool("IsHit", false);
        yield return new WaitForSeconds(1f);
        setNextAction();
    }

    IEnumerator Shoot()
    {
        // Can't be hit whilst fireing
        anim.SetBool("IsHit", false);
        anim.SetBool("IsShooting", true);
        yield return new WaitForSeconds(1f);

        for (int fired = 0; fired < fireAmount; fired++)
        {
            FireProjectile();
            yield return new WaitForSeconds(cooldownTime);
        }
        anim.SetBool("IsShooting", false);
        setNextAction();
    }

    private void FireProjectile()
    {
        shot = (GameObject)Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        Vector2 force = (Vector2)player.transform.position - (Vector2)firePoint.transform.position;
        shot.GetComponent<Rigidbody2D>().AddForce(force.normalized * (projectileSpeed * 30));
        Destroy(shot, projectileLifeTime);

        Debug.DrawRay(firePoint.transform.position, force, Color.yellow);
    }

    IEnumerator Roar()
    {
        // Can't be hit whilst roaring
        anim.SetBool("IsHit", false);
        anim.SetBool("RoarIdle", true);
        circleCollider.SetActive(true);
        yield return new WaitForSeconds(3.17f);
        circleCollider.SetActive(false);
        anim.SetBool("RoarIdle", false);
        setNextAction();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "PlayerProjectile")
        {
            hitByPlayerProjectile();
        }
    }

    public void hitByPlayerProjectile()
    {
        circleCollider.SetActive(false);
        anim.SetBool("IsHit", true);
        health--;

        if (health <= 0)
        {
            circleCollider.SetActive(false);
            anim.SetBool("IsDefeated", true);
            StartCoroutine(Defeated());
            return;
        }

        bossSequenceListValue = 0;
        setNextAction();
    }

    IEnumerator Defeated()
    {
        yield return new WaitForSeconds(1.1f);
        Instantiate(bossDeathFX);
        yield return new WaitForSeconds(3f);
        PlayerVictory();
    }

    void PlayerVictory()
    {
        Destroy(gameObject);
        GameObject g = GameObject.Find("HUD");
        WinMenuScript wScript = g.GetComponent<WinMenuScript>();
        wScript.WinActive = true;
    }
}
