using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    public static Music instance;

    private AudioSource audioSource;
    private bool isMuted;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        ApplyMuteState();
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        ApplyMuteState();
    }

    private void ApplyMuteState()
    {
        if (audioSource != null)
            audioSource.mute = isMuted;
    }

    public bool IsMusicMuted() => isMuted;
}
