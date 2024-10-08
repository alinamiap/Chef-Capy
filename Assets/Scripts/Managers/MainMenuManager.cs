using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public GameObject optionsCanvas;

    private void Start()
    {
        //No canvas is active at start
        if (tutorialCanvas != null) tutorialCanvas.SetActive(false);
        if (optionsCanvas != null) optionsCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowTutorial()
    {
        if (tutorialCanvas != null) tutorialCanvas.SetActive(true);
    }

    public void ShowOptions()
    {
        if (optionsCanvas != null) optionsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        //If running in the editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
