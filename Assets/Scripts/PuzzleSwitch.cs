using UnityEngine;
using System.Collections;
using UnitySampleAssets._2D;

public class PuzzleSwitch : MonoBehaviour
{
    public GameObject puzzleWall;
    public GameObject player;
    public GameObject player2;
    public bool canDestroy = false;
    public Camera2DFollow cameraSwitch;

	//wall slide sound
	private AudioSource _audioSource;
	public AudioClip wallUp;
	public AudioClip pressurePlate;

    // Use this for initialization
    void Start()
    {
		_audioSource = gameObject.GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        player2 = GameObject.Find("Player2");
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "puzzleBlock")
        {
			_audioSource.clip = pressurePlate;
			_audioSource.Play ();
            foreach (Transform puzzleChild in puzzleWall.transform)
            {
                StartCoroutine(DestroyWallBlock(puzzleChild));
            }
        }
    }

    IEnumerator DestroyWallBlock(Transform puzzleChild)
    {
		cameraSwitch.target = puzzleChild;
        yield return new WaitForSeconds(2f);
		_audioSource.clip = wallUp;
		_audioSource.Play ();
        puzzleChild.gameObject.SetActive(false); //active is depricated
        
        if(player)
			cameraSwitch.target = player.transform;

        else if(player2)
			cameraSwitch.target = player2.transform;
    }
}
