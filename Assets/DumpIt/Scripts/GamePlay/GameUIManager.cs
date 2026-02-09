using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("Buttons")]
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

    [Header("Panels")]
    public GameObject PausePanel;
    public GameObject PlatformExtendPanel;


    [Header("UI Text")]
    public TMP_Text Score;
    public TMP_Text Coin;
    public TMPro.TMP_Text TapText;
    public float tapTextStartDuration = 2f;

    private const int PLATFORM_COST = 20;
    private const int SWING_COST = 30;

    public float idleTime = 10f;
    [SerializeField] private float IdleCounter = 0;
    public Image imagepanel;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Button bindings
        Pausebtn.onClick.AddListener(TogglePause);
        Resumebtn.onClick.AddListener(ResumeGame);
        // Restartbtn.onClick.AddListener(RestartGame);
        Restartbtn.onClick.AddListener(OnRestartButtonClicked);

        Quitbtn.onClick.AddListener(QuitGame);

        Platformbtn.onClick.AddListener(OpenPlatformExtendPanel);
        Osciallate.onClick.AddListener(UseSwingPowerUp);

        leftextbtn.onClick.AddListener(LeftExtent);
        rightextbtn.onClick.AddListener(RightExtent);
        topextbtn.onClick.AddListener(TopExtent);
        bottomextbtn.onClick.AddListener(BottomExtent);

        crossbtn.onClick.AddListener(ClosePlatform);

        PausePanel.SetActive(false);
        PlatformExtendPanel.SetActive(false);

        if (DataManager.Instance != null)
        {
            UpdateScoreText(DataManager.Instance.GetCurrentScore());
            UpdateCoinText(DataManager.Instance.GetCurrentCoins());

            DataManager.Instance.OnScoreUpdated += UpdateScoreText;
            DataManager.Instance.OnCoinsUpdated += OnCoinsUpdated;
        }

        CheckPowerUps();

        TapText.gameObject.SetActive(true);
        StartCoroutine(HideTapTextAfterDelay());
    }
    private IEnumerator HideTapTextAfterDelay()
    {
        yield return new WaitForSeconds(tapTextStartDuration);
        TapText.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (GamePlayManager.Instance == null) return;
        if (GameMechanics.Instance == null) return;
        if (IdleCounter == idleTime && GameMechanics.Instance.isIdle) return;

        if (GameMechanics.Instance.isIdle)
        {
            IdleCounter += Time.deltaTime;

            if (IdleCounter >= idleTime)
            {
                IdleCounter = idleTime;
                TapText.gameObject.SetActive(true);
                return;
            }
        }
        else
        {
            TapText.gameObject.SetActive(false);
            GameMechanics.Instance.isIdle = true;
            IdleCounter = 0;
        }
    }

    #region Pause / Game Controls

    public void TogglePause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        imagepanel.GetComponent<Image>().enabled = true;
        transform.parent.GetComponent<Image>().enabled = true;

    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        imagepanel.GetComponent<Image>().enabled = false;
        transform.parent.GetComponent<Image>().enabled = false;

    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        transform.parent.GetComponent<Image>().enabled = false;
        imagepanel.GetComponent<Image>().enabled = false;

        PausePanel.SetActive(false);
        // Restartbtn.onClick.AddListener(AdManager.Instance.ShowAd);


        PlatformExtender.Instance.ResetPlatform();
        GamePlayManager.Instance.Restart();
        CameraControl.Instance.SwitchToDefaultCamera();
        DataManager.Instance.ResetScore();
    }

    public void QuitGame()
    {
        AppManager.Instance.ExitGame();
        CameraControl.Instance.SwitchToDefaultCamera();
        AppStateManager.Instance.SetHome();
    }

    #endregion

    #region Platform UI

    public void OpenPlatformExtendPanel()
    {
        PlatformExtendPanel.SetActive(true);
    }

    public void ClosePlatform()
    {
        PlatformExtendPanel.SetActive(false);
    }

    #endregion

    #region Platform Extension Logic

    private void TryExtend(System.Action extendAction)
    {
        if (DataManager.Instance.GetCurrentCoins() < PLATFORM_COST)
        {
            CheckPowerUps();
            return;
        }

        if (DataManager.Instance.SpendCoins(PLATFORM_COST))
        {
            extendAction?.Invoke();
            UpdateCoinText(DataManager.Instance.GetCurrentCoins());
            CheckPowerUps();
        }
    }

    public void LeftExtent()
    {
        if (PlatformExtender.Instance == null) return;

        else if (SwingMotion.Instance.swingZ == true)
            TryExtend(() => PlatformExtender.Instance.ExtendBack());
        else
            TryExtend(() => PlatformExtender.Instance.ExtendLeft());
    }

    public void RightExtent()
    {
        if (PlatformExtender.Instance == null) return;

        else if (SwingMotion.Instance.swingZ == true)
            TryExtend(() => PlatformExtender.Instance.ExtendForward());

        TryExtend(() => PlatformExtender.Instance.ExtendRight());
    }

    public void TopExtent()
    {
        if (PlatformExtender.Instance == null) return;

        else if (SwingMotion.Instance.swingZ == true)
            TryExtend(() => PlatformExtender.Instance.ExtendLeft());
        else
            TryExtend(() => PlatformExtender.Instance.ExtendForward());
    }

    public void BottomExtent()
    {
        if (PlatformExtender.Instance == null) return;

        else if (SwingMotion.Instance.swingZ == true)

            TryExtend(() => PlatformExtender.Instance.ExtendRight());
        else
            TryExtend(() => PlatformExtender.Instance.ExtendBack());
    }

    #endregion

    #region Powerups

    public void UseSwingPowerUp()
    {
        if (DataManager.Instance.SpendCoins(SWING_COST))
        {
            SwingMotion.Instance.swingZ = !SwingMotion.Instance.swingZ;
            CheckPowerUps();
        }
    }

    #endregion

    #region UI Updates

    private void UpdateScoreText(int score)
    {
        if (Score != null)
            Score.text = score.ToString();
    }

    private void UpdateCoinText(int coins)
    {
        if (Coin != null)
            Coin.text = coins.ToString();
    }

    private void OnCoinsUpdated(int coins)
    {
        UpdateCoinText(coins);
        CheckPowerUps();
    }

    public void CheckPowerUps()
    {
        int coins = DataManager.Instance.GetCurrentCoins();

        // Power buttons
        Platformbtn.interactable = coins >= PLATFORM_COST;
        Osciallate.interactable = coins >= SWING_COST;

        // Extend buttons
        bool canExtend = coins >= PLATFORM_COST;
        leftextbtn.interactable = canExtend;
        rightextbtn.interactable = canExtend;
        topextbtn.interactable = canExtend;
        bottomextbtn.interactable = canExtend;
    }


    public void DisplayGameOverPanel()
    {
        AppManager.Instance.ExitGame();
        AppStateManager.Instance.SetGameOver();
    }

    #endregion

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnScoreUpdated -= UpdateScoreText;
            DataManager.Instance.OnCoinsUpdated -= OnCoinsUpdated;
        }
    }
    // public void OnRestartButtonClicked()
    // {
    //     AdManager.Instance.ShowAd(OnAdFinishedRestart);
    // }
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1;

        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowAd(OnAdFinishedRestart);
        }
        else
        {
            Debug.LogError("AdManager is NULL! Restarting game without ad.");
            RestartGame();   
        }
    }

    private void OnAdFinishedRestart()
    {
        RestartGame();
    }

}
