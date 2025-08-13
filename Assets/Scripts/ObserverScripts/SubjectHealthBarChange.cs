using System;

public class SubjectHealthBarChange
{
    public static SubjectHealthBarChange Instance { get; private set; } = new();

    public event Action<int> TellHealthBarChange;

    private SubjectHealthBarChange()
    {
        Instance = this;
    }

    ~SubjectHealthBarChange()
    {
        Instance = null;
    }

    public void AddObserverTellHealthBarChange(Action<int> observer)
    {
        TellHealthBarChange += observer;
    }

    public void RemoveObserverTellHealthBarChange(Action<int> observer)
    {
        TellHealthBarChange -= observer;
    }

    public void NotifyObserversTellHealthBarChange(int currHealth)
    {
        TellHealthBarChange?.Invoke(currHealth);
    }
}