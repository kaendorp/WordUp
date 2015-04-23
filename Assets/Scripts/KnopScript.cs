using UnityEngine;
using System.Collections;

public class KnopScript : MonoBehaviour {

    public GameObject knop;
    public GameObject[] lichten;

    public GameObject[] platformen;    

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
                foreach (GameObject o in lichten)
                {
                    o.GetComponent<Light>().enabled = true;
                }             

                // Activeer platformen
                foreach (GameObject o in platformen)
                {
                    o.SetActive(true);
                }                     

                ingedrukt = true;
            }            
        }
    }    

    IEnumerator ButtonPress()
    {
        knop.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1);
        knop.GetComponent<Animator>().enabled = false;
    }
}
