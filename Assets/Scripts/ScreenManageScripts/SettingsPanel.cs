using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : UIPanel
{
    public override PanelType PanelType => PanelType.Settings;

    [SerializeField]
    private Button _resumeButton;

    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Slider _volumeSlider;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClicked);
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnResumeButtonClicked()
    {
        EventManager.Instance.InGameGo();
    }

    private void OnRestartButtonClicked()
    {
        EventManager.Instance.RestartGame();
    }

    private void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

}