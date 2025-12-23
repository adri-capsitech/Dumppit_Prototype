using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Button Pausebtn;
    public Button Resumebtn;
    public Button Restartbtn;
    public Button Quitbtn;
    public Button Platformbtn;
    public Button Osciallate;
    public Button leftextbtn;
    public Button rightextbtn;
    public Button topextbtn;
    public Button bottomextbtn;
    public Button crossbtn;
    // public GameObject platform;
    public GameObject PausePanel;
    public GameObject PlatformExtendPanel;
    public TMP_Text Score;
    public TMP_Text BestScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Pausebtn.onClick.AddListener(TogglePause);
        Resumebtn.onClick.AddListener(ResumeGame);
        Restartbtn.onClick.AddListener(RestartGame);
        Quitbtn.onClick.AddListener(QuitGame);
        Platformbtn.onClick.AddListener(PlatformExtend);
        crossbtn.onClick.AddListener(ClosePlatform);
        leftextbtn.onClick.AddListener(LeftExtent);
        rightextbtn.onClick.AddListener(RightExtent);
        topextbtn.onClick.AddListener(TopExtent);
        bottomextbtn.onClick.AddListener(BottomExtent);
        PausePanel.SetActive(false);
        PlatformExtendPanel.SetActive(false);
    }
    public void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            PausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PausePanel.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        PausePanel.SetActive(false);
        PlatformExtender.Instance.ResetPlatform();
    }
    public void QuitGame()
    {
        PausePanel.SetActive(false);
    }
    public void PlatformExtend()
    {
        PlatformExtendPanel.SetActive(true);
    }
    public void ClosePlatform()
    {
        PlatformExtendPanel.SetActive(false);
    }
    public void LeftExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendLeft();
    }
    public void RightExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendRight();
    }
    public void TopExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendForward();
    }
    public void BottomExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendBack();
    }
    public void DisplayGameOverPanel()
    {
        AppStateManager.Instance.SetGameOver();
    }
    // public void GetBestScore()
    // {
    //     // DataManager.Instance.GetCurrentScore();
    //     Score.text = DataManager.Instance.GetCurrentScore().ToString();
    // }
    // public void GetHighScore()
    // {
    //     // DataManager.Instance.GetBestScore();
    //     BestScore.text = DataManager.Instance.GetBestScore().ToString();
    // }
}
