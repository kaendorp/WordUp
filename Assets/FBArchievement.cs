using System.Collections;
using System.Collections.Generic;
using Facebook;

public class FacebookAchievements
{

    void GiveAchievement(string achievementUrl)
    {
        var data = new Dictionary<string, string>() { { "achievement", achievementUrl } };
        FB.API("/me/achievements",
                Facebook.HttpMethod.POST,
                AchievementCallback,
                data);
    }

    void AchievementCallback(FBResult result)
    {
        if (result != null)
        {
            //TODO handle result
        }
    }
}
