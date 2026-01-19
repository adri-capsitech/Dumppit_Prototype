using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject HomeScreenPanel;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        HomeScreenPanel.SetActive(true);
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDisable()
    {
        RemoveAllListener();
        HomeScreenPanel.SetActive(false);
    }
    void RemoveAllListener()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        RemoveAllListener();
        AppManager.Instance.StartGame();

        // // rsLogo moves UP
        // seq.Join(rsLogo.DOAnchorPosY(rsLogoTargetY, duration).SetEase(Ease.InOutQuad));

        // //childcar moves DOWN

        // childcar.gameObject.SetActive(false);

        // RectTransform obj = Instantiate(childcar, childcar.transform.position, Quaternion.identity, rsLogo.transform);
        // obj.gameObject.SetActive(true);
        // seq.Join(obj.DOAnchorPosY(childcarTargetY, duration).SetEase(Ease.InOutQuad));
        // // After both animations complete → Load Gameplay
        // seq.OnComplete(() =>
        // {
        //     AfterHomeanimationComplition();
        // });


        // AudioManager.Instance.PlayMusic("Click");


    }

}

// using UnityEngine;
// using UnityEngine.UI;

// public class HomeUI : MonoBehaviour
// {
//     [SerializeField] private Button playButton;

//     private void OnEnable()
//     {
//         Time.timeScale = 1f;
//         playButton.onClick.AddListener(OnPlayClicked);
//     }

//     private void OnDisable()
//     {
//         playButton.onClick.RemoveListener(OnPlayClicked);
//     }

//     private void OnPlayClicked()
//     {
//         AppManager.Instance.StartGame();
//     }
// }
