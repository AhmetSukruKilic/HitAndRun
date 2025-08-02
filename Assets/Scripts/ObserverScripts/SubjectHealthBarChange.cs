using System;

public class SubjectHealthBarChange
{
    public static SubjectHealthBarChange Instance { get; private set; } = new();

    public event Action TellHealthBarChange;

    private SubjectHealthBarChange()
    {
        Instance = this;
    }

    ~SubjectHealthBarChange()
    {
        Instance = null;
    }

    public void AddObserverTellHealthBarChange(Action observer)
    {
        TellHealthBarChange += observer;
    }

    public void RemoveObserverTellHealthBarChange(Action observer)
    {
        TellHealthBarChange -= observer;
    }

    public void NotifyObserversTellHealthBarChange()
    {
        TellHealthBarChange?.Invoke();
    }
}