using UnityEngine;

public class GameController : MonoBehaviour
{

    public void Awake()
    {
        EventManager.Instance.GameContinued += GameContinue;
        EventManager.Instance.GameFailed += GameOver;
        EventManager.Instance.GamePaused += GamePaused;
    }
    public void GameOver()
    {

    }

    public void GameContinue()
    {
        Time.timeScale = 1f; // Resume
    }

    public void GamePaused()
    {
        Time.timeScale = 0f; // Pause
    }


}