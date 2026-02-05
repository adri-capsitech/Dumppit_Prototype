using UnityEngine;
using UnityEngine.UI;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;

    void Start()
    {
        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        AudioController.Instance.PlaySFX(clickSound);
    }
}
