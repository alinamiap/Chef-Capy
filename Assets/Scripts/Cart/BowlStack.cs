using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlStack : MonoBehaviour
{
    public GameObject bowlPrefab;
    public Transform[] spawnPoints;
    private Camera mainCamera;
    private HashSet<Transform> occupiedSpawnPoints = new HashSet<Transform>();

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //If mouse overlaps with stacks collider, call Spawn function
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null && collider.gameObject == gameObject)
            {
                SpawnBowl();
            }
        }
    }

    private void SpawnBowl()
    {
        //Iterate through each spawn point, search for unoccupied
        //if available, create bowl prefab at that point and mark point as occupied
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!IsSpawnPointOccupied(spawnPoint))
            {
                Debug.Log("Spawning bowl at spawn point: " + spawnPoint.position);
                Instantiate(bowlPrefab, spawnPoint.position, Quaternion.identity);
                occupiedSpawnPoints.Add(spawnPoint);
                return; 
            }
            else
            {
                Debug.Log("Spawn point occupied: " + spawnPoint.position);
            }
        }

        //Debug.Log("No available spawn points for bowls.");
    }

    private bool IsSpawnPointOccupied(Transform spawnPoint)
    {
        //Checks small radius around spawn point for colliders. Then checks if collider belongs to bowl variants
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPoint.position, 0.5f);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Bowl") || col.CompareTag("SquareBowl") || col.CompareTag("CircleBowl") || col.CompareTag("ChoppedFruitBowl") || col.CompareTag("VeggieBowl"))
            {
                return true;
            }
        }
        return false;
    }
}