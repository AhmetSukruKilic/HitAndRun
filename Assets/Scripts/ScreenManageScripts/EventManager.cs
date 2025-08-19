using System;

public class EventManager
{
    public static EventManager Instance { get; private set; } = new EventManager();
    
    public event Action GameFailed;
    public event Action SettingsButtonClicked;
    public event Action GameContinued;
    public event Action GamePaused;
    
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
        GamePaused?.Invoke();
    }

    public void GameContinue()
    {  
        GameContinued?.Invoke();
    }

    public void GamePause()
    {
        GamePaused?.Invoke();
    }

    ~EventManager()
    {
        Instance = null;
    }
}