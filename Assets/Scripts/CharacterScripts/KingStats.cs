public class KingStats: IObserveTellObstacleHitKing
{
    private float _moveSpeed = 4f;

    private float _jumpForce = 5f;

    public float MoveSpeed { get => _moveSpeed; private set => _moveSpeed = value; }
    public float JumpForce { get => _jumpForce; private set => _jumpForce = value; }

    
    private int _currHealth;
    public int CurrHealth { get => _currHealth;
                        set { _currHealth = value; SubjectHealthBarChange.Instance.NotifyObserversTellHealthBarChange(); } }

    private int _maxHealth = 3;
    private int _minHealth = 0;

    public int MaxHealth { get => _maxHealth; }
    public int MinHealth { get => _minHealth; }

    public KingStats()
    {
        CurrHealth = MaxHealth;
        
        SubjectObstacleHitKing.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }
    
    public void Heal()
    {
        if (CurrHealth >= MaxHealth)
            return;
        CurrHealth += 1;
    }

    public void TakeDamage()
    {
        if (CurrHealth <= MinHealth)
            return;
        CurrHealth -= 1;
    }

    public void OnNotifyTellObstacleHitKing()
    {
        TakeDamage();
    }
}
