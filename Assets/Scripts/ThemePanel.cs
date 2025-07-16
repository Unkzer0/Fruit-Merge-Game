using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{
    [SerializeField] private Button crossButton;
    [SerializeField] private AudioClip buttonClickSound;
    void Start()
    {
        crossButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            PanelManager.instance.ShowOnly(null);
        });
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }
}
