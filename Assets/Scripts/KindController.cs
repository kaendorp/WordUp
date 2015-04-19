using UnityEngine;
using System.Collections;

public class KindController : MonoBehaviour 
{
    // Target (usually the player)
    public string targetLayer = "Player";

    // Bericht
    public string message = "";         // Het bericht wat getoont moet worden, gebruik \n om een nieuwe regel te beginnen
    public GameObject messageObject;    // TextMesh    

    // Kind gevonden?
    private bool gevonden;

    // Use this for initialization
    void Start()
    {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");

        gevonden = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = message;            

            // Als kind nog niet is gevonden
            if (gevonden == false)
            {
                Debug.Log("FOUND");
                gevonden = true;

                Player.kindPlus = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {            
            // Als kind nog niet is gevonden
            if (gevonden == false)
            {
                Debug.Log("FOUND");
                gevonden = true;

                Player.kindPlus = true;
            }
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
