using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : UIPanel
{
    public override PanelType PanelType => PanelType.InGame;

    [SerializeField]
    private TMP_Text _scoreText;
    
    [SerializeField]
    private Button _settingsButton;

    private void Awake()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnSettingsButtonClicked()
    {
        EventManager.Instance.SettingsButtonClick();
    }
}