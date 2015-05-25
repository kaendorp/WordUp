using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class AnalyticsCheckpoint : MonoBehaviour {

    public int checkpointNumber = 0;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
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
        });
    }
}
