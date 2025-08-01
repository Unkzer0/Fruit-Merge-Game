using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{
    [SerializeField] private Button crossButton;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private ThemeData[] availableThemes; // List of themes
    [SerializeField] private Button[] themeButtons; // Buttons to select themes

    private void Start()
    {
        crossButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            gameObject.SetActive(false);
        });

        // Assign theme buttons
        for (int i = 0; i < themeButtons.Length; i++)
        {
            int index = i; // Capture index for closure
            themeButtons[i].onClick.AddListener(() =>
            {
                PlayClickSound();
                ApplyTheme(index);
            });
        }
    }

    private void ApplyTheme(int index)
    {
        if (index >= 0 && index < availableThemes.Length)
        {
            ThemeManager.instance?.ApplyTheme(availableThemes[index]);
        }
    }

    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(buttonClickSound);
        }
    }
}
