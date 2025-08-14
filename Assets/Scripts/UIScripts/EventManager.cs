using System;

public class EventManager
{
    public static EventManager Instance { get; private set; } = new EventManager();
    
    public event Action GameFailed;
    public event Action SettingsButtonClicked;
    
    private EventManager()
    {
        Instance = this;
    }

    public void GameOver()
    {
        GameFailed?.Invoke();
    }

    public void SettingsButtonClick()
    {
        SettingsButtonClicked?.Invoke();
    }

    ~EventManager()
    {
        Instance = null;
    }
}