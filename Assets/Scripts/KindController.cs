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
    private bool healthBonus;
    private bool plusKind;
    private int checkHealth;
    public GameObject Stats;
    
    // Use this for initialization
    void Start()
    {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");
        
        healthBonus = false;
        plusKind = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetLayer)
        {
            messageObject.GetComponent<TextMesh>().text = message;

            if (plusKind == false)
            {
                plusKind = true;
                Stats.GetComponent<Player>().kindPlus = true;
            }
                        
            if (Stats.GetComponent<Player>().currentHealth < 10) // Als health niet vol is
            {                
                // Als kind nog niet de healthBonus gegeven heeft
                if (healthBonus == false)
                {
                    healthBonus = true;
                    Stats.GetComponent<Player>().kindhealthPlus = true;
                }
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
