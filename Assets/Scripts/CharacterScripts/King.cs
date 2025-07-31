using UnityEngine;

public class King : MonoBehaviour, IObserveTellObstacleHitKing
{
    private Rigidbody _rb;
    private float moveSpeed = 4f;
    private float jumpForce = 5f;

    private const float TARGET_Z = 0f;

    private float _forwardTimer = 0;
    private bool _obstacleHitRecently = true;
    private Vector3 _forceVector;

    private const float FRICTION = 0.5f;
    private const float TIME_DURATION = 4;
    private readonly Vector3 START_SET = new(0, 0.1f, -1f);

    void Start()
    {
        transform.position = START_SET;

        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 move = new Vector3(horizontal * moveSpeed, _rb.linearVelocity.y, 0);
        _rb.linearVelocity = move;

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

        _rb.AddForce(_forceVector * 2, ForceMode.Impulse);
    }

    private bool CanJump()
    {
        return Physics.Raycast(new Ray(transform.position, Vector3.down), 0.2f, LayerMask.GetMask("Ground"));
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject rootObj = collision.gameObject;

        if (rootObj != null)
        {
            if (rootObj.CompareTag("Obstacle"))
            {
                Subject.Instance.NotifyObserversTellObstacleHitKing();
            }
        }
    }

    void OnEnable()
    {
        Subject.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    void OnDestroy()
    {
        Subject.Instance.RemoveObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
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