//Handles volume and future options
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Text volumeLabel;
    public GameObject optionsCanvas;

    private void Start()
    {
        //Load saved volume settings or set default
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        volumeLabel.text = "Volume: " + Mathf.RoundToInt(savedVolume * 100).ToString();

        //Add listener for volume slider
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeLabel.text = "Volume: " + Mathf.RoundToInt(volume * 100).ToString();
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void GoBack()
    {
        optionsCanvas.SetActive(false);
    }
}
