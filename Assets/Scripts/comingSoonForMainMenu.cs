using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class comingSoonForMainMenu : MonoBehaviour
{
    [SerializeField] private Button crossButton;
    void Start()
    {
        crossButton.onClick.AddListener(() =>
        {
            PanelManager.instance.ShowMainMenu();
        });
    }
}
