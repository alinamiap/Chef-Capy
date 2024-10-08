using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging = false;
    private bool isOverCustomer = false;
    private Customers customer;
    private Camera mainCamera;

    private void Start()
    {
        //store original position of bowl
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    private void Update()
    {
        //Convert position to world position and check if it overlaps with bowl collider
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);
            if (collider != null && collider.gameObject == gameObject)
            {
                //If bowl clicked, drag
                isDragging = true;
            }
        }

        //If dragging, update position to follow mouse
        if (isDragging)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            //if bowl over customer when dropped, deliver order
            if (isOverCustomer && customer != null)
            {
                customer.ReceiveOrder(gameObject);
            }
            else
            {
                //Return to original position
                transform.position = originalPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            //If customer, store customer reference
            isOverCustomer = true;
            customer = collision.GetComponent<Customers>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            //if not customer, clear reference
            isOverCustomer = false;
            customer = null;
        }
    }
}