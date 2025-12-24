
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
    }
    private void OnDisable()
    {
        // Remove listeners to avoid duplicates
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        retryButton.onClick.RemoveListener(OnRetryButtonClicked);
    }

    public void OnHomeButtonClicked()
    {

        Debug.Log("Home Button Clicked");
        // DataManager.Instance.SaveBestScoreIfNeeded();
        // GamePlayManager.Instance.EndGame();

        // // Go to home screen - overlays will be hidden automatically after this frame
        // AppStateManager.Instance.SetHome();
        // AppStateManager.Instance.HideOverlay("FinalScorePopUp");
    }

    public void OnRetryButtonClicked()
    {
        Debug.Log("Retry Button Clicked");
        AppStateManager.Instance.SetGameplay();
        AppManager.Instance.RestartGame();
        UIManager.Instance.RestartGame();
    }

    // public void UpdateScore(int score)
    // {
    //     Debug.Log("Score is getting Updated");
    //     finalScoreText.text = "Final Score: " + score;
    // }
}