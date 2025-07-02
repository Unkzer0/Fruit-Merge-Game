using UnityEngine;
using UnityEngine.UI;

public class ResetDataButton : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private AudioClip clickSound;

    private void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(() =>
            {
                PlayClickSound();
                ResetAllPlayerPrefs();
            });
        }
    }

    private void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs deleted.");
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            SoundManager.instance?.PlayButtonClick(clickSound);
        }
    }
}
