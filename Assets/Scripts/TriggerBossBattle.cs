using UnityEngine;
using System.Collections;

public class TriggerBossBattle : MonoBehaviour
{
    public GameObject defaultCamera;
    public GameObject bossCamera;
    public GameObject invWallLeft;
    public GameObject invWallRight;
    public BossController bossController;

	[Header("MUSIC")]
	public GameObject worldMusic;
	public GameObject ambience;
	public GameObject bossMusic;
	public GameObject bossIntro;
	private bool isPlayed = false;
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = worldMusic.GetComponent<AudioSource> ();
		bossMusic.SetActive (false);
		bossIntro.SetActive (false);
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            bossController.isActive = true;
            GameObject player = col.gameObject;

            // Make sure we send the parent object
            // root never returns null, if this Transform doesn't have a parent it returns itself.
            if (player.transform.root.gameObject != player)
            {
                player = player.transform.root.gameObject;
            }
            bossController.setPlayerObject(player);
            bossController.beginBossBattle();

			//fade out music, disable ambience
			StartCoroutine(ChangeMusic());
			ambience.SetActive (false);
			if(!isPlayed)
			{
				StartCoroutine(PlayMusic());
				isPlayed = true;
			}
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
			//music switch to calm ambience
			ambience.SetActive (true);
			bossMusic.SetActive (false);
			isPlayed = false;
        }
    }

	private IEnumerator PlayMusic()
	{
		//start intro
		AudioClip intro = bossIntro.GetComponent<AudioSource> ().clip;
		Vector3 position = new Vector3 (0, 0, 0);

		AudioSource.PlayClipAtPoint(intro, position, 0.1f);
		yield return new WaitForSeconds(intro.length);
		bossIntro.SetActive (false);
		//after intro start boss music
		bossMusic.SetActive(true);
	}

	//switch to epic boss music
	private IEnumerator ChangeMusic()
	{
		float fTimeCounter = 0f;
		while(!(Mathf.Approximately(fTimeCounter, 0.05f)))
		{
			fTimeCounter = Mathf.Clamp01(fTimeCounter + Time.deltaTime);
			_audioSource.volume = 0.05f - fTimeCounter;
			yield return new WaitForSeconds(0.02f);
		}
		StopCoroutine("ChangeMusic");
	}
}
