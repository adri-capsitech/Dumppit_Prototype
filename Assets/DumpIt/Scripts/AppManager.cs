using UnityEngine;

public class AppManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private GameObject GameLogicPrefab;
    private GameObject GameLogic;
    void Start()
    {
        AppStateManager.Instance.SetGameplay();
        this.StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
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
            Destroy(GameLogic);
        }
    }
}
