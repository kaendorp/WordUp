using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour {

    public GameObject boss;
    private Animator bossAnim;
    public GameObject player;
    private Animator playerAnim;

    public RectTransform healthTransform; // healthbar in UI
    public Image visualHealth; // healthbar image to change color
    public int currentHealth, maxHealth;

    private float cachedY; // saved Y position, does not change
    private float minXValue; // minimal X position of healthbar
    private float maxXValue; // maximal X position of healthbar

    private IntroState _state = IntroState.woordenZijnZwak;

    private bool waitCoroutineStarted = false;
    private bool waitOver = false;

    private float step;
    private float speed = 1;
    private bool bossMoveOut = false;
    private Vector3 targetposition;

    // Bericht
    public string message = "";             // The message the fiendly will display when the player gets close
    public GameObject messageObject;        // TextMesh object that will display our message

    private enum IntroState
    {
        woordenZijnZwak,
        niemandKanJeHelpen,
        bossVertrekt,
        kindRed
    }

    public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    public GUITexture overlay;
    
    void Awake ()
    {
        // Set the texture so that it is the the size of the screen and covers it.
        overlay.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

        StartCoroutine(FadeToClear());
    }
    
    IEnumerator FadeToClear ()
    {
        // Lerp the colour of the texture between itself and transparent.
        float rate = 1f / fadeSpeed;
        float progress = 0f;
        while (progress < 1f)
        {
            overlay.color = Color.Lerp(overlay.color, Color.clear, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }
        overlay.color = Color.clear;
    }
    
    
    void FadeToBlack ()
    {
        // Lerp the colour of the texture between itself and black.
        overlay.color = Color.Lerp(overlay.color, Color.black, fadeSpeed * Time.deltaTime);
    }

	// Use this for initialization
	void Start () {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;

        float currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);

        healthTransform.position = new Vector3(currentXValue, cachedY);

        visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);


        bossAnim = boss.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("Knockback", true);

        targetposition = new Vector3(boss.transform.position.x + 5, boss.transform.position.y, boss.transform.position.z);
	}

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (bossMoveOut)
            step = speed * Time.deltaTime;
        switch (_state)
        {
            case IntroState.woordenZijnZwak:
                if (!waitCoroutineStarted)
                    StartCoroutine(WoordenZijnZwak());
                break;
            case IntroState.bossVertrekt:
                BossMoveOut();
                break;
            case IntroState.kindRed:
                break;
        }
	
	}

    IEnumerator Wait(int wait)
    {
        waitCoroutineStarted = true;
        waitOver = false;
        yield return new WaitForSeconds(wait);
        waitOver = true;
        waitCoroutineStarted = false;
    }

    IEnumerator WoordenZijnZwak()
    {
        waitCoroutineStarted = true;
        yield return new WaitForSeconds(5);
        bossAnim.SetTrigger("introRoarIdle");
        messageObject.GetComponent<TextMesh>().text = "Je woorden zijn zwak,\nniets wat je zegt\nkan mij raken!";
        yield return new WaitForSeconds(5);
        bossAnim.SetTrigger("introRoarIdle");
        messageObject.GetComponent<TextMesh>().text = "Niemand hier kan je helpen,\nhahaha!";
        yield return new WaitForSeconds(5);
        messageObject.GetComponent<TextMesh>().text = "";

        Vector3 theScale = boss.transform.localScale;
        theScale.x *= -1;
        boss.transform.localScale = theScale;
        yield return new WaitForSeconds(1);
        bossMoveOut = true;
        _state = IntroState.bossVertrekt;
        waitCoroutineStarted = false;
    }

    void BossMoveOut()
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetposition, step);

        if (boss.transform.position == targetposition)
        {
            bossMoveOut = false;
            _state = IntroState.kindRed;
        }
    }

}
