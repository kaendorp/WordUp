using UnityEngine;
using System.Collections;

public class IJstandScript : MonoBehaviour {

    public Rigidbody2D rigid;    

	// Use this for initialization
	void Start () 
    {
        rigid.gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigid.gravityScale = 0.5f;
            StartCoroutine(LifeCycle());     
        }        
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
