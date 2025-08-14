public class UIController : MonoBehaviour
{
    [SerializeField] 
    private List<UIPanel> _panels = new List<UIPanel>();

    private void Awake()
    {
        EventManager.Instance.GameFailed += OnGameFailed;
        EventManager.Instance.SettingsButtonClicked += OnSettingsButtonClicked;
        
        for (int i = 0; i < _panels.Count; i++)
        {
            if (_panels[i].PanelType == PanelType.InGame)
            {
                _panels[i].gameObject.SetActive(true);
            }
            else
            {
                _panels[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnSettingsButtonClicked()
    {
        OpenPanel(PanelType.Settings);
    }

    private void OnGameFailed()
    {
        OpenPanel(PanelType.GameOver);
    }

    public void OpenPanel(PanelType panelType)
    {
        _panels.ForEach(p => p.gameObject.SetActive(false));
        
        _panels.Find(p => p.PanelType == panelType)?.gameObject.SetActive(true);
    }
}
