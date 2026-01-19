using UnityEngine;
 
public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
 
    public AudioSource musicSource;
    public AudioSource sfxSource;
 
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
 
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip == null) return;    
        sfxSource.PlayOneShot(clip);
    }

    public void ToggleMusic(bool isOn)
    {
        musicSource.mute = !isOn;
    }
 
    public void ToggleSFX(bool isOn)
    {
        sfxSource.mute = !isOn;
    }
}