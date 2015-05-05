using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BerichtController : MonoBehaviour {
    // Target (usually the player)
    public string targetLayer = "Player";
    
    // Bericht
    public string message = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh

	private AudioClip _audioSource;
	private Vector3 position;
	private bool isPlayed;
	// Use this for initialization
	void Start () {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");

		_audioSource = this.GetComponent<AudioSource> ().clip;
		position = this.transform.position;
		isPlayed = false;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && isPlayed == false) 
		{
			AudioSource.PlayClipAtPoint (_audioSource, position);
			isPlayed = true;
		}
	}

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = message;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = "";
			isPlayed = false;
        }
    }
}
