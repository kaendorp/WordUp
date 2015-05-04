using UnityEngine;
using System.Collections;

public class IJsschotsScript : MonoBehaviour {

    private Vector2 pos1 = new Vector2(55.5f, 4.62f);
    private Vector2 pos2 = new Vector2(41.2f, 4.62f);
    public float speed;

    // Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {  
        transform.position = Vector2.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }    
}
