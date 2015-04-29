using UnityEngine;
using System.Collections;

public class KlokLamp : MonoBehaviour {
	private int kloknummer;
	private float positie;

	private float klok1 = -60;
	private float klok2 = -90;
	private float klok3 = -70;

	private bool klok1_juist;
	private bool klok2_juist;
	private bool klok3_juist;
	// Use this for initialization
	void Start () {
		klok1_juist = false;
		klok2_juist = false;
		klok3_juist = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LampAan(object[] temp)
	{
		kloknummer = (int) temp[0];
		positie = (float) temp[1];
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
			gameObject.GetComponent<Light>().enabled = true;
		}
	}
}
