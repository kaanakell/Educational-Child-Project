using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnArea;
    public int maxBalls = 15;
    public float spawnInterval = 0.1f;
    public string[] words;
    public TMP_InputField inputField;
    public Image buttonImage;
    public Button hintButton;
    public Sprite buttonSelectedSprite;
    public Sprite buttonDefaultSprite;
    public Sprite highlightedSprite;
    public Sprite normalSprite;
    public Color correctWordColor = Color.green;
    public Color incorrectWordColor = Color.red;
    public float incorrectWordClearDelay = 2f;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI starText;
    public int maxHints = 3;
    public Button heartButton;

    private List<string> wordList = new List<string>();
    private List<GameObject> spawnedBalls = new List<GameObject>();
    private HashSet<GameObject> clickedBalls = new HashSet<GameObject>();
    private List<GameObject> hintBalls = new List<GameObject>();
    private string currentHintWord = "";
    private int hintsUsed = 0;
    private int wordsFound = 0;
    private int wordsToFind = 3;
    private int sessionNumber = 1;

    public string wordsFileName = "words"; // Name of the words file in Resources (without .txt)


    void Start()
{
    LoadWordsFromFile();  // Load words from the file
    Button button = buttonImage.GetComponent<Button>();
    button.onClick.AddListener(CheckWord);
    buttonImage.sprite = buttonDefaultSprite;
    hintButton.onClick.AddListener(ShowHintWord);
    heartButton.onClick.AddListener(SpawnNewLettersIfNoWords);
    UpdateUI();
    StartCoroutine(SpawnBallsCoroutine());
}


