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
        analyticsDone = true;
        string customEventName = "Checkpoint" + checkpointNumber + Application.loadedLevelName;

        AnalyticsResult results = UnityAnalytics.CustomEvent(customEventName, new Dictionary<string, object>
        {
            { "runningTime", Time.timeSinceLevelLoad },
            { "damageTaken", GameControl.control.damageTaken },
            { "kidsFound", GameControl.control.kidsFound },
            { "lettersFound", GameControl.control.lettersFound },
            { "enemyDefeated", GameControl.control.enemiesDefeated },
            { "respawns", GameControl.control.respawns },
             { "timesPaused", GameControl.control.timesPaused },
            { "pauseDuration", GameControl.control.pauseDuration },
        });
        

        if (results != AnalyticsResult.Ok)
            Debug.LogError("Analytics " + customEventName + ": " + results.ToString());
        else
            Debug.Log("Analytics " + customEventName + ": Done");

        // Shots
        customEventName += "Shots";

        AnalyticsResult results2 = UnityAnalytics.CustomEvent(customEventName, new Dictionary<string, object>
        {
            { "projectile1Shot", GameControl.control.projectile1Shot },
            { "projectile2Shot", GameControl.control.projectile2Shot },
            { "projectile3Shot", GameControl.control.projectile3Shot },
        });

        if (results2 != AnalyticsResult.Ok)
            Debug.LogError("Analytics " + customEventName + ": " + results.ToString());
        else
            Debug.Log("Analytics " + customEventName + ": Done");
    }
}
