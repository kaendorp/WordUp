using UnityEngine;
using System.Collections;

public class GladScript : MonoBehaviour
{
    public bool glijHorizontaal;

    public bool glijLinks;
    public bool glijRechts;

	private AudioSource _audioSource;
	private bool isPlayed;
    // Use this for initialization
    void Start()
    {
		isPlayed = false;
		_audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
			_audioSource.enabled = true;
            Rigidbody2D rb;
			// Get Rigidbody
            if (GameObject.Find("Player") == null)
            {
                rb = GameObject.Find("Player2").GetComponent<Rigidbody2D>();
            }
            else
            {
                rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            }

            if (glijHorizontaal == true)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    rb.AddForce(Vector3.left * 50);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.AddForce(Vector2.right * 50);
                }
            }
            if (glijRechts == true)
            {
                rb.AddForce(Vector2.right * 50);
                rb.AddForce(Vector2.up * 10);
            }
            if (glijLinks == true)
            {
                rb.AddForce(Vector3.left * 50);
                rb.AddForce(Vector2.up * 10);
            }
        }
    }

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			_audioSource.enabled = false;
		}
	}
}
