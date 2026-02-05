using TMPro;
using UnityEngine;

public class LangDropdowncontroller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI output;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLanguageChanged(int index)
    {
        switch (index)
        {
            case 0:
                LocalizationManager.Instance.SetLanguage("en");
                break;

            case 1:
                LocalizationManager.Instance.SetLanguage("pt-BR");
                break;

            case 2:
                LocalizationManager.Instance.SetRussian();
                break;

            case 3:
                LocalizationManager.Instance.SetSpanish();
                break;
            case 4:
                LocalizationManager.Instance.SetLanguage("fr");
                break;
        }
    }
}
