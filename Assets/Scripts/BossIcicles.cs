using UnityEngine;
using System.Collections;

public class BossIcicles : MonoBehaviour {

    public GameObject[] iclicles;
    public bool triggerFall = false;
    public float lifetime = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (triggerFall) {
            triggerFall = false;

            StartCoroutine(FallIcicles());
        }
	
	}

    IEnumerator FallIcicles()
    {
        foreach (GameObject iclicle in iclicles)
        {
            if (iclicle != null)
            {
                iclicle.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }

        yield return new WaitForSeconds(lifetime);

        foreach (GameObject iclicle in iclicles)
        {
            if (iclicle != null)
            {
                iclicle.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }
}
