using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public Card CardPrefab;
    public Transform CardSpawnPosition;
    public Vector2 StartPosition = new Vector2(-2.15f, 3.62f);

    [Space]
    [Header("End Game Screen")]
    public GameObject EndGamePanel;
    public GameObject NewBestScoreText;
    public GameObject YourScoreText;
    public GameObject EndTimeText;
    public GameObject TimerObject;
    public Text bestTime;

    public CardMaterialData cardMaterialData; // Reference to ScriptableObject

    public enum GameState
    {
        NoAction,
        MovingOnPosition,
        DeletingPuzzles,
        FlipBack,
        Checking,
        GameEnd
    };

    public enum PuzzleState
    {
        PuzzleRotateting,
        CanRotate
    };

    public enum RevealedState
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    [HideInInspector]
    public PuzzleState CurrentPuzzleState;
    [HideInInspector]
    public RevealedState PuzzleRevealedNumber;

    [HideInInspector]
    public List<Card> CardList;

    private Vector2 _offset = new Vector2(1.5f, 1.52f);
    private Vector2 _offsetFor15Pairs = new Vector2(1.08f, 1.22f);
    private Vector2 _offsetFor20Pairs = new Vector2(1.08f, 1.0f);
    private Vector3 _newScaleDown = new Vector3(0.9f, 0.9f, 0.001f);

    private List<Material> _materialList = new List<Material>();
    private List<string> _texturePathList = new List<string>();
    private Material _firstMaterial;
    private string _firstTexturePath;

    private bool _initialized = false;

    private int _firstRevealedCard;
    private int _secondRevealedCard;
    private int _revealedCardNumber = 0;
    private int _cardToDestroy1;
    private int _cardToDestroy2;

    private bool _corutineStarted = false;

    private int _pairNumbers;
    private int _removedPairs;
    private Timer _gameTimer;


    // Start is called before the first frame update
    void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedCardNumber = 0;
        _firstRevealedCard = -1;
        _secondRevealedCard = -1;

        _removedPairs = 0;
        _pairNumbers = (int)GameSettings.Instance.GetPairNumber();

        _gameTimer = GameObject.Find("TimerObject").GetComponent<Timer>();

        if (cardMaterialData != null)
        {
            _firstMaterial = cardMaterialData.cardBackMaterial;
        }
        else
        {
            Debug.LogError("CardMaterialData is not assigned.");
        }

        LoadMaterials();
        int pairNumber = (int)GameSettings.Instance.GetPairNumber();
        if (pairNumber == 10)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnCardMesh(4, 5, StartPosition, _offset, false);
            MoveCard(4, 5, StartPosition, _offset);
        }
        else if (pairNumber == 15)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnCardMesh(5, 6, StartPosition, _offset, false);
            MoveCard(5, 6, StartPosition, _offsetFor15Pairs);
        }
        else if (pairNumber == 20)
        {
            CurrentGameState = GameState.MovingOnPosition;
            SpawnCardMesh(5, 8, StartPosition, _offset, true);
            MoveCard(5, 8, StartPosition, _offsetFor20Pairs);
        }

        StartCoroutine(StartLevel());

        var bestMinutes = Mathf.Floor(PlayerPrefs.GetFloat("Best Time", 0) / 60);
        var bestSeconds = Mathf.RoundToInt(PlayerPrefs.GetFloat("Best Time", 0) % 60);
        var formattedBestTime = bestMinutes.ToString("00") + ":" + bestSeconds.ToString("00");
        bestTime.text = formattedBestTime;
    }

    private IEnumerator StartLevel()
    {
        // Your logic to set up the level goes here
        //AdsManager.Instance.bannerAds.ShowBannerAd();
        AdManager.Instance.LoadAd();
        // Reveal all cards
        foreach (var card in CardList)
        {
            card.ApplySecondMaterial(); // Assuming ApplySecondMaterial reveals the card
        }
        SetAllCollidersEnabled(false);
        // Wait for a short delay
        yield return new WaitForSeconds(5f);
        SetAllCollidersEnabled(true);
        // Flip all cards back to their hidden state
        foreach (var card in CardList)
        {
            card.FlipBack();
        }
    }

    private void SetAllCollidersEnabled(bool isEnabled)
    {
        foreach (var card in CardList)
        {
            card.GetComponent<Collider>().enabled = isEnabled;
        }
    }

    public void CheckCard()
    {
        if (CurrentGameState == GameState.Checking || _revealedCardNumber >= 2)
            return;

        CurrentGameState = GameState.Checking;
        _revealedCardNumber = 0;

        for (int id = 0; id < CardList.Count; id++)
        {
            if (CardList[id].Revealed)
            {
                if (_revealedCardNumber == 0)
                {
                    _firstRevealedCard = id;
                    _revealedCardNumber++;
                    PuzzleRevealedNumber = RevealedState.OneRevealed;
                }
                else if (_revealedCardNumber == 1)
                {
                    _secondRevealedCard = id;
                    _revealedCardNumber++;
                    PuzzleRevealedNumber = RevealedState.TwoRevealed;
                }
            }
        }

        if (_revealedCardNumber == 2)
        {
            if (CardList[_firstRevealedCard].GetIndex() == CardList[_secondRevealedCard].GetIndex() && _firstRevealedCard != _secondRevealedCard)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _cardToDestroy1 = _firstRevealedCard;
                _cardToDestroy2 = _secondRevealedCard;
            }
            else
            {
                StartCoroutine(FlipBack()); // Flip back unmatched cards
            }
        }

        CurrentPuzzleState = PuzzleState.CanRotate;

        if (CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private bool isFlippingBack = false;

    private IEnumerator FlipBack()
    {
        if (isFlippingBack) yield break; // Prevent multiple executions

        isFlippingBack = true; // Set flag to true

        yield return new WaitForSeconds(0.5f); // Wait for the duration of the flip back animation

        // Flip the cards back
        if (_firstRevealedCard >= 0)
        {
            CardList[_firstRevealedCard].FlipBack();
            CardList[_firstRevealedCard].Revealed = false;
        }

        if (_secondRevealedCard >= 0)
        {
            CardList[_secondRevealedCard].FlipBack();
            CardList[_secondRevealedCard].Revealed = false;
        }

        // Reset the puzzle revealed state
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedCardNumber = 0; // Reset the revealed card number
        _firstRevealedCard = -1; // Reset the first revealed card index
        _secondRevealedCard = -1; // Reset the second revealed card index

        CurrentGameState = GameState.NoAction;

        isFlippingBack = false; // Reset flag after completion
    }
    public void DestroyCard()
    {
        PuzzleRevealedNumber = RevealedState.NoRevealed;
        _revealedCardNumber = 0; // Reset the revealed card number
        _firstRevealedCard = -1; // Reset the first revealed card index
        _secondRevealedCard = -1; // Reset the second revealed card index

        CardList[_cardToDestroy1].Deactivate();
        CardList[_cardToDestroy2].Deactivate();
        _removedPairs++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
    }

    private void LoadMaterials()
    {
        var textureDirectoryPath = GameSettings.Instance.GetTextureDirectoryName();
        var materialDirectoryPath = GameSettings.Instance.GetMaterialDirectoryName();
        var pairNumber = (int)GameSettings.Instance.GetPairNumber();
        const string matBaseName = "Card";
        var firstMaterialName = "Back";

        // Load the first material for the back of the cards
        _firstTexturePath = textureDirectoryPath + firstMaterialName;
        _firstMaterial = Resources.Load<Material>(materialDirectoryPath + firstMaterialName);

        // Load each material for the cards
        for (int index = 1; index <= pairNumber; index++)
        {
            var currentMaterialFilePath = materialDirectoryPath + matBaseName + index;
            var currentTextureFilePath = textureDirectoryPath + matBaseName + index;

            // Load material and texture
            Material material = Resources.Load<Material>(currentMaterialFilePath);
            Texture2D texture = Resources.Load<Texture2D>(currentTextureFilePath);

            if (material != null && texture != null)
            {
                // Clone the material to ensure each card gets its own instance
                Material clonedMaterial = new Material(material)
                {
                    mainTexture = texture
                };

                // Add the cloned material to the list
                _materialList.Add(clonedMaterial);
                _texturePathList.Add(currentTextureFilePath);
            }
            else
            {
                Debug.LogError($"Failed to load material or texture: {currentMaterialFilePath}, {currentTextureFilePath}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentGameState == GameState.DeletingPuzzles)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate)
            {
                DestroyCard();
                CheckGameEnd();
            }
        }

        if (CurrentGameState == GameState.FlipBack)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate && _corutineStarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }

        if (CurrentGameState == GameState.GameEnd)
        {
            if (EndGamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }

    private bool CheckGameEnd()
    {
        if (_removedPairs == _pairNumbers && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;
            _gameTimer.PauseTimer(); // Stop the timer

            // Check if the ad should be shown after every 3 games
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Memory Matching Lvl3")
            {
                AdManager.Instance.ShowInterstitialAd();
            }
            return true; // Return true when the game is ended
        }
        else
        {
            return false; // Return false when the game is not ended
        }
    }

    private void ShowEndGameInformation()
    {
        EndGamePanel.SetActive(true);
        YourScoreText.SetActive(true);
        TimerObject.SetActive(false);

        var timer = _gameTimer.elapsedTime; // Access the elapsed time variable directly
        var minutes = Mathf.Floor(timer / 60);
        var seconds = Mathf.RoundToInt(timer % 60);
        var newText = minutes.ToString("00") + ":" + seconds.ToString("00");
        Debug.Log("EndTimeText: " + EndTimeText);
        EndTimeText.GetComponent<TextMeshProUGUI>().text = newText;

        if (timer < PlayerPrefs.GetFloat("Best Time", 00))
        {
            PlayerPrefs.SetFloat("Best Time", timer);
            var bestMinutes = Mathf.Floor(PlayerPrefs.GetFloat("Best Time", 0) / 60);
            var bestSeconds = Mathf.RoundToInt(PlayerPrefs.GetFloat("Best Time", 0) % 60);
            var formattedBestTime = bestMinutes.ToString("00") + ":" + bestSeconds.ToString("00");
            bestTime.text = formattedBestTime;
        }
    }

    private void SpawnCardMesh(int rows, int columns, Vector2 Pos, Vector2 offset, bool scaleDown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var tempCard = (Card)Instantiate(CardPrefab, CardSpawnPosition.position, CardPrefab.transform.rotation);

                if (scaleDown)
                {
                    tempCard.transform.localScale = _newScaleDown;
                }

                tempCard.name = tempCard.name + 'c' + col + 'r' + row;
                CardList.Add(tempCard);
            }
        }
        ApplyTextures();
    }

    public void ApplyTextures()
    {
        var randomMatIndex = Random.Range(0, _materialList.Count);
        var AppliedTimes = new int[_materialList.Count];

        for (int i = 0; i < _materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }

        foreach (var o in CardList)
        {
            var randPrevious = randomMatIndex;
            var counter = 0;
            var forceMat = false;

            while (AppliedTimes[randomMatIndex] >= 2 || ((randPrevious == randomMatIndex) && !forceMat))
            {
                randomMatIndex = Random.Range(0, _materialList.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < _materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            randomMatIndex = j;
                            forceMat = true;
                        }
                    }

                    if (forceMat == false)
                        return;
                }
            }

            o.SetFirstMaterial(_firstMaterial, _firstTexturePath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materialList[randomMatIndex], _texturePathList[randomMatIndex]);
            o.SetIndex(randomMatIndex);
            o.Revealed = false;
            AppliedTimes[randomMatIndex] += 1;
            forceMat = false;
        }
    }

    private void MoveCard(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetPosition, CardList[index]));
                index++;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, Card obj)
    {
        var randomDis = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDis * Time.deltaTime);
            yield return null;
        }
    }
}