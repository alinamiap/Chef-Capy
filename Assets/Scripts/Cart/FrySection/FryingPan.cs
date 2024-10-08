using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MonoBehaviour
{
    public Transform veggieSpawnPoint;
    public GameObject cookingGrassPrefab;
    public GameObject cookingCornCarrotPrefab;
    public GameObject cookedGrassInBowlPrefab;
    public GameObject cookedCornCarrotInBowlPrefab;
    public float cookingTime = 5.0f;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 2.0f;

    private GameObject currentCookingVeggie;
    private string currentVeggieType;
    private bool isCooking = false;
    private Camera mainCamera;
    private Vector3 originalPosition;

    public bool IsCooking => isCooking;
    private AudioSource sizzleSound;

    private void Start()
    {
        //Initialize camera, store position of pan, initialize audio
        mainCamera = Camera.main;
        originalPosition = transform.position;
        sizzleSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //If cooking, shake and play sound. Else, stop sound and transfer to bowl
        if (isCooking)
        {
            ShakePan();
            if (sizzleSound != null && !sizzleSound.isPlaying)
            {
                sizzleSound.Play();
            }
        }
        else
        {
            if (sizzleSound != null && sizzleSound.isPlaying)
            {
                sizzleSound.Stop();
            }

            if (currentCookingVeggie != null)
            {
                TryTransferToBowl();
            }
            else
            {
                transform.position = originalPosition;
            }
        }
    }

    public void StartCooking(string veggieType)
    {
        if (currentCookingVeggie == null && !isCooking)
        {
            GameObject veggiePrefab = null;
            if (veggieType == "Grass")
            {
                veggiePrefab = cookingGrassPrefab;
            }
            else if (veggieType == "CornCarrotMedley")
            {
                veggiePrefab = cookingCornCarrotPrefab;
            }

            if (veggiePrefab != null)
            {
                currentCookingVeggie = Instantiate(veggiePrefab, veggieSpawnPoint.position, Quaternion.identity);
                currentCookingVeggie.transform.SetParent(transform);
                currentCookingVeggie.tag = "CookingVeggie";
                currentVeggieType = veggieType;
                StartCoroutine(CookVeggie());
            }
        }
    }

    private IEnumerator CookVeggie()
    {
        isCooking = true;
        yield return new WaitForSeconds(cookingTime);
        isCooking = false;

        TryTransferToBowl();
    }

    private void ShakePan()
    {
        float shakeOffset = Mathf.PingPong(Time.time * shakeSpeed, shakeAmount) - (shakeAmount / 2);
        transform.position = new Vector3(originalPosition.x + shakeOffset, originalPosition.y, originalPosition.z);
    }

    private void TryTransferToBowl()
    {
        GameObject cookedVeggiePrefab = GetCookedVeggiePrefab(currentVeggieType);
        if (cookedVeggiePrefab == null) return;

        //Check each spawn point for a circle bowl
        foreach (Transform spawnPoint in FindObjectOfType<BowlStack>().spawnPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint.position, 0.3f);
            bool isCircleBowl = false;
            GameObject circleBowl = null;

            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("CircleBowl"))
                {
                    isCircleBowl = true;
                    circleBowl = col.gameObject;
                    break;
                }
            }

            if (isCircleBowl)
            {
                Debug.Log("Circle bowl found at: " + spawnPoint.position);
                Destroy(circleBowl);
                GameObject newBowl = Instantiate(cookedVeggiePrefab, spawnPoint.position, Quaternion.identity);
                newBowl.tag = "VeggieBowl";
                Destroy(currentCookingVeggie);
                currentCookingVeggie = null;
                return;
            }
        }

        //If no available circle bowl, keep the cooked veggie in the pan
        //Debug.Log("No available circle bowls.");
    }

    private GameObject GetCookedVeggiePrefab(string veggieType)
    {
        if (veggieType == "Grass")
        {
            return cookedGrassInBowlPrefab;
        }
        else if (veggieType == "CornCarrotMedley")
        {
            return cookedCornCarrotInBowlPrefab;
        }
        return null;
    }

    private GameObject GetCookingVeggiePrefab(string veggieType)
    {
        if (veggieType == "Grass")
        {
            return cookingGrassPrefab;
        }
        else if (veggieType == "CornCarrotMedley")
        {
            return cookingCornCarrotPrefab;
        }
        return null;
    }
}