using UnityEngine;

public class GameController : MonoBehaviour
{
    public void GameOver()
    {
        EventManager.Instance.GameOver();
    }

    public void GameContinue()
    {
        EventManager.Instance.GameContinue();
    }
}