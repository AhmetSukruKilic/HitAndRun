using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class InGamePanel : UIPanel
{
    public override PanelType PanelType { get => PanelType.InGame; }

    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Transform _heartsParent;
    [SerializeField]
    private GameObject _heartContainerPrefab;

    private List<IInGameUIElement> _inGameUIs = new();

    void Awake()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);

        _inGameUIs.Add(new HealthUI(_heartsParent, _heartContainerPrefab));
    }

    private void OnDisable()
    {
        foreach (var ui in _inGameUIs)
        {
            ui.Dispose();
        }
    }

    private void OnSettingsButtonClicked()
    {
        EventManager.Instance.SettingsButtonClick();
    }

    public IInGameUIElement GetUI<T>(string UIElementName) where T : IInGameUIElement
    {
        return _inGameUIs.OfType<T>().FirstOrDefault(ui => ui.GetName() == UIElementName.ToLowerInvariant());
    }
}


public interface IInGameUIElement
{
    void Dispose();
    string GetName();
}

public sealed class HealthUI : IObserveHealthBarChange, IDisposable, IInGameUIElement
{
    private readonly Transform _heartsParent;
    private readonly GameObject _heartContainerPrefab;
    private readonly List<GameObject> _heartContainers = new();
    private readonly List<Image> _heartFills = new();

    private const int WIDTH = 67; // px offset if you aren't using a LayoutGroup

    public static HealthUI Instance { get; private set; }

    public int MaxHealth => _heartContainers.Count;

    private bool _isHeartsSet = false;

    public HealthUI(Transform heartsParent, GameObject heartContainerPrefab)
    {
        _heartsParent = heartsParent;
        _heartContainerPrefab = heartContainerPrefab;

        SubjectHealthBarChange.Instance.AddObserverTellHealthBarChange(OnNotifyHealthBarChange);
    }

    /// <summary>Build hearts and subscribe to health change notifications.</summary>
    public void Init(int maxHealth)
    {
        // Replace prior instance if any
        Instance?.Dispose();
        Instance = this;

        // Subscribe to your subject/bus

        BuildHearts(maxHealth);
        _isHeartsSet = true;
    }

    public void Dispose()
    {
        // Unsubscribe and destroy UI
        SubjectHealthBarChange.Instance.RemoveObserverTellHealthBarChange(OnNotifyHealthBarChange);
        ClearHearts();

        if (ReferenceEquals(Instance, this))
            Instance = null;
        _isHeartsSet = false;
    }

    private void BuildHearts(int maxHealth)
    {
        ClearHearts();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = UnityEngine.Object.Instantiate(_heartContainerPrefab, _heartsParent, false);
            heart.transform.localPosition += new Vector3(i * WIDTH, 0, 0);

            _heartContainers.Add(heart);

            var fill = heart.transform.Find("HeartFill")?.GetComponent<Image>();
            if (fill == null)
                Debug.LogError("HealthUI: 'HeartFill' child Image not found on heartContainerPrefab.");
            _heartFills.Add(fill);
        }
    }

    private void ClearHearts()
    {
        // Destroy container GOs; no need to destroy Image separately
        for (int i = 0; i < _heartContainers.Count; i++)
        {
            if (_heartContainers[i])
                UnityEngine.Object.Destroy(_heartContainers[i]);
        }
        _heartContainers.Clear();
        _heartFills.Clear();
    }

    // ----- Observer callback -----
    // If you want half-hearts, change the parameter type in your subject/interface to float.
    public void OnNotifyHealthBarChange(int currHealth)
    {
        if (!_isHeartsSet)
        {
            Init(currHealth);
            Debug.LogWarning("HealthUI: Hearts initialized.");
            return;
        }

        SetFilledHearts(currHealth);
    }

    // Integer hearts version
    private void SetFilledHearts(int currHealth)
    {
        for (int i = 0; i < MaxHealth; i++)
        {
            if (_heartFills[i] == null) continue;
            _heartFills[i].fillAmount = (i < currHealth) ? 1f : 0f;
        }
    }

    // Optional: fractional hearts support (call this from a float-based observer)
    public void SetFilledHeartsFloat(float currHealth)
    {
        int full = Mathf.FloorToInt(currHealth);
        for (int i = 0; i < MaxHealth; i++)
        {
            if (_heartFills[i] == null) continue;

            if (i < full) _heartFills[i].fillAmount = 1f;
            else if (i == full) _heartFills[i].fillAmount = Mathf.Clamp01(currHealth - full);
            else _heartFills[i].fillAmount = 0f;
        }
    }

    public string GetName()
    {
        return "healthui";
    }
}










