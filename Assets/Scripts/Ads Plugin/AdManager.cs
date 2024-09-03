using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
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

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        // Request configuration (customize based on your SDK version)
        RequestConfiguration requestConfiguration = new RequestConfiguration()
        {
            TagForChildDirectedTreatment = TagForChildDirectedTreatment.True,
            MaxAdContentRating = MaxAdContentRating.G
        };

        MobileAds.SetRequestConfiguration(requestConfiguration);

        RequestBanner();
        RequestInterstitial();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Banner ads
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2230857876088433/6554913133";
#elif UNITY_IPHONE
        string adUnitId = "";
#else
        string adUnitId = "unexpected_platform";
#endif

        Debug.Log("Creating banner view");

        if (_bannerView != null)
        {
            DestroyAd();
        }

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);

        AdRequest adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

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
        if (_bannerView == null)
        {
            Debug.Log("Creating banner view");

            if (_bannerView != null)
            {
                DestroyAd();
            }

            _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    // Interstitial Ads
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2230857876088433/2288365060";
#elif UNITY_IPHONE
        string adUnitId = "";
#else
        string adUnitId = "unexpected_platform";
#endif

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        AdRequest adRequest = new AdRequest();

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

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load an ad with error: " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response: " + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

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
            LoadInterstitialAd();
        }
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");
            LoadInterstitialAd();
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
            LoadInterstitialAd();
        };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RequestBanner();
        RequestInterstitial();
        LoadAdsForCurrentScene();
    }

    private void LoadAdsForCurrentScene()
    {
        LoadAd();
        LoadInterstitialAd();
    }
}
