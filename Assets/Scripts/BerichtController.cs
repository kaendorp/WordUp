using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BerichtController : MonoBehaviour {

    public int messageKey;
    // Target (usually the player)
    public string targetLayer = "Player";
    
    // Bericht
    public string message = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh

	private AudioClip _audioSource;
	private Vector3 position;
	private bool isPlayed;

    // Zet menu's active
    private GameObject HUD;
    private Transform berichtMaker;

	// Use this for initialization
	void Start () {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");

		_audioSource = this.GetComponent<AudioSource> ().clip;
		position = this.transform.position;
		isPlayed = false;

        // HUD
        HUD = GameObject.Find("HUD");
        berichtMaker = HUD.gameObject.transform.FindChild("BerichtMaker");
        if (berichtMaker == null)
            Debug.Log("BerichtMaker not found!");

        berichtMaker.gameObject.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HUD.GetComponent<BerichtenMenuController>().GetMessagePrefab(this.gameObject);
                HUD.GetComponent<BerichtenMenuController>().berichtMakerActive = true;
            }
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
