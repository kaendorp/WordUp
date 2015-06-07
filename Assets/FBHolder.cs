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

    bool doubleCheck;

    public GameObject playerSelect;
    public GameObject levelSelect;
    public GameObject achievementView;
    public GameObject FbContainer;

    void Awake()
    {
        //Initialize Facebook(SetInit = method after initialization, Onhideunity = method if unity is hidden)
        FB.Init(SetInit, OnHideUnity);        
    }


    void Update()
    {
        //Update function to turn main menu off and on, on certain points in the navigation
        if (FB.IsLoggedIn && doubleCheck == false)
        {
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = false;  
        }
        if (playerSelect.activeInHierarchy || levelSelect.activeInHierarchy || achievementView.activeInHierarchy || FbContainer.activeInHierarchy)
        {
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = true;
            doubleCheck = true;
        }
        if (!playerSelect.activeInHierarchy || !levelSelect.activeInHierarchy || !achievementView.activeInHierarchy || !FbContainer.activeInHierarchy)
        {
            doubleCheck = false;
        }
    }

    void Start()
    {
        if (GameControl.control.FBloginClicked == true)
        {
            // Onthoud de keuze 'spelen'
            UIFB_IsNotLoggedIn.SetActive(false);
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = false;
        }
        if (GameControl.control.FBlogin == true)
        {
            // Laad de Facebook user gegevens
            UIFB_IsNotLoggedIn.SetActive(false);
            HandleFBMenus(true);

            // Skipt het LogIn met Facebook Menu
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = false;
        }
    }

    private void SetInit()
    {
        if (GameControl.control.FBloginClicked == true)
        {
            UIFB_IsNotLoggedIn.SetActive(false);
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = false;
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        //If the game is not currently being shown
        if (!isGameShown)
        {
            //Pauze game
            Time.timeScale = 0;
        }
        else
        {
            //Resume game
            Time.timeScale = 1;
        }
    }

    public void FBLogin()
    {
        //Login to facebook with SDK(permissions, callback (when login is done)) and show main menu
        FB.Login("email, public_profile, publish_actions, user_friends", AuthCallBack);
        //Find main menu and make it active
        MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
        mainMenu._mainMenuUit = false;
        HandleFBMenus(true);
    }

    void AuthCallBack(FBResult result)
    {
        // If login succeeded handle fb menus and show main menu
        if (FB.IsLoggedIn)
        {
            HandleFBMenus(true);
            MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
            mainMenu._mainMenuUit = false;
        }
        // If login fails do not show the fb menu
        else
        {
            HandleFBMenus(false);
        }
    }

    public void Play()
    {
        //Ignore facebook functions and show main menu
        UIFB_IsNotLoggedIn.SetActive(false);
        MainMenu mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu>();
        mainMenu._mainMenuUit = false;
        GameControl.control.FBloginClicked = true;
    }

    void HandleFBMenus(bool isLoggedIn)
    {
        //If user is logged in menu is shown and filled with data
        if (isLoggedIn)
        {
            UIFB_IsLoggedIn.SetActive(true);
            UIFB_IsNotLoggedIn.SetActive(false);

            //get avatar
            FB.API(FBUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);

            //get username
            FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, HandleUserName);

            // Check for Achievements            
            GameControl.control.AchievementCheck();
            GameControl.control.FBlogin = true;
        }
        else
        {
            UIFB_IsLoggedIn.SetActive(false);
            UIFB_IsNotLoggedIn.SetActive(true);
        }
    }

    void HandleAvatar(FBResult result)
    {
        //If avatar GET call results in an error, retry calling it
        if (result.Error != null)
        {            
            FB.API(FBUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);
            return;
        }
        //Get empty img of UI
        Image UserAvatar = UIFB_Avatar.GetComponent<Image>();
        //Fill this img with the correct FB profile picture
        UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128), new Vector2(0,0));
    }

    void HandleUserName(FBResult result)
    {
        if (result.Error != null)
        {           
            FB.API(FBUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, HandleAvatar);
            return;
        }

        profile = FBUtil.DeserializeJSONProfile(result.Text);
        Text UserMsg = UIFB_UserName.GetComponent<Text>();

        UserMsg.text = "Hallo, " + profile["first_name"];
    }

    public void Share()
    {
        //FB. feed popup sharing box
        FB.Feed(linkCaption: "Ik speel WordUp",
                picture: "http://wordupgame.tk/Facebook/Images/Achievements/WordUp!.jpg",
                linkName: "Speel het ook!",
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                
                );
    }

    public void InviteFriends()
    {
        //FB. apprequest popup invite box
        FB.AppRequest(message: "Dit spel is fantastisch, speel het ook!",
            title: "Nodig je vrienden uit dit spel ook te spelen"
            );
    }

    //Scores API
    public void GetScores()
    {
        //Get scores of MAX 30 of your closest friends
        FB.API("/app/scores?fields=score,user.limit(30)", Facebook.HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback(FBResult result)
    {       
        //Deserialize JSON result friends scores
        scoresList = FBUtil.DeserializeScores(result.Text);

        //Empty scorelist UI
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

            FB.API(FBUtil.GetPictureURL(user["id"].ToString(), 128, 128), Facebook.HttpMethod.GET, delegate(FBResult pictureResult)
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

    void AlternateShare()
    {
        Application.ExternalCall("InstantiateSharing");
    }

    void AlternateFriendInvite()
    {
        Application.ExternalCall("InstantiateFriendInvite");
    }

    public void AlternateShareCallback()
    {
    }
}
