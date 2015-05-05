using UnityEngine;
using System.Collections;

public class BossIcicles : MonoBehaviour {

    public float lifetime = 3f;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
    void Update()
    {
        StartCoroutine(FallIcicles());
    }

    IEnumerator FallIcicles()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
