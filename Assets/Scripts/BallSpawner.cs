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
    public Button hintButton;  // Reference to the hint button
    public Sprite buttonSelectedSprite;
    public Sprite buttonDefaultSprite;
    public Sprite highlightedSprite;
    public Sprite normalSprite;
    public Color correctWordColor = Color.green;
    public Color incorrectWordColor = Color.red;
    public float incorrectWordClearDelay = 2f;

    private List<string> wordList = new List<string>();
    private List<GameObject> spawnedBalls = new List<GameObject>();
    private HashSet<GameObject> clickedBalls = new HashSet<GameObject>();
    private string currentHintWord = "";  // Store the current hint word
    private List<GameObject> hintBalls = new List<GameObject>();  // Store balls for the hint word

    void Start()
    {
        foreach (string word in words)
        {
            wordList.Add(word.ToLower());
        }

        Button button = buttonImage.GetComponent<Button>();
        button.onClick.AddListener(CheckWord);
        buttonImage.sprite = buttonDefaultSprite;

        // Add listener to hint button
        hintButton.onClick.AddListener(UseHint);

        StartCoroutine(SpawnBallsCoroutine());
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

        // Change the apply button sprite based on the input field content
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
            DestroyHintedBalls();  // Destroy hint balls if correct word is formed
            StartCoroutine(ClearFieldAfterDelay());
        }
        else
        {
            inputField.image.color = incorrectWordColor;
            StartCoroutine(ClearFieldAfterDelay());
        }

        // Reset the appearance of all clicked balls
        ResetBallAppearances();
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
        hintBalls.Clear();  // Clear the hint balls list
        ResetBallAppearances();  // Ensure all balls are reset
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
        //SpawnNewBalls(5);
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
        currentHintWord = FindHintWord();
        if (!string.IsNullOrEmpty(currentHintWord))
        {
            inputField.text = "";
            // Show the hint word in the input field and highlight corresponding letters
            HighlightLettersForWord(currentHintWord);
            hintButton.GetComponentInChildren<TextMeshProUGUI>().text = "Hint: " + currentHintWord.ToUpper();
        }
    }

    void UseHint()
    {
        if (!string.IsNullOrEmpty(currentHintWord))
        {
            inputField.text = currentHintWord.ToUpper();
            buttonImage.sprite = buttonSelectedSprite;  // Change the sprite when hint is used
        }
    }


    string FindHintWord()
    {
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();
        foreach (GameObject ball in spawnedBalls)
        {
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

        foreach (string word in wordList)
        {
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

            bool canFormWord = true;
            foreach (var kvp in wordLetterCounts)
            {
                if (!letterCounts.ContainsKey(kvp.Key) || letterCounts[kvp.Key] < kvp.Value)
                {
                    canFormWord = false;
                    break;
                }
            }

            if (canFormWord)
            {
                return word;
            }
        }
        return "";
    }

    void HighlightLettersForWord(string word)
    {
        inputField.text = "";
        List<GameObject> usedBalls = new List<GameObject>();

        foreach (char letter in word)
        {
            foreach (GameObject ball in spawnedBalls)
            {
                TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
                if (letterText != null && letterText.text.ToLower() == letter.ToString() && !usedBalls.Contains(ball))
                {
                    inputField.text += letterText.text.ToUpper();
                    SetBallAppearance(ball, true);
                    usedBalls.Add(ball);
                    hintBalls.Add(ball);  // Add to hint balls list
                    break;
                }
            }
        }
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
}
