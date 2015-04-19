﻿using UnityEngine;
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
    private int checkHealth;      
    
    // Use this for initialization
    void Start()
    {
        // Normaal gezien is een bericht een enkele regel
        // hiermee wordt een newline ge-escaped
        message = message.Replace("\\n", "\n");

        healthBonus = false;
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

            
            Player player = collision.gameObject.GetComponent<Player>();
            int checkHealth = player.currentHealth;

            if (checkHealth < 10)
            {
                // Als kind nog niet is gevonden
                if (healthBonus == false)
                {
                    //Debug.Log("FOUND");
                    healthBonus = true;

                    Player.kindPlus = true;
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
