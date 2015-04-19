using UnityEngine;
using System.Collections;

public class DoveController : MonoBehaviour
{
    public float verticalSpeed = 0.05f;
    public float lifeTime = 2f;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(SelfDestruct());

        if (this.transform.localScale.x < 0)
        {
            verticalSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(verticalSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }
}
