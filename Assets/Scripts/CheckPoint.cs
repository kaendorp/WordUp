using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class CheckPoint : MonoBehaviour {

    public GameMaster gameMaster;
    private bool analyticsDone = false;
    public int checkpointNumber = 0;

    // Use this for initialization
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    void Update()
    {
    }

	// Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
			gameObject.GetComponent<Animator> ().SetBool ("checked", true);
            gameMaster.currentCheckPoint = gameObject;

            if (!analyticsDone && checkpointNumber != 0)
                StartGameAnalytics();
        }
    }

    void StartGameAnalytics()
    {
        string customEventName = "Checkpoint" + checkpointNumber + Application.loadedLevelName;

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
        analyticsDone = true;
        Debug.Log(customEventName + ": Done");
    }
}
