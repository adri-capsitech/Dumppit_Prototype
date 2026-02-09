using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button privacyPolicyButton;
    public Image imagepanel;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackClicked);
        privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyClicked);
        imagepanel.enabled = true;
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackClicked);
        privacyPolicyButton.onClick.RemoveListener(OnPrivacyPolicyClicked);
        imagepanel.enabled = false;
    }

    public void OnBackClicked()
    {
        AppStateManager.Instance.HideOverlay("Settings");
        AppStateManager.Instance.SetHome();
        imagepanel.enabled = false;
    }

    void OnPrivacyPolicyClicked()
    {
        Application.OpenURL("http://www.thegamewise.com/privacy-policy/");
    }
}