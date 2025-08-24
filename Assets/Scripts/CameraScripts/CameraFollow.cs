using UnityEngine;

public class CameraFollow : MonoBehaviour, IObserveGameLostPanelChange, IObserveIngamePanelChange, IObserveSettingsPanelChange
{
    private static CameraFollow _instance;
    private Vector3 _initialPosition;
    public Vector3 InitialPosition => _instance._initialPosition;
    private readonly Vector3 _goUp = new Vector3(0, 25, 0);
    private Vector3 GoUp => _instance._goUp;

    public void OnToGameLostPanel()
    {
        transform.position = InitialPosition + GoUp;
    }

    public void OnToIngamePanel()
    {
        transform.position = InitialPosition;
    }

    public void OnToSettingsPanel()
    {
        transform.position = InitialPosition + GoUp;
    }

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        _initialPosition = transform.position;

        EventManager.Instance.IngameGoes += OnToIngamePanel;
        EventManager.Instance.GameFailed += OnToGameLostPanel;
        EventManager.Instance.SettingsButtonClicked += OnToSettingsPanel;

        DontDestroyOnLoad(gameObject);
    }


}
