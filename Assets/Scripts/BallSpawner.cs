using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Reference to the prefab of the ball
    public Transform spawnArea; // The area where balls will spawn
    public int maxBalls = 15; // Maximum number of balls to spawn
    public float spawnInterval = 0.1f; // Time interval between each spawn
    public string[] words; // Array of pre-determined words
    public TMP_InputField inputField; // Reference to the TMP_InputField in the canvas
    public Image buttonImage; // Reference to the button's Image component
    public Sprite buttonSelectedSprite; // Sprite to change when balls are selected
    public Sprite buttonDefaultSprite; // Default sprite for the button
    public Color correctWordColor = Color.green; // Color to change the input field when a correct word is formed
    public Color incorrectWordColor = Color.red; // Color to change the input field when an incorrect word is formed
    public float incorrectWordClearDelay = 2f; // Delay before clearing the input field for an incorrect word

    private List<string> wordList = new List<string>(); // List to store pre-determined words
    private List<GameObject> spawnedBalls = new List<GameObject>(); // List to store spawned balls
    private HashSet<GameObject> clickedBalls = new HashSet<GameObject>(); // Set to track clicked balls

    void Start()
    {
        // Populate the word list from the array
        foreach (string word in words)
        {
            wordList.Add(word.ToLower()); // Convert all words to lowercase for case-insensitive comparison
        }

        // Subscribe the button click event to the CheckWord function
        Button button = buttonImage.GetComponent<Button>();
        button.onClick.AddListener(CheckWord);

        // Set the button's default sprite
        buttonImage.sprite = buttonDefaultSprite;

        StartCoroutine(SpawnBallsCoroutine());
    }

    IEnumerator SpawnBallsCoroutine()
    {
        // Choose a guaranteed meaningful word from the word list
        string guaranteedWord = wordList[Random.Range(0, wordList.Count)];

        // Spawn balls with letters from the guaranteed word
        foreach (char letter in guaranteedWord)
        {
            SpawnBallWithLetter(letter.ToString());
            yield return new WaitForSeconds(spawnInterval);
        }

        // Spawn additional balls with random letters to fill the remaining slots
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
        AddColliderToBall(ball); // Add Collider2D component to enable interaction
        spawnedBalls.Add(ball);
    }

    void SpawnBallWithRandomLetter()
    {
        string randomLetter = GetRandomLetter();
        SpawnBallWithLetter(randomLetter);
    }

    string GetRandomLetter()
{
    // Separate vowels and consonants
    List<string> vowels = new List<string>();
    List<string> consonants = new List<string>();

    foreach (string word in wordList)
    {
        foreach (char letter in word)
        {
            string letterStr = letter.ToString();
            if (IsVowel(letterStr))
            {
                vowels.Add(letterStr);
            }
            else
            {
                consonants.Add(letterStr);
            }
        }
    }

    // Choose randomly from both categories
    if (Random.value < 0.5f && vowels.Count > 0)
    {
        return vowels[Random.Range(0, vowels.Count)];
    }
    else if (consonants.Count > 0)
    {
        return consonants[Random.Range(0, consonants.Count)];
    }
    else
    {
        // If one of the lists is empty, fallback to choosing randomly from the entire word list
        return wordList[Random.Range(0, wordList.Count)].Substring(0, 1);
    }
}

bool IsVowel(string letter)
{
    // Define your own logic for determining vowels
    string vowels = "aeiou";
    return vowels.Contains(letter.ToLower());
}


    void AssignLetterToBall(GameObject ball, string letter)
    {
        if (ball != null)
        {
            TextMeshPro textMesh = ball.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = letter.ToUpper(); // Convert the letter to uppercase
            }
        }
    }

    void AddColliderToBall(GameObject ball)
    {
        if (ball != null)
        {
            ball.AddComponent<CircleCollider2D>(); // Adding CircleCollider2D for simplicity
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
            if (hitCollider != null)
            {
                GameObject hitObject = hitCollider.gameObject;
                if (!clickedBalls.Contains(hitObject) && IsBallClickable(hitObject))
                {
                    OnBallClick(hitObject);
                    clickedBalls.Add(hitObject);
                    buttonImage.sprite = buttonSelectedSprite; // Change the button sprite when balls are selected
                }
            }
        }
    }

    void OnBallClick(GameObject ball)
    {
        TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
        if (letterText != null)
        {
            inputField.text += letterText.text; // Append the clicked letter to the input field
        }
    }
    void SpawnNewBalls(int count)
    {
        int ballsToSpawn = Random.Range(4, 6); // Randomly spawn either 3 or 5 balls
        for (int i = 0; i < ballsToSpawn; i++)
        {
            SpawnBallWithRandomLetter();
        }
    }



    public void CheckWord()
    {
        string inputText = inputField.text.ToLower();

        // Check if the input word is in the word list
        if (wordList.Contains(inputText))
        {
            inputField.image.color = correctWordColor; // Change input field color to green
            DestroyClickedBalls(); // Destroy clicked balls if the word is correct
            StartCoroutine(ClearFieldAfterDelay());
        }
        else
        {
            inputField.image.color = incorrectWordColor; // Change input field color to red for incorrect word

            // Clear the input field after a brief delay for an incorrect word
            StartCoroutine(ClearFieldAfterDelay());
        }
    }

    IEnumerator ClearFieldAfterDelay()
    {
        yield return new WaitForSeconds(incorrectWordClearDelay);
        inputField.text = ""; // Clear the input field
        inputField.image.color = Color.white; // Reset input field color
        buttonImage.sprite = buttonDefaultSprite; // Reset the button sprite

        // Clear the set of clicked balls when the input field is empty
        clickedBalls.Clear();
    }

    void DestroyClickedBalls()
    {
        foreach (GameObject ball in clickedBalls)
        {
            if (ball != null && ball.CompareTag("Ball")) // Check if the object is a ball
            {
                Destroy(ball);
            }
        }
        clickedBalls.Clear(); // Clear the set of clicked balls

        // Spawn 5 new balls
        SpawnNewBalls(Random.Range(5,6));
    }


    bool IsBallClickable(GameObject ball)
    {
        return !clickedBalls.Contains(ball);
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
}
