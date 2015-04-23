using UnityEngine;
using System.Collections;

public class KnopScript : MonoBehaviour {

    public GameObject knop;
    public GameObject licht;

    public GameObject platform6;
    public GameObject platform7;

    private bool ingedrukt;

	// Use this for initialization
	void Start () 
    {
        ingedrukt = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Player")
        {
            if (ingedrukt == false)
            {
                StartCoroutine(ButtonPress());
            
                // Zet licht aan
                licht.GetComponent<Light>().enabled = true;
            
                // Activeer platformen            
                platform6.SetActive(true);
                platform7.SetActive(true);

                ingedrukt = true;
            }            
        }
    }

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        StartCoroutine(ButtonPress());            
    //    }        
    //}

    IEnumerator ButtonPress()
    {
        knop.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1);
        knop.GetComponent<Animator>().enabled = false;
    }
}
