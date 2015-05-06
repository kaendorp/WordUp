using UnityEngine;
using System.Collections;

public class BitsToSpeech : MonoBehaviour {
	public string input;
	public AudioClip number1;
	public AudioClip number2;
	public AudioClip number3;
	public AudioClip number4;

	public float speed;

	private byte[] low;
	private Vector3 positie;

	// Use this for initialization
	void Start () {
		StartCoroutine(PlaySound (input));
		positie = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlaySound(string input)
	{
		low = System.Text.Encoding.UTF8.GetBytes(input);
		foreach(byte b in low)
		{
			//Debug.Log (b);
			if(b < 65)
			{
				AudioSource.PlayClipAtPoint(number1, positie);
				yield return new WaitForSeconds(speed);
			}
			else if(b > 65 && b < 105)
			{
				AudioSource.PlayClipAtPoint(number2, positie);
				yield return new WaitForSeconds(speed);
			}
			else if(b > 105 && b < 115)
			{	
				AudioSource.PlayClipAtPoint(number3, positie);
				yield return new WaitForSeconds(speed);
			}
			else
			{	
				AudioSource.PlayClipAtPoint(number4, positie);
				yield return new WaitForSeconds(speed);
			}
		}

	}
}
