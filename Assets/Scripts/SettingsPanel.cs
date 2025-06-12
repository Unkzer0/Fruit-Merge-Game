using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button sfxToggleButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;

  
    private void Start()
    {
        musicToggleButton.onClick.AddListener(() =>
        {
            Music.instance.ToggleMusic();
          
        });

        sfxToggleButton.onClick.AddListener(() =>
        {
            MergeManager.instance.ToggleSFX();
            FindObjectOfType<FruitDropperController>().ToggleSFX(); // safer than static if not singleton
      
        });

        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        resumeButton.onClick.AddListener(() =>
        {
            PanelManager.instance.ShowOnly(null);
        });

  
    }

   
}
