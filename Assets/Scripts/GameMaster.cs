using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
    public GameObject currentCheckPoint;
    private PlatformerCharacter2D player;

	void Start() {
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			}
        player = FindObjectOfType<PlatformerCharacter2D>();
		}

	public Transform playerPrefab;
	public Transform checkPoint;  
	public int spawnDelay = 2;
	public Transform spawnPrefab;


	public IEnumerator RespawnPlayer () {
		Debug.Log ("TODO: Add waiting for spawn");
		yield return new WaitForSeconds (spawnDelay);
        player.transform.position = currentCheckPoint.transform.position;
	}

	public void Respawn ()
    {
        gm.StartCoroutine (gm.RespawnPlayer());
	}
}
