using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour
{

    private enum IntroState
    {
        evilGloating,
        bossMoveOut
    }

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

    private IntroState _state = IntroState.evilGloating;

    private bool waitCoroutineStarted = false;      // Prevents the coroutine to trigger multiple times in FixedUpdate()

    private float step;
    private float speed = 0.5f;             // Speed of Silence walking away
    private bool bossMoveOut = false;
    private Vector3 targetposition;

    // Bericht
    public string message = "";             // The message the fiendly will display when the player gets close
    public GameObject messageObject;        // TextMesh object that will display our message

    // Fade in/out
    public float fadeInSpeed = 1.5f;        // Speed that the screen fades from black.
    public float fadeOutSpeed = 1.5f;       // Speed that the screen fades to black.
    public Image overlay;                   // Object in HUD that fills screen with a full alpha, black image

	[Header("VOICE")]
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;
	public float speedVoice;
	private byte[] low;
	private AudioSource bossSource;
    // Use this for initialization
    void Start()
    {
        /**
         * Taken from the hudscript, hardcoded for use in intro
         */
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;

        float currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);

        healthTransform.position = new Vector3(currentXValue, cachedY);

        visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);

        // Get animators and trigger the player 'hit' animation
        bossAnim = boss.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("IntroKnockback", true);

        // set the location where the Silence will walk to
        targetposition = new Vector3(boss.transform.position.x + 5, boss.transform.position.y, boss.transform.position.z);

        // Start the intro with a fade to clear
        StartCoroutine(FadeToClear());

		// for spoken tekst
		bossSource = gameObject.GetComponent<AudioSource>();
    }

    /**
     * Needed for hud
     */
    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bossMoveOut)
            step = speed * Time.deltaTime;
        switch (_state)
        {
            case IntroState.evilGloating:
                if (!waitCoroutineStarted)
                    StartCoroutine(EvilGloating());
                break;
            case IntroState.bossMoveOut:
                BossMoveOut();
                break;
        }

    }

    IEnumerator EvilGloating()
    {
        waitCoroutineStarted = true;
        yield return new WaitForSeconds(5);
        bossAnim.SetTrigger("IntroBossRoarIdle");
        yield return new WaitForSeconds(1);
		string endMessage1 = "Je woorden zijn zwak,\nniets wat je zegt\nkan mij raken!";
		messageObject.GetComponent<TextMesh>().text = endMessage1;
		StartCoroutine(PlaySound (endMessage1));
        yield return new WaitForSeconds(4);
        messageObject.GetComponent<TextMesh>().text = "";
        bossAnim.SetTrigger("IntroBossRoarIdle");
        yield return new WaitForSeconds(1);
		string endMessage2 = "Niemand hier kan je helpen,\nhahaha!";
        messageObject.GetComponent<TextMesh>().text = endMessage2;
		StartCoroutine(PlaySound (endMessage2));
        yield return new WaitForSeconds(5);
        messageObject.GetComponent<TextMesh>().text = "";

        Vector3 theScale = boss.transform.localScale;
        theScale.x *= -1;
        boss.transform.localScale = theScale;
        yield return new WaitForSeconds(1);
        bossMoveOut = true;
        _state = IntroState.bossMoveOut;
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(10);

        // Load the tutorial level
        Application.LoadLevel("Tutorial");
        waitCoroutineStarted = false;
    }

    void BossMoveOut()
    {
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetposition, step);

        if (boss.transform.position == targetposition)
        {
            bossMoveOut = false;
        }
    }

    IEnumerator FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        float rate = fadeInSpeed;
        float progress = 0f;
        while (progress < 1f)
        {
            overlay.color = Color.Lerp(Color.black, Color.clear, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }
        overlay.color = Color.clear;
    }

    IEnumerator FadeToBlack()
    {
        // Lerp the colour of the texture between itself and transparent.
        float rate = fadeOutSpeed;
        float progress = 0f;
        while (progress < 1f)
        {
            overlay.color = Color.Lerp(Color.clear, Color.black, progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }
        overlay.color = Color.black;
    }

	/**
     * Converts any string in message to sound
     */
	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			Debug.Log (b);
			if(b < 65)
			{
				bossSource.clip = number1;
				bossSource.volume = 0.2f;
				bossSource.Play ();
				yield return new WaitForSeconds(speedVoice);
			}
			else if(b > 65 && b < 105)
			{
				bossSource.clip = number2;
				bossSource.volume = 0.4f;
				bossSource.Play ();
				yield return new WaitForSeconds(speedVoice);
			}
			else if(b > 105 && b < 115)
			{	
				bossSource.clip = number3;
				bossSource.volume = 0.6f;
				bossSource.Play ();
				yield return new WaitForSeconds(speedVoice);
			}
			else
			{	
				bossSource.clip = number4;
				bossSource.volume = 0.8f;
				bossSource.Play ();
				yield return new WaitForSeconds(speedVoice);
			}
		}
	}
}
