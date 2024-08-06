using System.Collections;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private bool _muteFxPermanently = false;
    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }

    public EPairNumber _currentPairNumber = EPairNumber.E10Pairs;

    public static GameSettings Instance;

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

        AdsManager.Instance.bannerAds.ShowBannerAd();
    }

    public void SetPairNumber(EPairNumber number)
    {
        _currentPairNumber = number;
    }

    public EPairNumber GetPairNumber()
    {
        return _currentPairNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    public string GetTextureDirectoryName()
    {
        return "Textures/Card/";
    }

    public void MuteSoundEffectPermanently(bool muted)
    {
        _muteFxPermanently = muted;
    }

    public bool IsSoundEffectMutedPermanently() 
    {
        return _muteFxPermanently;
    }

    public void AdjustSettingsForLevel(string levelName)
    {
        switch (levelName)
        {
            case "Memory Matching Lvl1":
                SetPairNumber(EPairNumber.E10Pairs);
                break;
            case "Memory Matching Lvl2":
                SetPairNumber(EPairNumber.E15Pairs);
                break;
            case "Memory Matching Lvl3":
                SetPairNumber(EPairNumber.E20Pairs);
                break;
            default:
                SetPairNumber(EPairNumber.NotSet);
                break;
        }
    }
}
