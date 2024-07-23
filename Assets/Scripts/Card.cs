using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public AudioClip PressSound;
    private Material _firstMaterial;
    private Material _secondMaterial;

    private Quaternion _currentRotation;

    [HideInInspector] public bool Revealed = false;
    private CardManager _cardManager;
    private bool _clicked = false;
    private int _index;

    private AudioSource _audio;

    public void SetIndex(int id) { _index = id; }
    public int GetIndex() { return _index; }

    // Start is called before the first frame update
    void Start()
    {

        Revealed = false;
        _clicked = false;
        _cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        _currentRotation = gameObject.transform.rotation;

        _audio = GetComponent<AudioSource>();
        _audio.clip = PressSound;

        Debug.Log($"Card {gameObject.name} materials: {_firstMaterial.name}, {_secondMaterial.name}");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (_clicked == false)
        {
            _cardManager.CurrentPuzzleState = CardManager.PuzzleState.PuzzleRotateting;
            if (GameSettings.Instance.IsSoundEffectMutedPermanently() == false)
                _audio.Play();
            StartCoroutine(LoopRotation(45, false));
            _clicked = true;
        }

    }

    public void FlipBack()
    {
        if (gameObject.activeSelf)
        {
            _cardManager.CurrentPuzzleState = CardManager.PuzzleState.PuzzleRotateting;
            Revealed = false;
            if (!GameSettings.Instance.IsSoundEffectMutedPermanently())
                _audio.Play();

            // Reset material to the first material
            ApplyFirstMaterial();

            StartCoroutine(LoopRotation(45, true));
        }
    }


    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180.0f;
        const float rotSpeed1 = 90.0f;
        var startAngle = angle;
        var assigned = false;

        if (FirstMat)
        {
            while (rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                if (rot >= (startAngle - 2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;
                }
                rot += (1 * step * dir);
                yield return null;
            }
        }
        else
        {
            while (angle > 0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                angle -= (1 * step * dir);
                yield return null;
            }
        }

        gameObject.GetComponent<Transform>().rotation = _currentRotation;

        if (!FirstMat)
        {
            Revealed = true;
            ApplySecondMaterial();
            _cardManager.CheckCard();
        }
        else
        {
            _cardManager.PuzzleRevealedNumber = CardManager.RevealedState.NoRevealed;
            _cardManager.CurrentPuzzleState = CardManager.PuzzleState.CanRotate;
        }

        _clicked = false;

    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        if (mat == null)
        {
            Debug.LogError("Material parameter is null.");
            return;
        }

        _firstMaterial = new Material(mat);

        // Log the path being used
        Debug.Log($"Attempting to load texture from path: {texturePath}");

        Texture2D texture = Resources.Load<Texture2D>(texturePath);
        if (texture != null)
        {
            _firstMaterial.mainTexture = texture;
            Debug.Log("Texture loaded successfully.");
        }
        else
        {
            Debug.LogError($"Failed to load texture at path: {texturePath}. Ensure the path is correct and the texture exists.");
        }
    }


    public void SetSecondMaterial(Material mat, string texturePath)
    {
        _secondMaterial = new Material(mat);
        Texture2D texture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
        if (texture != null)
        {
            _secondMaterial.mainTexture = texture;
        }
        else
        {
            Debug.LogError("Failed to load texture: " + texturePath);
        }
    }

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMaterial;
    }

    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _secondMaterial;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCorutine());
    }

    private IEnumerator DeactivateCorutine()
    {
        Revealed = false;

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}