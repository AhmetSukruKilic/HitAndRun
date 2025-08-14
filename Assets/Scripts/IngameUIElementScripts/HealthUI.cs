using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class HealthUI : IObserveHealthBarChange, IDisposable
{
    private readonly Transform _heartsParent;
    private readonly GameObject _heartContainerPrefab;
    private readonly List<GameObject> _heartContainers = new();
    private readonly List<Image> _heartFills = new();

    private const int WIDTH = 67; // px offset if you aren't using a LayoutGroup

    public static HealthUI Instance { get; private set; }

    public int MaxHealth => _heartContainers.Count;

    public HealthUI(Transform heartsParent, GameObject heartContainerPrefab)
    {
        _heartsParent = heartsParent;
        _heartContainerPrefab = heartContainerPrefab;
    }

    /// <summary>Build hearts and subscribe to health change notifications.</summary>
    public void Init(int maxHealth)
    {
        // Replace prior instance if any
        Instance?.Dispose();
        Instance = this;

        // Subscribe to your subject/bus
        SubjectHealthBarChange.Instance.AddObserverTellHealthBarChange(OnNotifyHealthBarChange);

        BuildHearts(maxHealth);
    }

    public void Dispose()
    {
        // Unsubscribe and destroy UI
        SubjectHealthBarChange.Instance.RemoveObserverTellHealthBarChange(OnNotifyHealthBarChange);
        ClearHearts();

        if (ReferenceEquals(Instance, this))
            Instance = null;
    }

    private void BuildHearts(int maxHealth)
    {
        ClearHearts();

        for (int i = 0; i < maxHealth; i++)
        {
            // NOTE: using UnityEngine.Object.Instantiate from a non-MonoBehaviour is fine
            GameObject go = UnityEngine.Object.Instantiate(_heartContainerPrefab, _heartsParent, false);
            // If you're not using a HorizontalLayoutGroup, offset manually:
            go.transform.localPosition += new Vector3(i * WIDTH, 0, 0);

            _heartContainers.Add(go);

            var fill = go.transform.Find("HeartFill")?.GetComponent<Image>();
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
}
