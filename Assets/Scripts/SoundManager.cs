using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick(AudioClip clip)
    {
        if (clip == null || IsSFXMuted()) return;
        sfxSource.PlayOneShot(clip);
    }

    private bool IsSFXMuted()
    {
        return PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }
}
