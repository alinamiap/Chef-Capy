//Handles customer state including happiness, requested items, and orders
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    public float happiness = 1.0f;
    public float happinessDecayRate = 0.01f;
    private CustomerManager manager;
    private int spawnIndex;
    private CustomerManager.FoodItem requestedItem;
    private bool orderCompleted = false;

    private GameObject requestedItemInstance;
    private GameObject smileyInstance;

    //happiness prefabs
    public GameObject superSmileyPrefab;
    public GameObject smileyPrefab;
    public GameObject straightFacePrefab;
    public GameObject upsetFacePrefab;
    public GameObject angryFacePrefab;

    private void Update()
    {
        if (!orderCompleted)
        {
            happiness -= happinessDecayRate * Time.deltaTime;
            happiness = Mathf.Clamp01(happiness);

            UpdateHappinessDisplay();

            if (happiness <= 0)
            {
                LeaveDueToLowHappiness();
            }
        }
    }

    private void UpdateHappinessDisplay()
    {
        if (smileyInstance != null)
        {
            Destroy(smileyInstance);
        }

        if (happiness >= 0.9f)
        {
            smileyInstance = Instantiate(superSmileyPrefab, transform.position + new Vector3(3, 5, 0), Quaternion.identity);
        }
        else if (happiness >= 0.75f)
        {
            smileyInstance = Instantiate(smileyPrefab, transform.position + new Vector3(3, 5, 0), Quaternion.identity);
        }
        else if (happiness >= 0.5f)
        {
            smileyInstance = Instantiate(straightFacePrefab, transform.position + new Vector3(3, 5, 0), Quaternion.identity);
        }
        else if (happiness >= 0.2f)
        {
            smileyInstance = Instantiate(upsetFacePrefab, transform.position + new Vector3(3, 5, 0), Quaternion.identity);
        }
        else
        {
            smileyInstance = Instantiate(angryFacePrefab, transform.position + new Vector3(3, 5, 0), Quaternion.identity);
            if (!isShaking)
            {
                StartCoroutine(ShakeCustomer());
            }
        }

        //Parent the smiley face to the customer
        if (smileyInstance != null)
        {
            smileyInstance.transform.SetParent(transform);
        }
    }

    private bool isShaking = false;
    private IEnumerator ShakeCustomer()
    {
        isShaking = true;
        Vector3 originalPosition = transform.position;

        while (happiness < 0.2f && !orderCompleted)
        {
            float shakeOffset = Mathf.PingPong(Time.time * 5f, 0.1f) - 0.1f;
            transform.position = new Vector3(originalPosition.x + shakeOffset, originalPosition.y, originalPosition.z);
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }

    public void SetRequestedItem(CustomerManager.FoodItem item)
    {
        requestedItem = item;
        Debug.Log("Customer is ordering: " + item.name);
        DisplayRequestedItem(item);
    }

    public CustomerManager.FoodItem GetRequestedItem()
    {
        return requestedItem;
    }

    public void SetManager(CustomerManager manager, int spawnIndex)
    {
        this.manager = manager;
        this.spawnIndex = spawnIndex;
    }

    public int GetSpawnIndex()
    {
        return spawnIndex;
    }

    public void DisplayRequestedItem(CustomerManager.FoodItem item)
    {
        if (requestedItemInstance != null)
        {
            Destroy(requestedItemInstance);
        }

        requestedItemInstance = Instantiate(item.prefab, transform);
        requestedItemInstance.transform.localPosition = new Vector3(-1, 13, 0);
        requestedItemInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        foreach (var collider in requestedItemInstance.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
        foreach (var component in requestedItemInstance.GetComponents<MonoBehaviour>())
        {
            component.enabled = false;
        }
    }

    public void OrderCompleted(GameObject item)
    {
        if (!orderCompleted)
        {
            orderCompleted = true;
            Destroy(item);
            Destroy(requestedItemInstance);
            manager.CompleteOrder(gameObject);
            GameManager.instance.IncrementCustomersServed();
        }
    }

    public void ReceiveOrder(GameObject item)
    {
        if (item.CompareTag(requestedItem.prefab.tag))
        {
            Debug.Log("Order received: " + item.name);
            OrderCompleted(item);
        }
        else
        {
            Debug.Log("Wrong order: " + item.name);
        }
    }

    private void LeaveDueToLowHappiness()
    {
        Debug.Log("Customer left due to low happiness.");
        manager.RemoveCustomer(gameObject, spawnIndex);
        int potentialGoldLost = requestedItem.cost;
        GameManager.instance.IncrementCustomersLeftDueToLowHappiness(potentialGoldLost);
    }
}