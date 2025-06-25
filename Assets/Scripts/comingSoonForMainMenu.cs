using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class comingSoonForMainMenu : MonoBehaviour
{
    [SerializeField] private Button crossButton;
    [SerializeField] private AudioClip buttonClickSound;
    void Start()
    {
        crossButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            PanelManager.instance.ShowMainMenu();
        });
    }
    private void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
}
