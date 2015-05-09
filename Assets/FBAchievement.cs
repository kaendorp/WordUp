using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using UnityEngine.UI;
using Facebook.MiniJSON;
using System.Globalization;
using System.Text;

public class FBAchievement : MonoBehaviour
{
    public static FBAchievement fbControl;

    // Lijst voor achievement
    public List<string> ids = new List<string>();
    public List<string> namen = new List<string>();

    //Voor TestScene
    private string btnText = "Give achievement";
    public GameObject UIFB_IsLoggedIn;
    public GameObject UIFB_IsNotLoggedIn;
    private List<object> resultList = null;
    private List<object> dataList = null;
    void Awake()
    {
        // Creerd GameControl als deze er niet is en vangt af als hij er wel al is
        if (fbControl == null)
        {
            DontDestroyOnLoad(gameObject);
            fbControl = this;
        }
        else if (fbControl != this)
        {
            Destroy(gameObject);
        } 

        FB.Init(SetInit, OnHideUnity);        
    }

    //Bottom of code (last method), aangeroepen, (eerst ervoor zorgen dat facebook geinitialized is en de user dus ingelogged is);
    void AchievementCalls()
    {
        Debug.Log("Start");
        //Op deze manier moeten de classes worden aangeroepen:
        GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Warmte.html".ToString());
        //Wait a few seconds before calling the GET if wished to instantiate right after eachother
        GetOneAchievement(951155231582815, "http://wordupgame.tk/Facebook/Html/Achievements/A_Warmte.html".ToString());
        GetAllAchievements();
        GetAllAppAchievements();
    }

    //ACHIEVEMENT METHODS (Player):

    public void GiveOneAchievement(string achievementUrl)
    {
        var dict = new Dictionary<string, string>();
        Debug.Log(achievementUrl);
        dict["achievement"] = achievementUrl;
        FB.API(FB.UserId + "/achievements", Facebook.HttpMethod.POST, null, dict);
    }

    void GetOneAchievement(long achievementId, string achievementUrl)
    {
        var dict = new Dictionary<string, string>() { { "achievement", achievementUrl } };
        Debug.Log(achievementId + " " + achievementUrl);
        FB.API(FB.UserId + "/achievements/" + achievementId, Facebook.HttpMethod.GET, HandleGetAchievement, dict);
    }

    public void GetAllAchievements()
    {
        FB.API(FB.UserId + "/achievements", Facebook.HttpMethod.GET, HandleGetAchievement);
    }

    //ACHIEVEMENT METHOD APP
    public void GetAllAppAchievements()
    {
        FB.API(FB.AppId + "/achievements", Facebook.HttpMethod.GET, HandleGetAllAppAchievements);
    }

    //HANDLE METHODS
    void HandleGetAchievement(FBResult result)
    {
        if (result != null)
        {
            dataList = Util.DeserializeScores(result.Text);
            foreach (object dataInstance in dataList)
            {
                var entry = (Dictionary<string, object>)dataInstance;
                var data = (Dictionary<string, object>)entry["data"];

                var achievements = (Dictionary<string, object>)data["achievement"];

                Debug.Log(achievements["id"].ToString());
                Debug.Log(achievements["title"].ToString());
            }
        }
    }

    void HandleGetAllAppAchievements(FBResult result)
    {
        if (result != null)
        {
            dataList = Util.DeserializeScores(result.Text);
            foreach(object dataInstance in dataList)
            {
                var entry = (Dictionary<string, object>)dataInstance;

                Debug.Log(entry["id"].ToString());
                ids.Add(entry["id"].ToString());
                Debug.Log(entry["title"].ToString());
                namen.Add(entry["title"].ToString());

                string iets = string.Join(",", namen.ToArray());
                Debug.Log(iets);

                List<object> images = entry["image"] as List<object>;
                foreach (object image in images)
                {
                    var imageEntry = (Dictionary<string, object>)image;
                    Debug.Log(imageEntry["url"].ToString());
                }
            }
        }
    }

    //METHODS FOR TESTSCENE
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
        FB.Login("email, publish_actions", AuthCallBack);
    }

    void HandleFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            UIFB_IsLoggedIn.SetActive(true);
            UIFB_IsNotLoggedIn.SetActive(false);

        }
        else
        {
            UIFB_IsLoggedIn.SetActive(false);
            UIFB_IsNotLoggedIn.SetActive(true);
        }
    }


    void AuthCallBack(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB logged in");
            AchievementCalls();
        }
        else
        {
            Debug.Log("FB login failed");
        }
    }
}
