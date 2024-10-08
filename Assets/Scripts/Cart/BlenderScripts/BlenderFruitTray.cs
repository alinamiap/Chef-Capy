using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTray : MonoBehaviour
{
    public string fruitType;
    private Blender blender;
    private Camera mainCamera;

    private void Start()
    {
        //Initialize blender and camera
        blender = FindObjectOfType<Blender>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //If mouse click overlaps with fruit tray collider, blend that fruit
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null && collider.gameObject == gameObject)
            {
                //Debug.Log("Fruit tray clicked: " + fruitType); // Debug log to check click detection
                blender.StartBlending(fruitType);
            }
        }
    }
}
