//Handles overall game state, tracks time and gold, handles UI updates
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI goldText;
    public EndGamePanel endGamePanel;

    private float timePassed = 0f;
    private int playerGold = 0;
    private int customersServed = 0;
    private int customersLeftDueToLowHappiness = 0;
    private int goldWon = 0;
    private int potentialGoldLost = 0;

    public bool isGameActive;

    private CustomerManager customerManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Debug.Log("GameManager instance created.");
        }
        else
        {
            Destroy(gameObject);
            //Debug.Log("Duplicate GameManager instance destroyed.");
            return;
        }
    }

    private void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        StartGame();
    }

    private void Update()
    {
        if (isGameActive)
        {
            timePassed += Time.deltaTime;
            UpdateUI();
        }
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        //Debug.Log("Gold added: " + amount + ". Total gold: " + playerGold);
        UpdateUI();
    }

    public void AddGoldWon(int amount)
    {
        goldWon += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.FloorToInt(timePassed).ToString();
        if (goldText != null)
            goldText.text = "Gold: " + playerGold.ToString();
        //Debug.Log("UI Updated. Time: " + Mathf.FloorToInt(timePassed) + ", Gold: " + playerGold);
    }

    public void IncrementCustomersServed()
    {
        customersServed++;
    }

    public void IncrementCustomersLeftDueToLowHappiness(int potentialGoldFromCustomer)
    {
        customersLeftDueToLowHappiness++;
        potentialGoldLost += potentialGoldFromCustomer;
        if (customersLeftDueToLowHappiness >= 10)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameActive = false;
        customerManager.ClearAllCustomers();
        endGamePanel.ShowEndGamePanel(customersServed, goldWon, Mathf.FloorToInt(timePassed), potentialGoldLost);
    }

    public void StartGame()
    {
        //Debug.Log("Game started.");
        isGameActive = true;
        timePassed = 0;
        playerGold = 0;
        customersServed = 0;
        customersLeftDueToLowHappiness = 0;
        goldWon = 0;
        potentialGoldLost = 0;
        endGamePanel.HideEndGamePanel();
        UpdateUI();
    }

    public void ReplayGame()
    {
        ResetGameState();
        customerManager.ClearAllCustomers();
        StartGame();
    }

    public void ReturnToMainMenu()
    {
        ResetGameState();
        customerManager.ClearAllCustomers();
        SceneManager.LoadScene("MainMenuScene");
    }

    private void ResetGameState()
    {
        //Debug.Log("Resetting game state.");
        isGameActive = false;
        timePassed = 0;
        playerGold = 0;
        customersServed = 0;
        customersLeftDueToLowHappiness = 0;
        goldWon = 0;
        potentialGoldLost = 0;
        Time.timeScale = 1f;

        endGamePanel.HideEndGamePanel();
    }
}
