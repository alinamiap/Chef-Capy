using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieTray : MonoBehaviour
{
    public GameObject fryingPan1;
    public GameObject fryingPan2; 
    private FryingPan fryingPanScript1;
    private FryingPan fryingPanScript2;
    private Camera mainCamera;

    private void Start()
    {
        //Initialize camera and frying pans
        mainCamera = Camera.main;
        fryingPanScript1 = fryingPan1.GetComponent<FryingPan>();
        fryingPanScript2 = fryingPan2.GetComponent<FryingPan>();
    }

    private void Update()
    {
        //if mouse button down overlaps with veggie tray, cook that veggie
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null && collider.gameObject == gameObject)
            {
                if (gameObject.CompareTag("GrassTray"))
                {
                    StartCooking("Grass");
                }
                else if (gameObject.CompareTag("CornCarrotTray"))
                {
                    StartCooking("CornCarrotMedley");
                }
            }
        }
    }

    private void StartCooking(string veggieType)
    {
        if (!fryingPanScript1.IsCooking)
        {
            fryingPanScript1.StartCooking(veggieType);
        }
        else if (!fryingPanScript2.IsCooking)
        {
            fryingPanScript2.StartCooking(veggieType);
        }
        else
        {
            //Debug.Log("Both pans are currently occupied.");
        }
    }
}