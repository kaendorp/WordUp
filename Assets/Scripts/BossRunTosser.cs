using UnityEngine;
using System.Collections;

public class BossRunTosser : MonoBehaviour {

    public GameObject[] runners;
    public float step;
    public float speed = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine(FlingObjects());
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        
	}

    IEnumerator FlingObjects()
    {
        foreach (GameObject runner in runners)
        {
            transform.position = Vector3.MoveTowards(
                runner.transform.position, 
                new Vector3(runner.transform.position.x, runner.transform.position.y, runner.transform.position.z),
                step);
            yield return null;
        }
    }
}
