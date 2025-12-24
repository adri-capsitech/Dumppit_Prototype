using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    [SerializeField] private GameObject GameLogicPrefab;
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


        // this.StartGame();
    }

    public void StartGame()
    {

        AppStateManager.Instance.SetGameplay();
        if (GameLogic == null)
        {
            GameLogic = Instantiate(GameLogicPrefab);
        }
        GameLogic.SetActive(true);
    }

    public void ExitGame()
    {
        if (GameLogic != null)
        {
            GameLogic.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GameLogic.SetActive(true);
    }


}

