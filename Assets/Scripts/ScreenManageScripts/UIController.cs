using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IObserveGameLostPanelChange, IObserveIngamePanelChange, IObserveSettingsPanelChange
{
    [SerializeField]
    private List<UIPanel> _panels = new List<UIPanel>();
    public List<UIPanel> Panels => _instance._panels;
    private static UIController _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        if (_panels == null || _panels.Count == 0)
        {
            return;
        }

        EventManager.Instance.GameFailed += OnToGameLostPanel;
        EventManager.Instance.SettingsButtonClicked += OnToSettingsPanel;
        EventManager.Instance.IngameGoes += OnToIngamePanel;

        OpenPanel(PanelType.InGame);

        DontDestroyOnLoad(gameObject);
    }

    public void OnToSettingsPanel()
    {
        OpenPanel(PanelType.Settings);
    }

    public void OnToGameLostPanel()
    {
        OpenPanel(PanelType.GameOver);
    }

    public void OnToIngamePanel()
    {
        OpenPanel(PanelType.InGame);
    }

    public void OpenPanel(PanelType panelType)
    {
        Panels.ForEach(p => p.gameObject.SetActive(false));

        Panels.Find(p => p.PanelType == panelType)?.gameObject.SetActive(true);
    }

}
