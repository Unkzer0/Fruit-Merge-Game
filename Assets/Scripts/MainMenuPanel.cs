using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private AudioClip buttonClickSound;  // Assign in Inspector

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            StartCoroutine(HidePanelAfterDelay(0.45f));
        });
    }

    private IEnumerator HidePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PanelManager.instance.ShowOnly(null);
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            // Plays the sound at the camera's position
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
}


