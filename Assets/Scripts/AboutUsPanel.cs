using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutUsPanel : MonoBehaviour
{
    [SerializeField] private Button CrossButton;
    [SerializeField] private AudioClip buttonClickSound;

    private void Start()
    {

        CrossButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            PanelManager.instance.ShowMainMenu();
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


