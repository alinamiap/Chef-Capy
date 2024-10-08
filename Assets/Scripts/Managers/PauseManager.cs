//Handles pausing and resuming game, adjusting volume, and returning to main menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public Slider volumeSlider;

    private void Start()
    {
        //Pause canvas is hidden and game is unpaused at start
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        Time.timeScale = 1f;

        //Load saved volume settings or set default
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;

        //Listener for volume slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void Update()
    {
        //Toggle pause when escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (pauseCanvas.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        //Show the pause menu and stop time
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        //Hide the pause menu and resume time
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        //Ensure the game is resumed before quitting - bug fix
        ResumeGame();  
        
        //Call the GameManager's method to return to the main menu
        if (GameManager.instance != null)
        {
            GameManager.instance.ReturnToMainMenu();
        }
        else
        {
            //load the main menu scene directly if GameManager is not found
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        }
    }

    public void SetVolume(float volume)
    {
        //Set the global volume using AudioListener
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}