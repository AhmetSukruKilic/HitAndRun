using System;

public class SubjectObstacleHitKing
{
    public static SubjectObstacleHitKing Instance { get; private set; } = new();

    public event Action TellObstacleHitKing;

    private SubjectObstacleHitKing()
    {
        Instance = this;
    }

    ~SubjectObstacleHitKing()
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