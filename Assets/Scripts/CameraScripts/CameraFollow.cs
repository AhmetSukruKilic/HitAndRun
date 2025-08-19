using UnityEngine;

public class CameraFollow : MonoBehaviour, IObserveGameLostPanelChange, IObserveIngamePanelChange, IObserveSettingsPanelChange
{
    private Vector3 _initialPosition;

    private readonly Vector3 _goUp = new Vector3(0, 25, 0);

    public void OnToGameLostPanel()
    {
        transform.position = _initialPosition + _goUp;
    }

    public void OnToIngamePanel()
    {
        transform.position = _initialPosition;
    }

    public void OnToSettingsPanel()
    {
        transform.position = _initialPosition + _goUp;
    }

    void Start()
    {
        _initialPosition = transform.position;

        EventManager.Instance.GameContinued += OnToIngamePanel;
        EventManager.Instance.GameFailed += OnToGameLostPanel;
        EventManager.Instance.SettingsButtonClicked += OnToSettingsPanel;
    }


}
