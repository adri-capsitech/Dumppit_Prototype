
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
        if (AudioController.Instance != null)
            AudioController.Instance.PlayMusic(gameOverMusic);

    }
    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    public void OnHomeButtonClicked()
    {
        DataManager.Instance.SaveBestScoreIfNeeded();
        AdManager.Instance.ShowInterstitial(() =>
        {
            AppManager.Instance.ExitGame();
            AppStateManager.Instance.SetHome();
        });
        AppManager.Instance.ExitGame();

    }

    public void OnRetryButtonClicked()
    {
        AdManager.Instance.ShowInterstitial(() =>
        {
            AppStateManager.Instance.SetGameplay();
            AppManager.Instance.RestartGame();
            GameUIManager.Instance.RestartGame();
        });

    }


}