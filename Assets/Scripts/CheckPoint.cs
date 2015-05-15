using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public GameMaster gameMaster;

    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    void Update()
    {
    }

	// Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
			gameObject.GetComponent<Animator> ().SetBool ("checked", true);
            gameMaster.currentCheckPoint = gameObject;
        }
    }
}
