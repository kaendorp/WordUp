using UnityEngine;
using System.Collections;

public class ShieldResponse : MonoBehaviour {

    PlatformerCharacter2D character;

	// Use this for initialization
	void Start () {
        character = GetComponentInParent<PlatformerCharacter2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            character.ShieldCoolingDown = true;
        }
    }
}
