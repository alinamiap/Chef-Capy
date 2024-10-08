using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public bool isHovering = false;
    private GameObject hoveringItem = null;

    //HashSet for more efficient tag checking
    private readonly HashSet<string> foodItemTags = new HashSet<string> { 
        "WatermelonSmoothie", 
        "BlueberrySmoothie", 
        "ChoppedFruitBowl", 
        "VeggieBowl", 
        "SquareBowl", 
        "CircleBowl" 
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the colliding object is a food item
        if (foodItemTags.Contains(other.tag))
        {
            isHovering = true;
            hoveringItem = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the colliding object is a food item
        if (foodItemTags.Contains(other.tag))
        {
            isHovering = false;
            hoveringItem = null;
        }
    }

    private void Update()
    {
        //If item hovering and left mouse button up, destroy hovering item
        if (isHovering && Input.GetMouseButtonUp(0) && hoveringItem != null)
        {
            Destroy(hoveringItem);
            hoveringItem = null;
            isHovering = false;
        }
    }
}