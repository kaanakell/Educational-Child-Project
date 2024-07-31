using UnityEngine;
using UnityEngine.UI;

public class MuteButtonforAnimalGame : MonoBehaviour
{
    public Sprite UnMutedFxSprite;
    public Sprite MutedFxSprite;

    private Button _button;
    private SpriteState _state;

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

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        UpdateButtonSprite();
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            UpdateButtonSprite();
        }
    }

    private void UpdateButtonSprite()
    {
        if (GameManager.Instance.IsSoundEffectMutedPermanently())
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
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        if (GameManager.Instance.IsSoundEffectMutedPermanently())
        {
            GameManager.Instance.MuteSoundEffectPermanently(false);
        }
        else
        {
            GameManager.Instance.MuteSoundEffectPermanently(true);
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
