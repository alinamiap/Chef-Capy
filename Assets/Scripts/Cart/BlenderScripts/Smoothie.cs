using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Smoothie : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    private bool isOverCustomer = false;
    private Customers customer;
    private Camera mainCamera;

    private void Start()
    {
        //Intialize camera, store position of smoothie
        startPosition = transform.position;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //if mouse click colliders with smoothie, make it draggable
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null && collider.gameObject == gameObject)
            {
                isDragging = true;
            }
        }

        //Adjust position of smoothie to follow mouse
        if (isDragging)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }

        //On mouse button up, deliver order or return to position
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                if (isOverCustomer && customer != null)
                {
                    customer.ReceiveOrder(gameObject);
                }
                else
                {
                    transform.position = startPosition;
                }
            }
        }
    }

    //On trigger, store or clear customer reference
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            isOverCustomer = true;
            customer = collision.GetComponent<Customers>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            isOverCustomer = false;
            customer = null;
        }
    }
}
