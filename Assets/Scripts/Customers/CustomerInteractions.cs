//Handles interaction between customer and food items
//Checks if correct item is delivered
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteraction : MonoBehaviour
{
    private Customers customer;

    private void Start()
    {
        customer = GetComponent<Customers>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay2D called with: " + other.tag);

        //Check if the tag of the other collider matches the requested item tag and if the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse button up detected.");
            
            if (other.CompareTag(customer.GetRequestedItem().prefab.tag))
            {
                Debug.Log("Tag matches. Delivering order.");
                customer.ReceiveOrder(other.gameObject);
            }
            else
            {
                Debug.Log("Tag does not match: " + other.tag + " vs " + customer.GetRequestedItem().prefab.tag);
            }
        }
    }
}