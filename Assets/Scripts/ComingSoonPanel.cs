using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComingSoonPanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    void Start()
    {
        resumeButton.onClick.AddListener(() =>
        {
            PanelManager.instance.ShowOnly(null);
        });
    }
}
