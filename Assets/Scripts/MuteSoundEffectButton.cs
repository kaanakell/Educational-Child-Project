using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSoundEffectButton : MonoBehaviour
{
    public Sprite UnMutedFxSprite;
    public Sprite MutedFxSprite;

    private Button _button;
    private SpriteState _state;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        if (_button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }

        _state = new SpriteState();

        if (UnMutedFxSprite == null || MutedFxSprite == null)
        {
            Debug.LogError("Sprites not assigned!");
            return;
        }

        if (GameSettings.Instance == null)
        {
            Debug.LogError("GameSettings.Instance is null!");
            return;
        }

        UpdateButtonSprite();
    }

    void Update()
    {
        UpdateButtonSprite();
    }

    private void UpdateButtonSprite()
    {
        if (GameSettings.Instance.IsSoundEffectMutedPermanently())
        {
            _state.pressedSprite = MutedFxSprite;
            _state.highlightedSprite = MutedFxSprite;
            _button.GetComponent<Image>().sprite = MutedFxSprite;
            MuteAllAudio(true);
        }
        else
        {
            _state.pressedSprite = UnMutedFxSprite;
            _state.highlightedSprite = UnMutedFxSprite;
            _button.GetComponent<Image>().sprite = UnMutedFxSprite;
            MuteAllAudio(false);
        }

        _button.spriteState = _state;
    }

    public void ToggleFxIcon()
    {
        if (GameSettings.Instance.IsSoundEffectMutedPermanently())
        {
            GameSettings.Instance.MuteSoundEffectPermanently(false);
        }
        else
        {
            GameSettings.Instance.MuteSoundEffectPermanently(true);
        }

        UpdateButtonSprite();
    }

    private void MuteAllAudio(bool mute)
    {
        // Mute or unmute background music
        if (MusicManager.Instance != null && MusicManager.Instance.GetComponent<AudioSource>() != null)
        {
            MusicManager.Instance.GetComponent<AudioSource>().mute = mute;
        }

        // Mute or unmute all audio sources in the scene (excluding MusicManager if you want to handle it separately)
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != MusicManager.Instance.GetComponent<AudioSource>())
            {
                audioSource.mute = mute;
            }
        }
    }
}
