using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppedFruitTray : MonoBehaviour
{
    public GameObject choppedAppleInSquareBowlPrefab;
    public GameObject choppedBananaInSquareBowlPrefab;
    public Transform[] spawnPoints;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null)
            {
                Debug.Log("Clicked on: " + collider.gameObject.name);
                if (collider.gameObject.CompareTag("AppleTray"))
                {
                    Debug.Log("Apple tray clicked.");
                    PlaceFruitInSquareBowl(choppedAppleInSquareBowlPrefab);
                }
                else if (collider.gameObject.CompareTag("BananaTray"))
                {
                    Debug.Log("Banana tray clicked.");
                    PlaceFruitInSquareBowl(choppedBananaInSquareBowlPrefab);
                }
            }
            else
            {
                Debug.Log("No collider found at mouse position.");
            }
        }
    }

    private void PlaceFruitInSquareBowl(GameObject choppedFruitPrefab)
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            //Check if the spawn point has a square bowl
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint.position, 0.3f);
            bool isSquareBowl = false;
            GameObject squareBowl = null;

            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("SquareBowl"))
                {
                    isSquareBowl = true;
                    squareBowl = col.gameObject;
                    break;
                }
            }

            if (isSquareBowl)
            {
                Debug.Log("Square bowl found at: " + spawnPoint.position);
                Destroy(squareBowl);
                GameObject newBowl = Instantiate(choppedFruitPrefab, spawnPoint.position, Quaternion.identity);
                newBowl.tag = "ChoppedFruitBowl";
                Debug.Log("Fruit placed in square bowl.");
                return;
            }
        }
        Debug.Log("No available square bowls.");
    }
}