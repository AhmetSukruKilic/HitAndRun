using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, IObserveGameLostPanelChange, IObserveIngamePanelChange, IObserveSettingsPanelChange
{
    private static GameController _instance;

    public void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        EventManager.Instance.GameFailed += OnToGameLostPanel;
        EventManager.Instance.SettingsButtonClicked += OnToSettingsPanel;
        EventManager.Instance.GameRestarted += OnRestartGame;
        EventManager.Instance.IngameGoes += OnToIngamePanel;

        DontDestroyOnLoad(gameObject);
    }

    private void GamePaused()
    {
        Time.timeScale = 0f; // Pause
    }
    private void GameContinued()
    {
        Time.timeScale = 1f; // Resume
    }

    public void OnToGameLostPanel()
    {
        GamePaused();
    }

    public void OnToIngamePanel()
    {
        GameContinued();
    }

    public void OnToSettingsPanel()
    {
        GamePaused();
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the game by reloading the current scene
        GameContinued();
    }
}