void LoadWordsFromFile()
{
    TextAsset wordFile = Resources.Load<TextAsset>(wordsFileName);
    if (wordFile != null)
    {
        string[] wordsFromFile = wordFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in wordsFromFile)
        {
            wordList.Add(word.ToLower());
        }
    }
    else
    {
        Debug.LogError("Words file not found!");
    }
}




    IEnumerator SpawnBallsCoroutine()
    {
        string guaranteedWord = wordList[Random.Range(0, wordList.Count)];

        foreach (char letter in guaranteedWord)
        {
            SpawnBallWithLetter(letter.ToString());
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int i = 0; i < maxBalls - guaranteedWord.Length; i++)
        {
            SpawnBallWithRandomLetter();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBallWithLetter(string letter)
    {
        Vector3 randomPosition = GetRandomPositionInSpawnArea();
        GameObject ball = Instantiate(ballPrefab, randomPosition, Quaternion.identity);
        AssignLetterToBall(ball, letter);
        AddColliderToBall(ball);
        spawnedBalls.Add(ball);
    }

    void SpawnBallWithRandomLetter()
    {
        string randomLetter = GetRandomLetter();
        SpawnBallWithLetter(randomLetter);
    }

    string GetRandomLetter()
    {
        string allLetters = "abcdefghijklmnopqrstuvwxyz";
        return allLetters[Random.Range(0, allLetters.Length)].ToString();
    }

    void AssignLetterToBall(GameObject ball, string letter)
    {
        if (ball != null)
        {
            TextMeshPro textMesh = ball.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = letter.ToUpper();
            }
        }
    }

    void AddColliderToBall(GameObject ball)
    {
        if (ball != null)
        {
            ball.AddComponent<CircleCollider2D>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
            if (hitCollider != null)
            {
                GameObject hitObject = hitCollider.gameObject;
                if (IsBallClickable(hitObject))
                {
                    if (clickedBalls.Contains(hitObject))
                    {
                        ClearLetterFromInput(hitObject);
                        clickedBalls.Remove(hitObject);
                        SetBallAppearance(hitObject, false);
                    }
                    else
                    {
                        OnBallClick(hitObject);
                        clickedBalls.Add(hitObject);
                        SetBallAppearance(hitObject, true);
                    }
                }
            }
        }

        if (inputField.text.Length > 0)
        {
            buttonImage.sprite = buttonSelectedSprite;
        }
        else
        {
            buttonImage.sprite = buttonDefaultSprite;
        }
    }

    void ClearLetterFromInput(GameObject ball)
    {
        TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
        if (letterText != null)
        {
            string letter = letterText.text;
            int lastIndex = inputField.text.LastIndexOf(letter);
            if (lastIndex != -1)
            {
                inputField.text = inputField.text.Remove(lastIndex, 1);
            }
        }
    }

    void OnBallClick(GameObject ball)
    {
        TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
        if (letterText != null)
        {
            string letter = letterText.text;
            inputField.text += letter;
        }
    }

    void SetBallAppearance(GameObject ball, bool highlight)
    {
        SpriteRenderer renderer = ball.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = highlight ? highlightedSprite : normalSprite;
        }
    }

    public void CheckWord()
    {
        string inputText = inputField.text.ToLower();
        if (wordList.Contains(inputText))
        {
            inputField.image.color = correctWordColor;
            DestroyClickedBalls();
            DestroyHintedBalls();
            StartCoroutine(ClearFieldAfterDelay());
            wordsFound++;
            if (wordsFound >= wordsToFind)
            {
                StartNewSession();
            }
        }
        else
        {
            inputField.image.color = incorrectWordColor;
            StartCoroutine(ClearFieldAfterDelay());
        }

        ResetBallAppearances();
        UpdateUI();
    }

    void ResetBallAppearances()
    {
        foreach (GameObject ball in clickedBalls)
        {
            SetBallAppearance(ball, false);
        }
        clickedBalls.Clear();

        foreach (GameObject ball in hintBalls)
        {
            SetBallAppearance(ball, false);
        }
        hintBalls.Clear();
    }

    IEnumerator ClearFieldAfterDelay()
    {
        yield return new WaitForSeconds(incorrectWordClearDelay);
        inputField.text = "";
        inputField.image.color = Color.white;
        buttonImage.sprite = buttonDefaultSprite;
        clickedBalls.Clear();
        hintBalls.Clear();
        ResetBallAppearances();
    }

    void DestroyClickedBalls()
    {
        foreach (GameObject ball in clickedBalls)
        {
            if (ball != null)
            {
                Destroy(ball);
            }
        }
        clickedBalls.Clear();
        SpawnNewBalls(5);
    }

    void DestroyHintedBalls()
    {
        foreach (GameObject ball in hintBalls)
        {
            if (ball != null)
            {
                Destroy(ball);
            }
        }
        hintBalls.Clear();
    }

    void SpawnNewBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnBallWithRandomLetter();
        }
    }

    public void ShowHintWord()
{
    Debug.Log($"ShowHintWord called. hintsUsed: {hintsUsed}, maxHints: {maxHints}");

    if (hintsUsed >= maxHints)
    {
        Debug.Log("No more hints available.");
        return;
    }

    currentHintWord = FindHintWord();
    if (!string.IsNullOrEmpty(currentHintWord))
    {
        HighlightLettersForWord(currentHintWord);
        hintsUsed++;
        Debug.Log($"Hint used. New hintsUsed value: {hintsUsed}");
        UpdateUI();
    }
    else
    {
        Debug.Log("No valid hint word found.");
    }
}


    string FindHintWord()
{
    foreach (string word in wordList)
    {
        if (CanFormWord(word))
        {
            return word;
        }
    }
    return null;
}


    bool CanFormWord(string word)
{
    Dictionary<char, int> letterCounts = new Dictionary<char, int>();
    
    // Count letters in the spawned balls
    foreach (GameObject ball in spawnedBalls)
    {
        if (ball == null)
        {
            continue;
        }

        TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
        if (letterText != null)
        {
            char letter = letterText.text.ToLower()[0];
            if (letterCounts.ContainsKey(letter))
            {
                letterCounts[letter]++;
            }
            else
            {
                letterCounts[letter] = 1;
            }
        }
    }

    // Check if we can form the word with the available letters
    Dictionary<char, int> wordLetterCounts = new Dictionary<char, int>();
    foreach (char letter in word)
    {
        if (wordLetterCounts.ContainsKey(letter))
        {
            wordLetterCounts[letter]++;
        }
        else
        {
            wordLetterCounts[letter] = 1;
        }
    }

    foreach (KeyValuePair<char, int> kvp in wordLetterCounts)
    {
        char letter = kvp.Key;
        int requiredCount = kvp.Value;
        if (!letterCounts.ContainsKey(letter) || letterCounts[letter] < requiredCount)
        {
            return false;
        }
    }

    return true;
}


    void HighlightLettersForWord(string word)
{
    hintBalls.Clear();
    Dictionary<char, int> letterCounts = new Dictionary<char, int>();

    foreach (char letter in word)
    {
        if (letterCounts.ContainsKey(letter))
        {
            letterCounts[letter]++;
        }
        else
        {
            letterCounts[letter] = 1;
        }
    }

    foreach (GameObject ball in spawnedBalls)
    {
        if (ball == null)
        {
            continue;
        }

        TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
        if (letterText != null)
        {
            char letter = letterText.text.ToLower()[0];
            if (letterCounts.ContainsKey(letter) && letterCounts[letter] > 0)
            {
                hintBalls.Add(ball);
                SetBallAppearance(ball, true);
                letterCounts[letter]--;
                if (letterCounts[letter] == 0)
                {
                    letterCounts.Remove(letter);
                }
            }
        }
    }
}


    void UpdateUI()
    {
        heartText.text = $"{wordsFound}/{wordsToFind}";
        starText.text = $"{maxHints - hintsUsed}/{maxHints}";
    }

    void StartNewSession()
    {
        sessionNumber++;
        wordsFound = 0;
        hintsUsed = 0;
        wordsToFind = (sessionNumber == 1) ? 3 : sessionNumber == 2 ? 5 : 10;  // Adjust words to find based on the session
        maxBalls += sessionNumber * 5;  // Increase difficulty by increasing the number of balls
        foreach (GameObject ball in spawnedBalls)
        {
            if (ball != null)
            {
                Destroy(ball);
            }
        }
        spawnedBalls.Clear();
        StartCoroutine(SpawnBallsCoroutine());
    }

    Vector3 GetRandomPositionInSpawnArea()
    {
        Vector3 center = spawnArea.position;
        Vector3 size = spawnArea.localScale;
        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);
        return new Vector3(randomX, randomY, randomZ);
    }

    bool IsBallClickable(GameObject ball)
    {
        return ball.CompareTag("Ball");
    }

    public void SpawnNewLettersIfNoWords()
    {
        string hintWord = FindHintWord();
        if (hintWord == null)
        {
            int lettersNeeded = Random.Range(3, 6);  // Adjust the number of new letters as needed
            for (int i = 0; i < lettersNeeded; i++)
            {
                SpawnBallWithRandomLetter();
            }
        }
    }
}
