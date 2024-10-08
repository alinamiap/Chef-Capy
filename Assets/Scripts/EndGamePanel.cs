using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    public GameObject endGameCanvas;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI customersServedText;
    public TextMeshProUGUI goldWonText;
    public TextMeshProUGUI timePlayedText;
    public TextMeshProUGUI goldLostText;
    public Image capyImage;
    public Sprite happyCapySprite;
    public Sprite upsetCapySprite;
    private int customersServed;
    private int goldWon;

    private void Start()
    {
        if(endGameCanvas != null)
        {
        endGameCanvas.SetActive(false);
        }    
    }

    public void ShowEndGamePanel(int customersServed, int goldWon, int timePlayed, int potentialGoldLost)
    {
        this.customersServed = customersServed;
        this.goldWon = goldWon;

        //Update the UI elements
        customersServedText.text = "Customers Served: " + customersServed;
        goldWonText.text = "Gold Received: " + goldWon;
        timePlayedText.text = "Time Played: " + timePlayed + "s";
        goldLostText.text = "Gold Lost: " + potentialGoldLost;

        //Calculate the ratio
        float totalPossibleGold = goldWon + potentialGoldLost;
        float ratio = customersServed > 0 ? (float)goldWon / totalPossibleGold : 0;

        //Set the capybara image and header text based on the ratio
        if (ratio >= 0.75f)
        {
            capyImage.sprite = happyCapySprite;
            headerText.text = "What a great day!";
        }
        else
        {
            capyImage.sprite = upsetCapySprite;
            headerText.text = "Capy needs more money...";
        }

        //Show panel
        endGameCanvas.SetActive(true);
    }

    public void HideEndGamePanel()
    {
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(false);
        }
    }
}
