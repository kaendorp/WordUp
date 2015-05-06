using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestMessage : MonoBehaviour {

    //Id of messagepoint (where you obtain the message)
    public int retrievalNr = 1;
    //Facebook Id > Name of user, or when not logged in a give name
    public string user;
    //Message
    public string message;
    //text on button
    private string btnText = "Submit Message";
    
    //resultaat van query
    public string result;
   
    //get messages - tijdelijk
    public int id;
    public int retrievalNrget = 1;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), btnText))
        {
            StartCoroutine(submitMessage(retrievalNr, user, message));
        }

    }

    private IEnumerator submitMessage(int retrievalNr, string user, string message)
    {
        id = UnityEngine.Random.Range(1, 10);
        WWW webRequest = new WWW("http://wordupgame.tk/insert_message.php?RetrievalNr=" + retrievalNr + "&User=" + user + "&Message=" + message);
        yield return webRequest;
        yield return retrieveMessages(id, retrievalNrget);
    }

    void Start()
    {
        StartCoroutine(retrieveMessages(id, retrievalNrget));
    }

    IEnumerator retrieveMessages(int id, int retrievalNrget)
    {
        WWW webRequest = new WWW("http://wordupgame.tk/get_message.php?Id=" + id + "&RetrievalNr" + retrievalNrget);
        yield return webRequest;

        string[] lines = webRequest.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries); //Split the response by newlines.

        foreach (string line in lines) //Parse every line
        {
            string[] parts = line.Split(',');

            int result_id = int.Parse(parts[0]);
            int result_retrievalNr = int.Parse(parts[1]);
            String result_user = parts[3].ToString();
            String result_message = parts[4].ToString();;

            result = result_id + result_retrievalNr + result_user + result_message;
            Debug.Log(result);
        }
    }
}
