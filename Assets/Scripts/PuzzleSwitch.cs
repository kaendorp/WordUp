using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class PuzzleSwitch : MonoBehaviour
{

    public GameObject puzzleWall;
    public GameObject player;
    public bool canDestroy = false;
    public Camera2DFollow camera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "puzzleBlock")
        {
            foreach (Transform puzzleChild in puzzleWall.transform)
            {
                StartCoroutine(DestroyWallBlock(puzzleChild));
            }
        }
    }

    IEnumerator DestroyWallBlock(Transform puzzleChild)
    {
        camera.target = puzzleChild;
        yield return new WaitForSeconds(2f);
        puzzleChild.gameObject.active = false;
        camera.target = player.transform;
    }
}
