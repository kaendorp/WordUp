using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour
{
    /**
     * This script contains the API call that initializes the Unity 
     * Analytics SDK and begins the data collection process. You can 
     * attach this script to any GameObject in any Scene in your game to 
     * initialize the Unity Analytics SDK. 
     */
    void Start()
    {
        // TODO: don't save projectId in plaintext on github
        const string projectId = "24cef1e3-ecd8-42a0-ac21-ac6f3a37bd36";
        UnityAnalytics.StartSDK(projectId);
    }
}
