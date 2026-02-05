using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button privacyPolicyButton;
    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackClicked);
        privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClicked);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackClicked);
        privacyPolicyButton.onClick.RemoveListener(OnPrivacyPolicyClicked);
    }

    public void OnBackClicked()
    {
          AppStateManager.Instance.HideOverlay("Settings");
        AppStateManager.Instance.SetHome();
    }

    void OnPrivacyPolicyClicked()
    {
        Application.OpenURL("http://www.thegamewise.com/privacy-policy/");
    }
}