using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BerichtController : MonoBehaviour {
    // Target (usually the player)
    public string targetLayer = "Player";
    
    // Bericht
    public string message = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh

	// Use this for initialization
	void Start () {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");
	}

	// Update is called once per frame
	void Update () {
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
        }
    }
}
