using UnityEngine;
using System.Collections;

public class IJstandScript : MonoBehaviour {

    public Rigidbody2D rigid;    
	private AudioClip _audioSource;
	private Vector3 positie;
	private bool isPlayed;

	// Use this for initialization
	void Start () 
    {
        rigid.gravityScale = 0;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
		isPlayed = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (isPlayed) {
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			isPlayed = false;
		}
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rigid.gravityScale = 0.5f;
            StartCoroutine(LifeCycle()); 
			isPlayed = true;
        }        
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
