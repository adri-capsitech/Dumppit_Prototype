using UnityEngine;
using UnityEngine.EventSystems;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Instance { get; private set; }

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
    public void GameOver()
    {
        DataManager.Instance.SaveBestScoreIfNeeded();
        UIManager.Instance.DisplayGameOverPanel();
    }
    public void Restart()
    {

        DataManager.Instance.ResetScore();
        GameMechanics.Instance.StartGame();
    }

}


