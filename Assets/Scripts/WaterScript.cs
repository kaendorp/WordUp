using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour 
{
	private AudioClip _audioSource;
	private Vector3 positie;
	private bool isSplashPlayed;
	
    // Use this for initialization
	void Start () 
    {
		isSplashPlayed = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}	

	void OnTriggerExit2D(Collider2D collision)
	{
		isSplashPlayed = false;
	}

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

		if (collision.gameObject.tag == "Player") 
		{
			if(!isSplashPlayed)
			{
				AudioSource.PlayClipAtPoint (_audioSource, positie);
				isSplashPlayed = true;
			}
            GameControl.control.droogOverGevallen = true;
		}

		if (collision.gameObject.tag == "Ijstand") 
		{
			AudioSource.PlayClipAtPoint (_audioSource, positie);
		}
    }
}
