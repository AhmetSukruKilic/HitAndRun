using System;

public class EventManager
{
    public static EventManager Instance { get; private set; } = new EventManager();
    
    public event Action GameFailed;
    public event Action SettingsButtonClicked;
    public event Action GameRestarted;
    public event Action IngameGoes;
    
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

    public void InGameGo()
    {
        IngameGoes?.Invoke();
    }

    public void RestartGame()
    {
        GameRestarted?.Invoke();
        IngameGoes?.Invoke();
    }

    ~EventManager()
    {
        Instance = null;
    }
}