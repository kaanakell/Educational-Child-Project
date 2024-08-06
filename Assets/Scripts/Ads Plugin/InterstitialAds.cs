using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;
    public BannerAds bannerAds;

    private bool _isAdLoaded = false;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        Debug.Log("Loading Interstitial Ad: " + _adUnitId);
        _isAdLoaded = false; // Reset the flag before loading
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowInterstitialAd()
    {
        Debug.Log("Attempting to show Interstitial Ad: " + _adUnitId);
        if (_isAdLoaded)
        {
            Advertisement.Show(_adUnitId, this);
            Debug.Log("Interstitial Ad shown successfully: " + _adUnitId);
        }
        else
        {
            Debug.LogWarning("Interstitial Ad is not ready yet.");
            LoadAd();  // Attempt to reload if not ready
        }
    }

    // Implement Load Listener and Show Listener interface methods:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad loaded successfully: " + adUnitId);
        _isAdLoaded = true;  // Set the flag when the ad is loaded
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        _isAdLoaded = false;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _isAdLoaded = false;
    }

    public void OnUnityAdsShowStart(string adUnitId) 
    {
        Debug.Log("Ad started showing: " + adUnitId);
    }

    public void OnUnityAdsShowClick(string adUnitId) 
    {
        Debug.Log("Ad clicked: " + adUnitId);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Ad completed showing: " + adUnitId);
        // Optionally load a new ad after the current one finishes
        LoadAd();  // Preload the next ad
    }
}
