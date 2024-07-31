using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
 
public class BannerAds : MonoBehaviour
{
    // For the purpose of this example, these buttons are for functionality testing:

 
    [SerializeField] BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
 
    [SerializeField] string androidAdUnitId = "Banner_Android";
    [SerializeField] string iosAdUnitId = "Banner_iOS";
    string adUnitId = null; // This will remain null for unsupported platforms.
 
    void Start()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
        
    }
 
    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        // Set the banner position:
        Advertisement.Banner.SetPosition(bannerPosition);
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(adUnitId, options);
    }

    #region LoadCallbacks
 
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");   
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
    
    }
    #endregion
 
    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
 
        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(adUnitId, options);
    }
 
    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }
    
 #region ShowCallbacks
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
 #endregion

}
