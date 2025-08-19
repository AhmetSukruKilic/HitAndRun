using UnityEngine;

public class KingControler : MonoBehaviour, IObserveTellObstacleHitKing
{
    private KingStats _kingStats;
    public KingStats KingStats { get => _kingStats; private set => _kingStats = value; }

    private Rigidbody _rb;

    private float _forwardTimer = 0;
    private bool _obstacleHitRecently = true;
    private Vector3 _forceVector;

    private Obstacle _alreadyCollidedObstacle = null;

    private ITriggerable _alreadyTriggeredTriggerable = null;

    private const float FRICTION = 0.5f;
    private const float TIME_DURATION = 4;
    private readonly Vector3 START_SET = new(0, 0.1f, -0.3f);
    private const float TARGET_Z = 0f;

    void Start()
    {
        KingStats = new KingStats();

        transform.position = START_SET;

        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        SubjectObstacleHitKing.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
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
        if (!collision.gameObject.TryGetComponent<Obstacle>(out var obstacleObj))
        {
            return;
        }

        if (_alreadyCollidedObstacle != obstacleObj)
        {
            SubjectObstacleHitKing.Instance.NotifyObserversTellObstacleHitKing();
            _alreadyCollidedObstacle = obstacleObj;
        }
    }

    void OnTriggerStay(Collider triggered)
    {
        if (!triggered.gameObject.TryGetComponent<ITriggerable>(out var triggerableObj))
        {
            return;
        }

        if (_alreadyTriggeredTriggerable != triggerableObj)
        {
            triggerableObj.Triggered();
            _alreadyTriggeredTriggerable = triggerableObj;
        }

        // add coin and health
    }

    public void OnNotifyTellObstacleHitKing()
    {
        _forwardTimer = 0f;
        _obstacleHitRecently = true;
    }

    void OnDestroy()
    {
        SubjectObstacleHitKing.Instance.RemoveObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }
}