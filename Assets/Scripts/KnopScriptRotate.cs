using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class KnopScriptRotate : MonoBehaviour {

    public GameObject knop;
    public GameObject lamp;
	public GameObject rotateMyLamp;
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

	}

    void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.tag == "Player")
        {
            if (ingedrukt == false)
            { 
                // Zet licht aan
				StartCoroutine(ShowTheLight ());
                lamp.GetComponent<Light>().enabled = true;
				rotateMyLamp.SendMessage("LampAan", true);
           
                ingedrukt = true;
				hint.GetComponent<Animator>().enabled = false;
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
