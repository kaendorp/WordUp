using UnityEngine;
using System.Collections;

public class IcarusScript : MonoBehaviour 
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameControl.control.icarusFall = true;
        }
    }
}
