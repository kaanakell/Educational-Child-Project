using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using JetBrains.Annotations;
using System;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    private BannerView _bannerView;
    private InterstitialAd interstitial;
    public string adUnitId;
    public static AdManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        RequestBanner();
        RequestInterstitial();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Banner ads

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "";
#else
            string adUnitId = "unexpected_platform";
#endif
        /// <summary>
        /// Creates a 320x50 banner view at top of the screen.
        /// </summary>
        /// 
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);

    }

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyAd();
            }

            // Create a 320x50 banner at top of the screen
            _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    //InterstitialAds

private void RequestInterstitial()
{
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    string adUnitId = "";
#else
    string adUnitId = "unexpected_platform";
#endif

    // Clean up the old ad before loading a new one
    if (_interstitialAd != null)
    {
        _interstitialAd.Destroy();
        _interstitialAd = null;
    }

    // Create an ad request
    AdRequest adRequest = new AdRequest();

    // Load the interstitial ad
    InterstitialAd.Load(adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
    {
        if (error != null || ad == null)
        {
            Debug.LogError("Interstitial ad failed to load with error: " + error);
            return;
        }

        Debug.Log("Interstitial ad loaded successfully.");
        _interstitialAd = ad;
    });
}


    private InterstitialAd _interstitialAd;

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
 public void ShowInterstitialAd()
{
    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    {
        Debug.Log("Showing interstitial ad.");
        _interstitialAd.Show();
        RegisterReloadHandler(_interstitialAd);
    }
    else
    {
        Debug.LogError("Interstitial ad is not ready yet.");
        // Reload the ad immediately if it wasn't ready
        LoadInterstitialAd();
    }
}

private void RegisterReloadHandler(InterstitialAd interstitialAd)
{
    interstitialAd.OnAdFullScreenContentClosed += () =>
    {
        Debug.Log("Interstitial Ad full screen content closed.");
        LoadInterstitialAd(); // Reload the ad so it's ready for next time
    };
    interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
    {
        Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
        LoadInterstitialAd(); // Reload the ad in case of failure
    };
}


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    //DestroyAd();  // Hide any existing banners
    RequestBanner();
    RequestInterstitial();               // Reinitialize the ads
    LoadAdsForCurrentScene();      // Load ads for the new scene
}

private void LoadAdsForCurrentScene()
    {
        LoadAd();
        LoadInterstitialAd();
    }

}
