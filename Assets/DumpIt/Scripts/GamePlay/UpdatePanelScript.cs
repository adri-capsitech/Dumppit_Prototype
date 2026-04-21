using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanelScript : MonoBehaviour
{
    public static UpdatePanelScript instance;

    [SerializeField] private GameObject UpdatePanel;
    [SerializeField] private Button updateBtn;
    [SerializeField] private TMP_Text LoadingTxt;
    public string playStoreUrl;
    public string appStoreUrl;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        updateBtn.onClick.AddListener(() =>
        {
#if UNITY_ANDROID
        Application.OpenURL(playStoreUrl);
#elif UNITY_IOS
            Application.OpenURL(appStoreUrl);
#endif
        });
        //  gameObject.SetActive(false);
    }
    public void setLoading(bool isTrue)
    {
        if (isTrue)
        {
            LoadingTxt.gameObject.SetActive(true);
            UpdatePanel.SetActive(false);
        }
        else LoadingTxt.gameObject.SetActive(false);
    }

    public void ShowForceUpdatePopup()
    {
        if (UpdatePanel != null)
        {
            UpdatePanel.SetActive(true);
        }
    }

    public void ShowOptionalUpdatePopup()
    {

        if (UpdatePanel != null)
        {
            UpdatePanel.SetActive(true);
        }
    }

}
