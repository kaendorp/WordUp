using UnityEngine;
using System.Collections;

public class KlokLamp : MonoBehaviour {
	private int kloknummer;
	private int positie;

	private int klok1 = 0;
	private int klok2 = 1;
	private int klok3 = 2;

	private bool klok1_juist;
	private bool klok2_juist;
	private bool klok3_juist;

	public AudioClip _audioSource;
	public AudioClip _audioSource2;
	private Vector3 geluidspositie;

	public GameObject[] platformen;
	// Use this for initialization
	void Start () {
		klok1_juist = false;
		klok2_juist = false;
		klok3_juist = false;
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		geluidspositie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LampAan(object[] temp)
	{
		kloknummer = (int) temp[0];
		positie = (int) temp[1];
		switch (kloknummer) 
		{
			case 1:
				klok1_juist = (positie == klok1) ? true : false;
				break;
			case 2:
				klok2_juist = (positie == klok2) ? true : false;
				break;
			case 3:
				klok3_juist = (positie == klok3) ? true : false;
				break;
		}

		if (klok1_juist == true && klok2_juist == true && klok3_juist == true) 
		{
			gameObject.GetComponent<Light> ().enabled = true;
			foreach (GameObject o in platformen)
			{
				o.SetActive(true);
			}     
			AudioSource.PlayClipAtPoint (_audioSource2, geluidspositie);
		} 
		else 
		{
			gameObject.GetComponent<Light> ().enabled = false;
			foreach (GameObject o in platformen)
			{
				o.SetActive(false);
			}  
			AudioSource.PlayClipAtPoint (_audioSource, geluidspositie);
		}
	}
}
