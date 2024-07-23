using System.Collections.Generic;
using Unity.VisualScripting;
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
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        // You may initialize other settings here
    }

    public void SetPairNumber(EPairNumber Number)
    {
        _currentPairNumber = Number;
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

}