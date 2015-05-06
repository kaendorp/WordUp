using UnityEngine;
using System.Collections;

public class BerichtenMenuController : MonoBehaviour {
    private GUISkin skin;
    [TextArea(1, 2)]
    public string selectedText = "zz";
    private Rect buttonRect = new Rect(15, 15, 260, 30);

    private RectTransform window;

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
                selectedText = message;
                break;
            case 2:
                selectedText = selectedText.Replace("***", message);
                break;
        }
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
