using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blender : MonoBehaviour
{
    public Transform[] smoothieSpawnPoints;
    public GameObject blueberrySmoothiePrefab;
    public GameObject watermelonSmoothiePrefab;
    public float blendingTime = 2.0f;

    private bool isBlending = false;
    private Animator animator;
    private AudioSource blendingSound;

    private void Start()
    {
        //Initialize animator and audio source
        animator = GetComponent<Animator>();
        blendingSound = GetComponent<AudioSource>();
    }

    public void StartBlending(string fruitType)
    {
        Transform spawnPoint = GetAvailableSpawnPoint();
        if (!isBlending && spawnPoint !=null)
        {
            StartCoroutine(BlendSmoothie(fruitType, spawnPoint));
        }
    }

    private IEnumerator BlendSmoothie(string fruitType, Transform spawnPoint)
    {
        isBlending = true;

        //Set proper animation based on fruit type
        if (fruitType == "Blueberry")
        {
            animator.SetBool("isBlendingBlueberry", true);
        }
        else if (fruitType == "Watermelon")
        {
            animator.SetBool("isBlendingWatermelon", true);
        }

        //Play blending sound
        if(blendingSound != null && !blendingSound.isPlaying)
        {
            blendingSound.Play();
        }

        //Debug.Log("Blending started: " + fruitType); // Debug log to check blending start

        //Simulate blending time
        yield return new WaitForSeconds(blendingTime);

        //Create the respective smoothie
        GameObject smoothiePrefab = null;
        if (fruitType == "Blueberry")
        {
            smoothiePrefab = blueberrySmoothiePrefab;
            animator.SetBool("isBlendingBlueberry", false);
        }
        else if (fruitType == "Watermelon")
        {
            smoothiePrefab = watermelonSmoothiePrefab;
            animator.SetBool("isBlendingWatermelon", false);
        }

        if (smoothiePrefab != null)
        {
            Instantiate(smoothiePrefab, spawnPoint.position, Quaternion.identity);
            //Debug.Log(fruitType + " smoothie created.");
        }

        //Stop blending sound
        if(blendingSound != null && blendingSound.isPlaying)
        {
            blendingSound.Stop();
        }

        isBlending = false;
    }

    private Transform GetAvailableSpawnPoint()
    {
        //Checks spawn points, if available return spawn point
        foreach (Transform spawnPoint in smoothieSpawnPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(spawnPoint.position);
            bool isOccupied = false;
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("BlueberrySmoothie") || col.CompareTag("WatermelonSmoothie"))
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                return spawnPoint;
            }
        }

        return null;
    }
}