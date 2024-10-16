using UnityEngine;
using System.Collections.Generic;

public class AnimalInstantiation : MonoBehaviour
{
    public static AnimalInstantiation Instance;
    
    public GameObject[] animalPrefabs;
    public GameObject row;
    public float spaceBetweenAnimals = 1.5f;

    private List<GameObject> instantiatedAnimals = new List<GameObject>();
    private int currentGroupIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        ShuffleArray(animalPrefabs);
        InstantiateNextGroup();
    }

    void Update()
    {
        if (AllAnimalsMatched())
        {
            instantiatedAnimals.Clear();

            if (currentGroupIndex * 3 >= animalPrefabs.Length)
            {
                EndGameManager.Instance.ShowEndGamePanel();
            }
            else
            {
                InstantiateNextGroup();
            }
        }
    }

    public int GetTotalAnimalCount()
    {
        return animalPrefabs.Length;
    }

    private void InstantiateNextGroup()
    {
        Vector3 startPosition = row.transform.position;

        for (int i = 0; i < 3; i++)
        {
            int prefabIndex = currentGroupIndex * 3 + i;
            if (prefabIndex >= animalPrefabs.Length)
            {
                break;
            }

            Vector3 randomPosition = startPosition + new Vector3(i * spaceBetweenAnimals, 0f, 0f);
            GameObject animalPrefab = animalPrefabs[prefabIndex];
            GameObject instantiatedAnimal = Instantiate(animalPrefab, randomPosition, Quaternion.identity, row.transform);
            instantiatedAnimals.Add(instantiatedAnimal);
        }

        currentGroupIndex++;
    }

    private bool AllAnimalsMatched()
    {
        for (int i = instantiatedAnimals.Count - 1; i >= 0; i--)
        {
            GameObject animal = instantiatedAnimals[i];
            if (animal == null || animal.GetComponent<AnimalInteraction>().IsMatched)
            {
                instantiatedAnimals.RemoveAt(i);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

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
}

