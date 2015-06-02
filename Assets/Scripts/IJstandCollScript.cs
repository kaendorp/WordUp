using UnityEngine;
using System.Collections;

public class IJstandCollScript : MonoBehaviour {

    public GameObject IJstand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(IJstand);
            GameControl.control.ijsGeraakt = true;
        }        
    }    
}
