using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BerichtController : MonoBehaviour {

    public int messageKey;
    // Target (usually the player)
    public string targetLayer = "Player";
    
    // Bericht
    [TextArea(1, 2)]
    public string message = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh

	private AudioClip _audioSource;
	private Vector3 position;
	private bool isPlayed;

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

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && isPlayed == false) 
		{
			AudioSource.PlayClipAtPoint (_audioSource, position);
			isPlayed = true;
		}
	}

    /**
     * If the player stands in the trigger of the messageprefab and presses
     * space, it will spawn the berichtMaker menu and sends this gameobject 
     * to be processed there.
     */
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
