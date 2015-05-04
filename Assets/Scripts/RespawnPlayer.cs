using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    public GameMaster gameMaster;

	// Use this for initialization
	void Start () {
        gameMaster = FindObjectOfType<GameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameMaster.Respawn();
        }
    }
}
