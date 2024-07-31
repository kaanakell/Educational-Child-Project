using UnityEngine;
using System.Collections.Generic;

public class AnimalInstantiation : MonoBehaviour
{
    public GameObject[] animalPrefabs; // Array to hold all animal prefabs
    public GameObject row; // Reference to the row object
    public float spaceBetweenAnimals = 1.5f; // Adjust this value to control the space between animals

    private List<GameObject> instantiatedAnimals = new List<GameObject>(); // List to keep track of instantiated animals
    private int currentGroupIndex = 0; // Index to keep track of the current group of animals

    void Start()
    {
        // Shuffle the array of animal prefabs
        ShuffleArray(animalPrefabs);

        // Instantiate the first group of animals
        InstantiateNextGroup();
    }

    void Update()
{
    // Check if all animals in the current group have been matched
    if (AllAnimalsMatched())
    {
        // Check if there are more groups of animals to instantiate
        if (currentGroupIndex * 3 >= animalPrefabs.Length)
        {
            // All animals are matched, end the game
            EndGameManager.Instance.ShowEndGamePanel();
        }
        else
        {
            // Instantiate the next group of animals
            InstantiateNextGroup();
        }
    }
}


    private void InstantiateNextGroup()
{
    // Calculate the start position for the new group
    Vector3 startPosition = row.transform.position + new Vector3(0f, 0f, 0f);

    // Instantiate animals within the row, one of each type
    for (int i = 0; i < 3; i++) // Assuming each group has 3 animals
    {
        int prefabIndex = currentGroupIndex * 3 + i;
        if (prefabIndex >= animalPrefabs.Length)
        {
            break; // No more animals to instantiate
        }

        Vector3 randomPosition = startPosition + new Vector3(i * spaceBetweenAnimals, 0f, 0f);
        GameObject animalPrefab = animalPrefabs[prefabIndex];
        GameObject instantiatedAnimal = Instantiate(animalPrefab, randomPosition, Quaternion.identity, row.transform);
        instantiatedAnimals.Add(instantiatedAnimal);
    }

    currentGroupIndex++;
}


    // Function to check if all animals in the current group have been matched
    private bool AllAnimalsMatched()
    {
        foreach (GameObject animal in instantiatedAnimals)
        {
            if (animal != null && !animal.GetComponent<AnimalInteraction>().IsMatched)
                return false; // At least one animal is still not matched
        }
        return true; // All animals have been matched
    }

    // Function to shuffle an array
    private void ShuffleArray(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // Method to reset the instantiation logic
    public void ResetInstantiatedAnimals()
    {
        foreach (var animal in instantiatedAnimals)
        {
            if (animal != null)
            {
                Destroy(animal);
            }
        }
        instantiatedAnimals.Clear();
        currentGroupIndex = 0;
        InstantiateNextGroup();
    }
}
