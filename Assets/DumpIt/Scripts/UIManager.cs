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
    public GameObject PausePanel;
    public GameObject PlatformExtendPanel;
    public TMP_Text Score;
    public TMP_Text Coin;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        Platformbtn.onClick.AddListener(UsePlatformPowerUp);
        Osciallate.onClick.AddListener(UseSwingPowerUp);
        crossbtn.onClick.AddListener(ClosePlatform);
        leftextbtn.onClick.AddListener(LeftExtent);
        rightextbtn.onClick.AddListener(RightExtent);
        topextbtn.onClick.AddListener(TopExtent);
        bottomextbtn.onClick.AddListener(BottomExtent);
        PausePanel.SetActive(false);
        PlatformExtendPanel.SetActive(false);

        if (GamePlayManager.Instance != null)
            GamePlayManager.Instance.ScoreTextUI = Score;

        if (DataManager.Instance != null)
        {
            UpdateScoreText(DataManager.Instance.GetCurrentScore());
            UpdateCoinText(DataManager.Instance.GetCurrentCoins());

            DataManager.Instance.OnScoreUpdated += UpdateScoreText;
            DataManager.Instance.OnCoinsUpdated += OnCoinsUpdated;
        }

        Platformbtn.interactable = false;
        Osciallate.interactable = false;
        CheckPowerUps();
    }
    private void OnCoinsUpdated(int coins)
    {
        UpdateCoinText(coins);
        CheckPowerUps();
    }

    public void TogglePause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PlatformExtender.Instance.ResetPlatform();
        GamePlayManager.Instance.Restart();
    }

    public void QuitGame()
    {
        AppManager.Instance.ExitGame();
        AppStateManager.Instance.SetHome();
        PausePanel.SetActive(false);
    }
    public void OpenPlatformExtendPanel()
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
        MakeButtonsNotInteractable();
    }

    public void RightExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendRight();
        MakeButtonsNotInteractable();
    }

    public void TopExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendForward();
        MakeButtonsNotInteractable();
    }

    public void BottomExtent()
    {
        if (PlatformExtender.Instance == null)
        {
            Debug.LogError("PlatformExtender not assigned!");
            return;
        }
        PlatformExtender.Instance.ExtendBack();
        MakeButtonsNotInteractable();
    }

    public void DisplayGameOverPanel()
    {
        AppManager.Instance.ExitGame();
        AppStateManager.Instance.SetGameOver();
    }

    private void UpdateScoreText(int currentScore)
    {
        if (Score != null)
            Score.text = currentScore.ToString();
    }
    private void UpdateCoinText(int currentCoins)
    {
        if (Coin != null)
            Coin.text = currentCoins.ToString();
    }


    private void OnDestroy()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnScoreUpdated -= UpdateScoreText;
            DataManager.Instance.OnCoinsUpdated -= OnCoinsUpdated;
        }
    }

    public void CheckPowerUps()
    {
        int coins = DataManager.Instance.GetCurrentCoins();
        Debug.Log($"--------------------->>>>>>>>>>>>>>Current Coins --->>: {coins}");

        if (coins >= 20)
        {
            Platformbtn.interactable = true;
        }
        else
            Platformbtn.interactable = false;

        if (coins >= 30)
            Osciallate.interactable = true;
        else
            Osciallate.interactable = false;
    }

    public void UsePlatformPowerUp()
    {
        if (DataManager.Instance.SpendCoins(20))
        {
            CheckPowerUps();
            OpenPlatformExtendPanel();
            leftextbtn.interactable = true;
            rightextbtn.interactable = true;
            topextbtn.interactable = true;
            bottomextbtn.interactable = true;
        }
    }

    public void UseSwingPowerUp()
    {
        if (DataManager.Instance.SpendCoins(30))
        {
            CheckPowerUps();
            SwingMotion.Instance.swingZ = !SwingMotion.Instance.swingZ;
        }

    }
    public void MakeButtonsNotInteractable()
    {
        leftextbtn.interactable = false;
        rightextbtn.interactable = false;
        topextbtn.interactable = false;
        bottomextbtn.interactable = false;
    }
}
