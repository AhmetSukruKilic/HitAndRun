using System;

public class Subject
{
    public static Subject Instance { get; private set; } = new();

    public event Action TellObstacleHitKing;

    private Subject()
    {
        Instance = this;
    }

    ~Subject()
    {
        Instance = null;
    }

    public void AddObserverTellObstacleHitKing(Action observer)
    {
        TellObstacleHitKing += observer;
    }

    public void RemoveObserverTellObstacleHitKing(Action observer)
    {
        TellObstacleHitKing -= observer;
    }

    public void NotifyObserversTellObstacleHitKing()
    {
        TellObstacleHitKing?.Invoke();
    }
}