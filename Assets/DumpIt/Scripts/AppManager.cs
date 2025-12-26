using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    public GameObject GameLogicPrefab;
    [SerializeField]
    private GameObject HomeScreen;

    private GameObject GameLogic;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AppStateManager.Instance.SetHome();
        //  AppStateManager.Instance.SetGameplay();
        //this.StartGame();
    }

    public void StartGame()
    {

        AppStateManager.Instance.SetGameplay();
        if (GameLogic == null)
        {
            GameLogic = Instantiate(GameLogicPrefab);
        }
        GameLogic.SetActive(true);
        DataManager.Instance.ResetScore();
    }

    public void ExitGame()
    {

        DestroyGameLogic();

    }
    private void DestroyGameLogic()
    {
        if (GameLogic != null)
        {
            Destroy(GameLogic);
            GameLogic = null;
        }
    }

    public void RestartGame()
    {
        StartGame();
    }


}

