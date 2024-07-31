using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnitId;
    [SerializeField] string iosAdUnitId;
    private string adUnitId;
 
    public void Awake()
    {
    #if UNITY_IOS
            adUnitId = iosAdUnitId;
    #elif UNITY_ANDROID
            adUnitId = androidAdUnitId;
    #elif UNITY_EDITOR
            adUnitId = androidAdUnitId; //Only for testing the functionality in the Editor
    #endif
    }

    public void LoadInterstitialAd()
    {
        ShowInterstitialAd();
        Advertisement.Load(adUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        Advertisement.Show(adUnitId, this);
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    #endregion

    #region ShowCallbacks

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Interstitial Ad Completed");
    }
    #endregion
}
