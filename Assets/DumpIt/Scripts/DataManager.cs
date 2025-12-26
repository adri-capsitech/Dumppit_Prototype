
using System;
using System.Collections.Generic;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    // Singleton instance
    public static DataManager Instance { get; private set; }

    private const string SCORE_KEY = "CURRENT_SCORE";
    private const string BEST_SCORE_KEY = "BEST_SCORE";

    private const string COIN_KEY = "COIN_KEY";
    private const string DYNAMIC_HIGH_SCORE_KEY = "DYNAMIC_BEST_SCORE";

    public int FinalScore = 0;
    public int Score = 0;
    public event Action<int> OnNewBestScore;
    public event Action<int> OnScoreUpdated;
    public event Action<int> OnCoinsUpdated;

    private bool highScoreAchieved = false;
    private int nextCoinMilestone = 20;
    private int coins = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (!PlayerPrefs.HasKey(BEST_SCORE_KEY))
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetInt(COIN_KEY, 0);
        PlayerPrefs.Save();

        coins = 0;
    }

    public int GetCurrentScore()
    {
        return PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
    }
    public int GetDynamicBestScore()
    {
        return PlayerPrefs.GetInt(DYNAMIC_HIGH_SCORE_KEY, 0);
    }
    public int GetCurrentCoins()
    {
        return PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    public void UpdateScore()
    {
        Score += 10;
        PlayerPrefs.SetInt(SCORE_KEY, Score);
        PlayerPrefs.Save();
        FinalScore = Score;
        Debug.Log("The final score is " + FinalScore);

        OnScoreUpdated?.Invoke(FinalScore);

        CheckCoinReward();
        SaveBestScoreIfNeeded();
    }
    private void CheckCoinReward()
    {
        if (GetCurrentScore() >= nextCoinMilestone)
        {
            coins += 20;
            PlayerPrefs.SetInt(COIN_KEY, coins);
            PlayerPrefs.Save();
            OnCoinsUpdated?.Invoke(coins);
            nextCoinMilestone += 20;
        }
    }
    // public void UpdateCoinReward(int amount)
    // {
    //     if (amount == 20)
    //         Debug.Log("20 coins used for platform extender");
    //     else if (amount == 30)
    //         Debug.Log("30 coins used for swing powerup");
    //     coins -= amount;
    //     PlayerPrefs.SetInt(COIN_KEY, coins);
    //     PlayerPrefs.Save();
    //     OnCoinsUpdated?.Invoke(coins);


    // }
    public bool SpendCoins(int amount)
    {
        if (coins < amount)
            return false;

        coins -= amount;
        PlayerPrefs.SetInt(COIN_KEY, coins);
        PlayerPrefs.Save();

        OnCoinsUpdated?.Invoke(coins);
        return true;
    }

    public bool HasHighScore()
    {
        return highScoreAchieved;
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, 0);
        PlayerPrefs.SetInt(COIN_KEY, 0);
        PlayerPrefs.Save();
        Score = 0;
        OnScoreUpdated?.Invoke(Score);
        FinalScore = 0;
        highScoreAchieved = false;
        coins = 0;
        nextCoinMilestone = 20;
    }
    public void SaveBestScoreIfNeeded()
    {
        int finalScore = FinalScore;
        int bestScore = GetBestScore();

        if (finalScore > bestScore)
        {
            PlayerPrefs.SetInt(BEST_SCORE_KEY, finalScore);
            PlayerPrefs.Save();
        }
    }

    public (int finalScore, int bestScore) GetFinalResult()
    {
        return (FinalScore, GetBestScore());
    }
}