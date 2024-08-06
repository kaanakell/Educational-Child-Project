using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.TOP_LEFT;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    private string _adUnitId;

    private void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        #if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
        #endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Load and show the banner ad
        LoadBanner();
    }

    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions loadOptions = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Set up options to notify the SDK of show events:
        BannerOptions showOptions = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Load and show the Banner Ad Unit:
        Advertisement.Banner.Load(_adUnitId, loadOptions);
        Advertisement.Banner.Show(_adUnitId, showOptions);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded successfully.");
    }

    private void OnBannerError(string message)
    {
        Debug.LogError($"Banner failed to load: {message}");
    }

    private void OnBannerClicked() 
    {
        Debug.Log("Banner clicked.");
    }

    private void OnBannerShown() 
    {
        Debug.Log("Banner shown.");
    }

    private void OnBannerHidden() 
    {
        Debug.Log("Banner hidden.");
        // Optionally, reload the banner if it's hidden unexpectedly
        LoadBanner();
    }

    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    public void ShowBannerAd()
    {
        LoadBanner();
    }
}
