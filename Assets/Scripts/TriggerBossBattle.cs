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
            bossController.setPlayerObject(col.gameObject);
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
