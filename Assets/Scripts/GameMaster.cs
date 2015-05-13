using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;
    //Default in editor
    public GameObject currentCheckPoint;
    private PlatformerCharacter2D player;
    public GameObject RespawnEffect;

    public Transform playerPrefab;
    public Transform checkPoint;
    public int spawnDelay = 1;
    public Transform spawnPrefab;


    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        player = FindObjectOfType<PlatformerCharacter2D>();
    }


    public IEnumerator RespawnPlayer()
    {
        //Debug.Log("TODO: Add waiting for spawn");
        //player.gameObject.active = false;
        yield return new WaitForSeconds(1);
        Instantiate(RespawnEffect, currentCheckPoint.transform.position, currentCheckPoint.transform.rotation);
        player.transform.position = currentCheckPoint.transform.position;
        //player.gameObject.active = true;
    }


    public void Respawn()
    {
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
