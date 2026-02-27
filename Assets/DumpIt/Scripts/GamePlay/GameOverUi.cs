
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour
{

    public static GameOverUi Instance;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button retryButton;
    public AudioClip gameOverMusic;


    private void Start()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        var result = DataManager.Instance.GetFinalResult();

        bestScoreText.text = result.bestScore.ToString();
        finalScoreText.text = result.finalScore.ToString();
        // Add button listeners
        homeButton.onClick.AddListener(OnHomeButtonClicked);
        retryButton.onClick.AddListener(OnRetryButtonClicked);
        if(AudioController.Instance != null)
            AudioController.Instance.PlayMusic(gameOverMusic);
        
    }
    private void OnDisable()
    {
        // Remove listeners to avoid duplicates
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    public void OnHomeButtonClicked()
    {

        //Debug.Log("Home Button Clicked");
        DataManager.Instance.SaveBestScoreIfNeeded();
        // Destroy(AppManager.Instance.GameLogicPrefab);

        // GamePlayManager.Instance.EndGame();

        // // Go to home screen - overlays will be hidden automatically after this frame
        // GameMechanics.Instance.ResetGameState();
        // AdManager.Instance.InterstitialShowAd();
        AdManager.Instance.ShowInterstitial(() =>
        {
            AppManager.Instance.ExitGame();
            AppStateManager.Instance.SetHome();
        });
        AppManager.Instance.ExitGame();
        // AdManager.Instance.InterstitialShowAd();
        // AppStateManager.Instance.SetHome();
        // AppStateManager.Instance.HideOverlay("FinalScorePopUp");
    }

    public void OnRetryButtonClicked()
    {
        //Debug.Log("Retry Button Clicked");
        // AdManager.Instance.InterstitialShowAd();
        AdManager.Instance.ShowInterstitial(() =>
        {
            AppStateManager.Instance.SetGameplay();
            AppManager.Instance.RestartGame();
            GameUIManager.Instance.RestartGame();
        });
        // AppStateManager.Instance.SetGameplay();
        // AppManager.Instance.RestartGame();
        // GameUIManager.Instance.RestartGame();
    }

    // public void UpdateScore(int score)
    // {
    //     //Debug.Log("Score is getting Updated");
    //     finalScoreText.text = "Final Score: " + score;
    // }
}