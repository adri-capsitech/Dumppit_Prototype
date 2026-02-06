using TMPro;
using UnityEngine;

public class LangDropdowncontroller : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private readonly string[] languageCodes =
    {
        "en",
        "pt-BR",
        "ru",
        "sp",
        "fr"
    };

    void Start()
    {

        string savedLang = PlayerPrefs.GetString("language", "en");
        int index = System.Array.IndexOf(languageCodes, savedLang);

        if (index >= 0)
        {
            dropdown.SetValueWithoutNotify(index);
        }
    }

    public void OnLanguageChanged(int index)
    {
        if (index < 0 || index >= languageCodes.Length)
            return;

        LocalizationManager.Instance.SetLanguage(languageCodes[index]);
    }
}
