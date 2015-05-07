using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TestMessage : MonoBehaviour {

    //Id of messagepoint (where you obtain the message)
    public int retrievalNr = 1;
    //Facebook Id > Name of user, or when not logged in a give name
    public string user ="3";
    //Message
    public string message = "Hoi";
    //text on button
    private string btnText = "Submit Message";
   
    //resultaat van query
    public string result;
   
    //get messages - tijdelijk
    public int retrievalNrget = 1;

    public void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 200, 50), btnText))
        {
            StartCoroutine(submitMessage(1, user, message));
        }

    }

    private IEnumerator submitMessage(int retrievalNr, string user, string message)
    {
        Debug.Log(user);
        Debug.Log("test");

        string url = ("http://wordupgame.tk/insert_message.php?RetrievalNr=" + retrievalNr + "&User=" + user + "&Message=" + message);
        Debug.Log(url);
        WWW webRequest = new WWW(url);
        yield return webRequest;
        StartCoroutine( retrieveMessages(retrievalNrget));
    }

    void Start()
    {
        message = "hoi";
        user = "3";
    }

    IEnumerator retrieveMessages(int retrievalNrget)
    {

        Debug.Log("1");
        WWW webRequest = new WWW("http://wordupgame.tk/get_message.php?RetrievalNr=" + retrievalNr);
        yield return webRequest;

        string[] lines = webRequest.text.Split('\n');

        foreach (string line in lines) 
        {
            result = line;
            Debug.Log(result);
        }
    }
}
