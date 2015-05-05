using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour {

    public GameObject UIFB_IsLoggedIn;
    public GameObject UIFB_IsNotLoggedIn;
    public GameObject UIFB_Avatar;
    public GameObject UIFB_UserName;

    public Text scoresDebug;
    private List<object> scoresList = null;

    private Dictionary<string,string> profile = null;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }
    
    private void SetInit()
    {
        Debug.Log("FB Initialized");

        if (FB.IsLoggedIn)
        {
            Debug.Log("FB logged in");
            HandleFBMenus(true);
        }
        else
        {
            HandleFBMenus(false);
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBLogin()
    {
        FB.Login("email", AuthCallBack);
    }

    void AuthCallBack(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB logged in");
            HandleFBMenus(true);
        }
        else
        {
            Debug.Log("FB login failed");
            HandleFBMenus(false);
        }
    }

    void HandleFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            UIFB_IsLoggedIn.SetActive(true);
            UIFB_IsNotLoggedIn.SetActive(false);

            //get avatar
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);

            //get username
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, HandleUserName);
        }
        else
        {
            UIFB_IsLoggedIn.SetActive(false);
            UIFB_IsNotLoggedIn.SetActive(true);
        }
    }

    void HandleAvatar(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting avatar");
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);
            return;
        }

        Image UserAvatar = UIFB_Avatar.GetComponent<Image>();
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128), new Vector2(0,0));
    }

    void HandleUserName(FBResult result)
    {
        if (result.Error != null)
        {
            Debug.Log("problem with getting username");
            FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);
            return;
        }

        profile = Util.DeserializeJSONProfile(result.Text);
        Text UserMsg = UIFB_UserName.GetComponent<Text>();

        UserMsg.text = "Hallo, " + profile["first_name"];
    }

    public void Share()
    {
        FB.Feed(linkCaption: "Ik speel WordUp", 
                picture: "http://el-coyot.com.ua/calc/img/na.jpg",
                linkName: "Speel het ook!",
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                
                );
    }

    public void InviteFriends()
    {
        FB.AppRequest(message: "Dit spel is fantastisch, speel het ook!",
            title: "Nodig je vrienden uit dit spel ook te spelen"
            );
    }

    //Scores API

    public void GetScores()
    {
        FB.API("/app/scores?fields=score, user.limit(30)", Facebook.HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback(FBResult result)
    {
        Debug.Log("Scores callback: " + result.Text);
        scoresDebug.text = result.Text;

        scoresList = Util.DeserializeScores(result.Text);

        foreach (object score in scoresList)
        {
            var entry = (Dictionary<string, object>)score;
            var user = (Dictionary<string, object>)entry["user"];
            scoresDebug.text = scoresDebug.text + "UN: " + user["name"] + " - " + entry["score"] + ", ";
        }
    }   

    public void SetScore()
    {
    }
}
