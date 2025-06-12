using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static Music instance;

    private AudioSource audioSource;
    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Makes it persist between scenes
        }
        else
        {
            Destroy(gameObject); // Only one allowed
            return;
        }

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
        {
            audioSource.mute = isMuted;
        }
    }

    public bool IsMusicMuted() => isMuted;
}
