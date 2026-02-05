using UnityEngine;
using UnityEngine.UI;

public class AudioToggleUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite SoundOn;
    public Sprite SoundOff;
    public Sprite MusicOn;
    public Sprite MusicOff;
    bool isSoundOn;
    bool isMusicOn;
    [Header("UI Images")]
    public Image soundImage;
    public Image musicImage;

    void Start()
    {
       
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        // ApplySFX();
        // ApplyMusic();
        UpdateSFXUI();
        UpdateMusicUI();
    }

    // ---------- SFX ----------
    public void ToggleSFX()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);

        ApplySFX();
        UpdateSFXUI();
    }

    void ApplySFX()
    {
        AudioController.Instance.ToggleSFX(isSoundOn);
    }

    void UpdateSFXUI()
    {
        soundImage.sprite = isSoundOn ? SoundOn : SoundOff;
    }

    // ---------- MUSIC ----------
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);

        ApplyMusic();
        UpdateMusicUI();
    }

    void ApplyMusic()
    {
        AudioController.Instance.ToggleMusic(isMusicOn);
    }

    void UpdateMusicUI()
    {
        musicImage.sprite = isMusicOn ? MusicOn : MusicOff;
    }
}
