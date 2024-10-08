//Handles display and hiding of tutorial canvas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialCanvas;

    private void Start()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false);
        }
    }

    public void ShowTutorial()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(true);
        }
    }

    public void HideTutorial()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false);
        }
    }

    public void GoBack()
    {
        HideTutorial();
    }
}
