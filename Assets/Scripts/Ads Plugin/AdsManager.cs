using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
    public InterstitialAds interstitialAds;
    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize the ads
        InitializeAds();

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeAds()
    {
        StartCoroutine(InitializeAdsCoroutine());
    }

    private IEnumerator InitializeAdsCoroutine()
    {
        while (!Advertisement.isInitialized)
        {
            yield return null; // Wait until initialization is complete
        }

        bannerAds.LoadBanner();
        interstitialAds.LoadAd(); // Load the ad first
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Advertisement.Banner.Hide();  // Hide any existing banners
    InitializeAds();               // Reinitialize the ads
    LoadAdsForCurrentScene();      // Load ads for the new scene
}

    private void LoadAdsForCurrentScene()
    {
        bannerAds.LoadBanner();
        bannerAds.ShowBannerAd();
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the object is destroyed
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
