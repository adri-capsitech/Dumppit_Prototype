using UnityEngine;
using UnityEngine.UI;

public class AudioToggleUI : MonoBehaviour
{
    public Sprite SoundOn;
    public Sprite SoundOff;

    private Image soundImage;
    private bool isSoundOn = true;

    void Awake()
    {
        soundImage = GetComponent<Image>();
    }

    void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        UpdateUI();
        ApplySound();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        ApplySound();
        UpdateUI();  
    }

    void ApplySound()
    {
        AudioController.Instance.ToggleMusic(isSoundOn);
        AudioController.Instance.ToggleSFX(isSoundOn);
    }

    void UpdateUI()
    {
        Debug.Log("Called");
        soundImage.sprite = isSoundOn ? SoundOn : SoundOff;
    }
}