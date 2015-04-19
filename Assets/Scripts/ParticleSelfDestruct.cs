using UnityEngine;
using System.Collections;

public class ParticleSelfDestruct : MonoBehaviour {
    public float lifeTime = 2f;

	// Use this for initialization
	void Start () {
        StartCoroutine(SelfDestruct());	
	}
	
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }
}
