using UnityEngine;

public class KingControler : MonoBehaviour, IObserveTellObstacleHitKing
{
    [SerializeField] private Transform heartsParent;         
    [SerializeField] private GameObject heartContainerPrefab; 

    private KingStats _kingStats;
    public KingStats KingStats { get => _kingStats; set => _kingStats = value; }

    private Rigidbody _rb;

    private float _forwardTimer = 0;
    private bool _obstacleHitRecently = true;
    private Vector3 _forceVector;

    private const float FRICTION = 0.5f;
    private const float TIME_DURATION = 4;
    private readonly Vector3 START_SET = new(0, 0.1f, -0.3f);
    private const float TARGET_Z = 0f;


    void Awake()
    {
        KingStats = new KingStats();

        var _healthUI = new HealthUI(heartsParent, heartContainerPrefab);
        _healthUI.Init(KingStats.MaxHealth);

        
        transform.position = START_SET;

        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(horizontal * KingStats.MoveSpeed, _rb.linearVelocity.y, 0);
        _rb.linearVelocity = move;

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            _rb.AddForce(Vector3.up * KingStats.JumpForce, ForceMode.Impulse);
        }

        if (_obstacleHitRecently)
        {
            _forwardTimer += Time.deltaTime;

            if (_forwardTimer >= TIME_DURATION / 2)
            {
                MoveForward();
            }

            if (_forwardTimer >= TIME_DURATION)
            {
                ResetToDefaultPosition();
                _forwardTimer = 0;
                _obstacleHitRecently = false;
            }
        }
    }
    private void MoveForward()
    {
        float currentZ = _rb.position.z;
        float distance = TARGET_Z - currentZ;
        float acceleration = 100 * distance / (TIME_DURATION * TIME_DURATION) / FRICTION;
        float force = _rb.mass * acceleration;
        
        _forceVector = new Vector3(0f, 0f, force);

        _rb.AddForce(_forceVector, ForceMode.Force);
    }
    
    private void ResetToDefaultPosition()
    {
        float currentZ = _rb.position.z;
        float distance = TARGET_Z - currentZ;
        float acceleration = 100 * distance / (TIME_DURATION * TIME_DURATION) / FRICTION;
        float force = _rb.mass * acceleration;

        _forceVector = new Vector3(0f, 0f, force);

        _rb.AddForce(_forceVector, ForceMode.Impulse);
    }

    private bool CanJump()
    {
        return Physics.Raycast(new Ray(transform.position, Vector3.down), 0.2f, LayerMask.GetMask("Ground"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Obstacle rootObj = collision.gameObject.GetComponent<Obstacle>();

        if (rootObj != null)
        {
            SubjectObstacleHitKing.Instance.NotifyObserversTellObstacleHitKing();   
        }
    }

    void OnEnable()
    {
        SubjectObstacleHitKing.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    void OnDestroy()
    {
        SubjectObstacleHitKing.Instance.RemoveObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    public void OnNotifyTellObstacleHitKing()
    {

        if (_obstacleHitRecently)
        {
            Debug.Log("GameEnded");   // put this to Subject to change ui
        }
        
        _forwardTimer = 0f;
        _obstacleHitRecently = true;
    }

}