using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public GameObject creditCanvas;

    private void Start()
    {
        //Hidden at start
        if (creditCanvas != null)
        {
            creditCanvas.SetActive(false);
        }
    }

    public void ShowCredits()
    {
        if (creditCanvas != null)
        {
            creditCanvas.SetActive(true);
        }
    }

    public void HideCredits()
    {
        if (creditCanvas != null)
        {
            creditCanvas.SetActive(false);
        }
    }
}