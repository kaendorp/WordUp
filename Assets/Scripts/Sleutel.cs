using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class Sleutel : MonoBehaviour {

	public GameObject deur;
	private AudioClip _audioSource;
	private Vector3 positie;

	// Use this for initialization
	void Awake () 
	{
		_audioSource = gameObject.GetComponent<AudioSource>().clip;
		positie = gameObject.transform.position;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
            StartGameAnalytics();
			AudioSource.PlayClipAtPoint (_audioSource, positie);
			Destroy (gameObject);
			deur.SendMessage("GetKey");
		}
	}

     /**
     * Sends the selected player and level to analytics
     */
    void StartGameAnalytics()
    {
        string customEventName = "Checkpoint" + this.gameObject.name.Replace(" ", "") + Application.loadedLevelName;

        UnityAnalytics.CustomEvent(customEventName, new Dictionary<string, object>
        {
            { "runningTime", Time.timeSinceLevelLoad },
            { "damageTaken", GameControl.control.damageTaken },
            { "projectile1Shot", GameControl.control.projectile1Shot },
            { "projectile2Shot", GameControl.control.projectile2Shot },
            { "projectile3Shot", GameControl.control.projectile3Shot },
            { "kidsFound", GameControl.control.kidsFound },
            { "lettersFound", GameControl.control.lettersFound },
            { "enemyDefeated", GameControl.control.enemiesDefeated },
            { "respawns", GameControl.control.respawns },
            // { "timesPaused", GameControl.control.timesPaused },
            { "pauseDuration", GameControl.control.pauseDuration },
        });

        Debug.Log(customEventName + ": Done");
    }
}
