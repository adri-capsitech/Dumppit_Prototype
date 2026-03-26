using System.Collections;
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
    private Coroutine soundRoutine;
    private Coroutine musicRoutine;

    void Start()
    {

        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        // ApplySFX();
        // ApplyMusic();
        UpdateSFXUIInstant();
        UpdateMusicUIInstant();
    }

    // ---------- SFX ----------
    public void ToggleSFX()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);

        ApplySFX();
        UpdateSFXUI();
    }
    void UpdateSFXUIInstant()
    {
        soundImage.sprite = isSoundOn ? SoundOn : SoundOff;
    }

    void ApplySFX()
    {
        AudioController.Instance.ToggleSFX(isSoundOn);
    }

    void UpdateSFXUI()
    {
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);

        soundRoutine = StartCoroutine(AnimateToggle(soundImage, isSoundOn ? SoundOn : SoundOff));
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
        if (musicRoutine != null)
            StopCoroutine(musicRoutine);

        musicRoutine = StartCoroutine(AnimateToggle(musicImage, isMusicOn ? MusicOn : MusicOff));
    }
    void UpdateMusicUIInstant()
    {
        musicImage.sprite = isMusicOn ? MusicOn : MusicOff;
    }


    IEnumerator AnimateToggle(Image img, Sprite newSprite)
    {
        float duration = 0.1f;
        float t = 0f;

        Vector3 originalScale = img.transform.localScale;
        Vector3 smallScale = originalScale * 0.8f;
        Vector3 bigScale = originalScale * 1.1f;

        // shrink
        while (t < duration)
        {
            t += Time.deltaTime;
            img.transform.localScale = Vector3.Lerp(originalScale, smallScale, t / duration);
            yield return null;
        }

        // swap sprite at smallest point
        img.sprite = newSprite;

        // expand with slight bounce
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            img.transform.localScale = Vector3.Lerp(smallScale, bigScale, t / duration);
            yield return null;
        }

        // return to normal
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            img.transform.localScale = Vector3.Lerp(bigScale, originalScale, t / duration);
            yield return null;
        }

        img.transform.localScale = originalScale;
    }
}
