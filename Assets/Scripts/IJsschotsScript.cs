using UnityEngine;
using System.Collections;

public class IJsschotsScript : MonoBehaviour {

	private Vector2 pos1 = new Vector2(55.5f, 2.62f);
	private Vector2 pos2 = new Vector2(41.2f, 2.62f);
    public float speed;
	private float yPosition;

    // Use this for initialization
	void Start () 
    {
		/*
		yPosition = gameObject.transform.position.y; 
		pos1 = new Vector2 (55.5f, yPosition);
		pos2 = new Vector2 (41.2f, yPosition);
		*/
	}
	
	// Update is called once per frame
	void Update () 
    {  
		Debug.Log (yPosition);
        transform.position = Vector2.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }    
}
