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
    public List<string> namen = new List<string>();

    //Voor TestScene    
    public GameObject UIFB_IsLoggedIn;
    public GameObject UIFB_IsNotLoggedIn;    
    private List<object> dataList = null;
    void Awake()
    {
        // Creerd FBControl als deze er niet is en vangt af als hij er wel al is
        if (fbControl == null)
        {
            DontDestroyOnLoad(gameObject);
            fbControl = this;
        }
        else if (fbControl != this)
        {
            Destroy(gameObject);
        }   
    }

    //Bottom of code (last method), aangeroepen, (eerst ervoor zorgen dat facebook geinitialized is en de user dus ingelogged is);
    void AchievementCalls()
    {        
        //Op deze manier moeten de classes worden aangeroepen:
        GiveOneAchievement("http://wordupgame.tk/Facebook/Html/Achievements/A_Warmte.html".ToString());
        //Wait a few seconds before calling the GET if wished to instantiate right after eachother
        GetOneAchievement(951155231582815, "http://wordupgame.tk/Facebook/Html/Achievements/A_Warmte.html".ToString());
        GetAllAchievements();
        GetAllAppAchievements();
    }

    //ACHIEVEMENT METHODS (Player):

    //Give the player ONE achievement
    public void GiveOneAchievement(string achievementUrl)
    {
        var dict = new Dictionary<string, string>();        
        dict["achievement"] = achievementUrl;
        FB.API(FB.UserId + "/achievements", Facebook.HttpMethod.POST, null, dict);
    }
    //GET ONE achievement
    void GetOneAchievement(long achievementId, string achievementUrl)
    {
        var dict = new Dictionary<string, string>() { { "achievement", achievementUrl } };        
        FB.API(FB.UserId + "/achievements/" + achievementId, Facebook.HttpMethod.GET, HandleGetAchievement, dict);
    }
    //GET ALL achievements
    public void GetAllAchievements()
    {
        FB.API(FB.UserId + "/achievements", Facebook.HttpMethod.GET, HandleGetAchievement);
    }

    //GET ALL achievements of APP
    public void GetAllAppAchievements()
    {
        FB.API(FB.AppId + "/achievements", Facebook.HttpMethod.GET, HandleGetAllAppAchievements);
    }

    //HANDLE METHOD: GET ONE achievement, GET ALL achievements - PLAYER
    void HandleGetAchievement(FBResult result)
    {
        if (result != null)
        {
            //Deserialize JSON result into usable data
            dataList = FBUtil.DeserializeScores(result.Text);
            foreach (object dataInstance in dataList)
            {
                var entry = (Dictionary<string, object>)dataInstance;
                var data = (Dictionary<string, object>)entry["data"];

                var achievements = (Dictionary<string, object>)data["achievement"];
                
                namen.Add(achievements["title"].ToString());
                Debug.Log(achievements["title"].ToString());
            }
        }
    }
    //HANDLE METHOD: GET ALL achievements - APP
    void HandleGetAllAppAchievements(FBResult result)
    {
        //Deserialize JSON result into usable data
        if (result != null)
        {
            dataList = FBUtil.DeserializeScores(result.Text);
            foreach(object dataInstance in dataList)
            {
                var entry = (Dictionary<string, object>)dataInstance;           
  
                List<object> images = entry["image"] as List<object>;
                foreach (object image in images)
                {
                    var imageEntry = (Dictionary<string, object>)image;                    
                }
            }
        }
    }

 
}
