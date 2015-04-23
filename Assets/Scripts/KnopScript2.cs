using UnityEngine;
using System.Collections;

public class KnopScript2 : MonoBehaviour {

    public GameObject knop;
    public GameObject licht1;
    public GameObject licht2;

    public GameObject platform2;
    public GameObject platform3;
    public GameObject platform4;
    public GameObject platform5;

    private bool ingedrukt;

    // Use this for initialization
    void Start()
    {
        ingedrukt = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            if (ingedrukt == false)
            {
                StartCoroutine(ButtonPress());

                // Zet lichten aan
                licht1.GetComponent<Light>().enabled = true;
                licht2.GetComponent<Light>().enabled = true;

                // Activeer platformen
                platform2.SetActive(true);
                platform3.SetActive(true);
                platform4.SetActive(true);
                platform5.SetActive(true);
                
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
