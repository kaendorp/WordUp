using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class CheckPoint : MonoBehaviour {

    public GameMaster gameMaster;
    public Player player;
    private bool analyticsDone = false;
    public int checkpointNumber = 0;
    public int lettersNeeded;
    string amountText;
    public string message = "";
    public GameObject messageObject;  

    // Use this for initialization
    void Start()
    {
        message = message.Replace("\\n", "\n");
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
        messageObject = transform.FindChild("CheckpointMessage").gameObject;
    }

    void Update()
    {
        if(gameMaster.currentCheckPoint != gameObject)
        gameObject.GetComponent<Animator>().SetBool("checked", false);
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

            if (player.countLetters < lettersNeeded)
            {

                int verschilLetters = (lettersNeeded - player.countLetters);

                if (verschilLetters != 1)
                {
                    amountText = verschilLetters.ToString();
                    message = amountText + " Letters gemist...";
                    messageObject.GetComponent<TextMesh>().text = message;
                }
                else
                {
                    amountText = verschilLetters.ToString();
                    message = amountText + " Letter gemist...";
                    messageObject.GetComponent<TextMesh>().text = message;
                }

            }
            else
            {
                message = "";
                messageObject.GetComponent<TextMesh>().text = message;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            message = "";
            messageObject.GetComponent<TextMesh>().text = message;
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
