using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BerichtGetSet : MonoBehaviour
{
    /**
     * Submits the message to the database.
     */
    public IEnumerator submitMessage(int retrievalNr, string user, string message)
    {
        string url = ("http://wordupgame.tk/insert_message.php?RetrievalNr=" + retrievalNr + "&User=" + WWW.EscapeURL(user) + "&Message=" + WWW.EscapeURL(message));
        Debug.Log(url);
        WWW webRequest = new WWW(url);
        yield return webRequest;
    }
    /**
     * Return a random message for a message gameObject by it's id
     */
    public IEnumerator RetrieveMessagesFromServer(int retrievalNr, Action<string> callback)
    {
        string result = null;
        WWW webRequest = new WWW("http://wordupgame.tk/get_message.php?RetrievalNr=" + retrievalNr);

        while (!webRequest.isDone && webRequest.progress < 0.1f) // wait for download to complete
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (webRequest.error == null)
        {
            string[] lines = webRequest.text.Split(';');            // returns an semicolon seperated array, so split

            System.Random randomMessage = new System.Random();
            int selectedMessage = randomMessage.Next(0, (lines.Length - 1));  // select a random message

            result = lines[selectedMessage];
        }
        else
        {
            result = null;
            Debug.Log("BerichtGet error: " + webRequest.error);
        }

        callback(result);       // return the message
    }
}
