using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Reference to the prefab of the ball
    public Transform spawnArea; // The area where balls will spawn
    public int maxBalls = 15; // Maximum number of balls to spawn
    public float spawnInterval = 0.1f; // Time interval between each spawn
    public string[] letters; // Array of letters to assign to balls
    public TMP_InputField inputField; // Reference to the TMP_InputField in the canvas

    private int ballsSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnBallsCoroutine());
    }

    IEnumerator SpawnBallsCoroutine()
    {
        while (ballsSpawned < maxBalls)
        {
            Vector3 randomPosition = GetRandomPositionInSpawnArea();
            GameObject ball = Instantiate(ballPrefab, randomPosition, Quaternion.identity);

            AssignRandomLetterToBall(ball);
            ballsSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void AssignRandomLetterToBall(GameObject ball)
    {
        if (ball != null)
        {
            TextMeshPro textMesh = ball.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null && letters.Length > 0)
            {
                int randomIndex = Random.Range(0, letters.Length);
                textMesh.text = letters[randomIndex];
            }
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
                OnBallClick(hitObject);
            }
        }
    }

    void OnBallClick(GameObject ball)
    {
        if (inputField != null)
        {
            TextMeshPro letterText = ball.GetComponentInChildren<TextMeshPro>();
            if (letterText != null)
            {
                inputField.text += letterText.text; // Append the clicked letter to the input field
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
}


















