//Handles spawning of customers, their interaction, and increases game difficulty over time
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public Transform[] customerSpawnPoints;
    public int maxCustomers = 5;
    public float spawnInterval = 10.0f;
    private float timer;
    private List<GameObject> activeCustomers = new List<GameObject>();
    private bool[] occupiedSpawnPoints;

    [System.Serializable]
    public class FoodItem
    {
        public string name;
        public GameObject prefab;
        public int cost;
    }
    public FoodItem[] foodItems;

    private GameManager gameManager;

    private float difficultyIncreaseInterval = 10.0f;
    private float difficultyTimer = 0.0f;
    private float happinessDecayRateIncrease = 0.005f;
    private float spawnRateIncrease = 0.5f;

    private void Start()
    {
        occupiedSpawnPoints = new bool[customerSpawnPoints.Length];
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (GameManager.instance.isGameActive)
        {
            timer += Time.deltaTime;
            difficultyTimer += Time.deltaTime;

            if (timer >= spawnInterval && activeCustomers.Count < maxCustomers)
            {
                SpawnCustomer();
                timer = 0;
            }

            if (difficultyTimer >= difficultyIncreaseInterval)
            {
                IncreaseDifficulty();
                difficultyTimer = 0;
            }
        }
    }

    private void SpawnCustomer()
    {
        int customerIndex = Random.Range(0, customerPrefabs.Length);
        GameObject customerPrefab = customerPrefabs[customerIndex];

        int spawnIndex = FindAvailableSpawnPoint();
        if (spawnIndex == -1)
        {
            return;
        }

        occupiedSpawnPoints[spawnIndex] = true;

        GameObject newCustomer = Instantiate(customerPrefab, customerSpawnPoints[spawnIndex].position, Quaternion.identity);
        FoodItem requestedItem = foodItems[Random.Range(0, foodItems.Length)];
        newCustomer.GetComponent<Customers>().SetRequestedItem(requestedItem);
        newCustomer.GetComponent<Customers>().SetManager(this, spawnIndex);

        activeCustomers.Add(newCustomer);
    }

    private int FindAvailableSpawnPoint()
    {
        for (int i = 0; i < customerSpawnPoints.Length; i++)
        {
            if (!occupiedSpawnPoints[i])
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveCustomer(GameObject customer, int spawnIndex)
    {
        activeCustomers.Remove(customer);
        occupiedSpawnPoints[spawnIndex] = false;
        Destroy(customer);
    }

    public void CompleteOrder(GameObject customer)
    {
        FoodItem requestedItem = customer.GetComponent<Customers>().GetRequestedItem();
        int itemCost = requestedItem.cost;

        float happiness = customer.GetComponent<Customers>().happiness;
        int tip = Mathf.FloorToInt(itemCost * 0.2f * happiness);

        gameManager.AddGold(itemCost + tip);
        gameManager.AddGoldWon(itemCost + tip);

        int spawnIndex = customer.GetComponent<Customers>().GetSpawnIndex();
        RemoveCustomer(customer, spawnIndex);
        //Debug.Log("Order completed. Gold added: " + (itemCost + tip));
    }

    private void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(1.0f, spawnInterval - spawnRateIncrease);
        foreach (var customer in activeCustomers)
        {
            Customers customerScript = customer.GetComponent<Customers>();
            customerScript.happinessDecayRate += happinessDecayRateIncrease;
        }
        //Debug.Log("Difficulty increased. New spawn interval: " + spawnInterval);
    }

    public void ClearAllCustomers()
    {
        //Remove all active customers
        foreach (var customer in activeCustomers)
        {
            Destroy(customer);
        }
        activeCustomers.Clear();

        //Reset occupied spawn points
        for (int i = 0; i < occupiedSpawnPoints.Length; i++)
        {
            occupiedSpawnPoints[i] = false;
        }
    }
}