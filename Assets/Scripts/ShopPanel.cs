using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button crossButton;
    [SerializeField] private AudioClip buttonClickSound;
    void Start()
    {
        crossButton.onClick.AddListener(() =>
        {
            PlayClickSound();
            gameObject.SetActive(false);
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
