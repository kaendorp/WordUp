using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class KnopScript : MonoBehaviour {

    public GameObject knop;
    public GameObject lamp;
	public GameObject[] platformen;    
	public GameObject hint;
	public GameObject cameraFocus;
	public GameObject player;
	public GameObject player2;
	public Camera2DFollow cameraKnop;

	public float speed;
	private AudioClip _audioSource;
	private Vector3 positie;
	private int teller = 0;
    private bool ingedrukt;
	
	// Use this for initialization
	void Start () 
    {
        ingedrukt = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
		player = GameObject.Find("Player");
		player2 = GameObject.Find("Player2");
	}

    void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Player")
        {
            if (ingedrukt == false)
            { 
                // Zet licht aan
                lamp.GetComponent<Light>().enabled = true;
				StartCoroutine(ShowTheLight ());
				// Activeer platformen
                foreach (GameObject o in platformen)
                {
                    o.SetActive(true);
                }                     
				hint.GetComponent<Animator>().enabled = false;
                ingedrukt = true;
            }

			if (ingedrukt == true && teller == 0) 
			{
				knop.transform.Translate (0, -Time.deltaTime * 3, 0);
				AudioSource.PlayClipAtPoint (_audioSource, positie);
				teller++;
			}
        }
    }    

	IEnumerator ShowTheLight()
	{
		cameraKnop.target = cameraFocus.transform;
		yield return new WaitForSeconds(speed);

		if(player)
			cameraKnop.target = player.transform;
		
		else if(player2)
			cameraKnop.target = player2.transform;
	}
}
