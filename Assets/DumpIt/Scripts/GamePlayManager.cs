using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance { get; private set; }
    public TMPro.TMP_Text ScoreTextUI;
    public event Action<int> OnScoreChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Debug.Log("GAME: StartGame()");
        GameMechanics.Instance.StartGame();
    }

    public void UpdateScoreUI()
    {
        int score = DataManager.Instance.GetCurrentScore();
        ScoreTextUI.text = score.ToString();
        OnScoreChanged?.Invoke(score);
    }
    public void GameOver()
    {
        DataManager.Instance.SaveBestScoreIfNeeded();
        UIManager.Instance.DisplayGameOverPanel();

    }
    public void Restart()
    {
        DataManager.Instance.ResetScore();
        // // Reset pendulum
        GameMechanics.Instance.StartGame();

    }

}


