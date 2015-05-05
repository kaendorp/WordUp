using UnityEngine;
using System.Collections;

public class BossIcicles : MonoBehaviour {

    public bool triggerFall = false;
    public float lifetime = 3f;

	// Use this for initialization
	void Start () {
        foreach (Transform child in this.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerFall) {
            triggerFall = false;

            StartCoroutine(FallIcicles());
        }
	}

    /**
     * Called by BossController during stomp attack
     * 
     */
    public void TriggerIceFall()
    {
        triggerFall = true;
    }

    IEnumerator FallIcicles()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        yield return new WaitForSeconds(lifetime);
    }
}
