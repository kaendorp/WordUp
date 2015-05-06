using UnityEngine;
using System.Collections;

public class BerichtenMenuController : MonoBehaviour {
    private GUISkin skin;
    [TextArea(1, 2)]
    public string selectedText = "zz";
    [TextArea(1, 2)]
    public string setText = "";
    private Rect buttonRect = new Rect(15, 15, 260, 30);

    private RectTransform window;

    private string[] messageList = new string[7];

	// Use this for initialization
	void Start () {
        skin = Resources.Load("BerichtWoordSkin") as GUISkin;
	}

	// Update is called once per frame
	void Update () {
        window = this.gameObject.GetComponent<RectTransform>();
	}

    public void SetMessage(string message, int stage)
    {
        switch (stage)
        {
            case 0:
                messageList[0] = message;
                break;
            case 1:
                messageList[1] = messageList[0];
                break;
            case 2:
                if (message != null)
                    messageList[2] = messageList[1].Replace("***", message);
                else
                    messageList[2] = messageList[1];
                break;
            case 3:
                messageList[3] = messageList[2] + message;
                break;
            case 4:
                messageList[4] = messageList[3] + message;
                break;
            case 5:
                messageList[5] = messageList[4];
                break;
            case 6:
                if (message != null)
                    messageList[6] = messageList[5].Replace("***", message);
                else
                    messageList[6] = messageList[5];
                break;
            case 7:
                // Confirm message
                break;
        }

        selectedText = messageList[stage];
    }

    void OnGUI()
    {
        buttonRect.width = window.rect.width;
        buttonRect.x = (window.rect.width / 2) - (buttonRect.width / 2);
        //buttonRect.y = (Screen.height / 2) - (buttonRect.height / 2);

        // Set the skin to use
        GUI.skin = skin;
        float height = buttonRect.y + 6f;

        GUI.SetNextControlName("Bericht");
        //buttonRect.y += (height*i);
        GUI.Label(
            buttonRect,
            new GUIContent(selectedText)
                );
    }
}
