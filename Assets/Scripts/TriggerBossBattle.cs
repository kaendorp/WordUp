using UnityEngine;
using System.Collections;

public class TriggerBossBattle : MonoBehaviour
{
    public GameObject defaultCamera;
    public GameObject bossCamera;
    public GameObject invWallLeft;
    public GameObject invWallRight;
    public BossController bossController;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            bossController.isActive = true;
            GameObject player = col.gameObject;

            // Make sure we send the parent object
            // root never returns null, if this Transform doesn't have a parent it returns itself.
            if (player.transform.root.gameObject != player)
            {
                player = player.transform.root.gameObject;
            }
            bossController.setPlayerObject(player);
            bossController.beginBossBattle();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            defaultCamera.SetActive(false);
            bossCamera.SetActive(true);
            invWallLeft.SetActive(true);
            invWallRight.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            defaultCamera.SetActive(true);
            bossCamera.SetActive(false);
            invWallLeft.SetActive(false);
            invWallRight.SetActive(false);
        }
    }
}
