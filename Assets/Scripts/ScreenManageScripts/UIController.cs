using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private List<UIPanel> _panels = new List<UIPanel>();

    private void Awake()
    {
        EventManager.Instance.GameFailed += OnGameFailed;
        EventManager.Instance.SettingsButtonClicked += OnSettingsButtonClicked;
        EventManager.Instance.GameContinued += OnGameContinue;

        OpenPanel(PanelType.InGame);
    }

    private void OnSettingsButtonClicked()
    {
        OpenPanel(PanelType.Settings);
    }

    private void OnGameFailed()
    {
        OpenPanel(PanelType.GameOver);
    }

    private void OnGameContinue()
    {
        OpenPanel(PanelType.InGame);
    }

    public void OpenPanel(PanelType panelType)
    {
        _panels.ForEach(p => p.gameObject.SetActive(false));

        _panels.Find(p => p.PanelType == panelType)?.gameObject.SetActive(true);
    }
}
