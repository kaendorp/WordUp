using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour {

    public GameObject UIFB_IsLoggedIn;
    public GameObject UIFB_IsNotLoggedIn;
    public GameObject UIFB_Avatar;
    public GameObject UIFB_UserName;

    private List<object> scoresList = null;

    private Dictionary<string,string> profile = null;

    public GameObject ScoreEntryPanel;
    public GameObject ScoreScrollList;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }
    
    private void SetInit()
    {       
        if (FB.IsLoggedIn)
        {            
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
        FB.Login("email, publish_actions", AuthCallBack);        
    }

    void AuthCallBack(FBResult result)
    {
        if (FB.IsLoggedIn)
        {            
            HandleFBMenus(true);
        }
        else
        {            
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

            // Check for Achievements            
            GameControl.control.AchievementCheck();
            GameControl.control.FBlogin = true;
        }
        else
        {
            
        }
    }

    void HandleAvatar(FBResult result)
    {
        if (result.Error != null)
        {            
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
                picture: "http://wordupgame.tk/Facebook/Images/Achievements/WordUp!.jpg",
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
        FB.API("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback(FBResult result)
    {       
        scoresList = Util.DeserializeScores(result.Text);

        foreach (Transform item in ScoreScrollList.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    
        foreach (object score in scoresList)
        {
            var entry = (Dictionary<string, object>)score;
            var user = (Dictionary<string, object>)entry["user"];

            GameObject ScorePanel;
            ScorePanel = Instantiate(ScoreEntryPanel) as GameObject;
            ScorePanel.transform.parent = ScoreScrollList.transform;

            //get name of friend + score
            Transform ScoreName = ScorePanel.transform.Find("FriendName");
            Transform ScoreScore = ScorePanel.transform.Find("FriendScore");
            Text scoreName = ScoreName.GetComponent<Text>();
            Text scoreScore = ScoreScore.GetComponent<Text>();

            scoreName.text = user["name"].ToString();
            scoreScore.text = entry["score"].ToString();

            //get friend avatar
            Transform UserAvatar = ScorePanel.transform.Find("FriendAvatar");
            Image userAvatar = UserAvatar.GetComponent<Image>(); ;

            FB.API(Util.GetPictureURL(user["id"].ToString(), 128, 128), Facebook.HttpMethod.GET, delegate(FBResult pictureResult)
            {
                if (pictureResult.Error != null)
                {
                    Debug.Log(pictureResult.Error);
                }
                else
                {
                    userAvatar.sprite = Sprite.Create(pictureResult.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
                }
            });
        }
    }   

    public void SetScore()
    {
        var scoreData = new Dictionary<string, string>();
        
        //test: insert score
        scoreData["score"] = GameControl.control.highScore.ToString();
        
        FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult result)
        {
            Debug.Log("Score submit result: " + result.Text);
        }, scoreData);
    }
}
