using UnityEngine;

public class GameController : MonoBehaviour, IObserveGameLostPanelChange, IObserveIngamePanelChange, IObserveSettingsPanelChange
{

    public void Awake()
    {
        EventManager.Instance.GameContinued += OnToIngamePanel;
        EventManager.Instance.GameFailed += OnToGameLostPanel;
        EventManager.Instance.SettingsButtonClicked += OnToSettingsPanel;
    }

    public void OnToGameLostPanel()
    {
        Time.timeScale = 0f; // Pause
    }

    public void OnToIngamePanel()
    {
        Time.timeScale = 1f; // Resume
    }

    public void OnToSettingsPanel()
    {
        Time.timeScale = 0f; // Pause
    }
}