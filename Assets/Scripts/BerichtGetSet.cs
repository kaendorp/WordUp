using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BerichtGetSet : MonoBehaviour {

    //Id of messagepoint (where you obtain the message)
    //private int retrievalNr;
    //Facebook Id > Name of user, or when not logged in a give name
    //private string user;
    //Message
    //private string message;
    //text on button
    //private string btnText;
   
    //resultaat van query
    //private string result;
   
    //get messages - tijdelijk
    //private int retrievalNrget;

    //public void OnGUI()
    //{

    //    if (GUI.Button(new Rect(10, 10, 200, 50), btnText))
    //    {
    //        StartCoroutine(submitMessage(1, user, message));
    //    }
    //}

    private IEnumerator submitMessage(int retrievalNr, string user, string message)
    {
        string url = ("http://wordupgame.tk/insert_message.php?RetrievalNr=" + retrievalNr + "&User=" + user + "&Message=" + message);
        Debug.Log(url);
        WWW webRequest = new WWW(url);
        yield return webRequest;
    }

    void Start()
    {
        //message = "hoi";
        //user = "3";
    }

    public IEnumerator RetrieveMessagesFromServer(int retrievalNr, Action<string> callback)
    {
        string result = null;
        //Debug.Log("1");
        WWW webRequest = new WWW("http://wordupgame.tk/get_message.php?RetrievalNr=" + retrievalNr);
        while (!webRequest.isDone && webRequest.progress < 0.1f)
        {
            yield return new WaitForSeconds(0.2f);
        }
        string[] lines = webRequest.text.Split('\n');

        System.Random randomMessage = new System.Random();
        int selectedMessage = randomMessage.Next(0, (lines.Length - 1));

        result = lines[selectedMessage];

        if (string.IsNullOrEmpty(result))
        {
            result = lines[0];
        }

        callback(result);
    }
}
