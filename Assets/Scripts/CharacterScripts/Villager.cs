using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField]
    private KingControler _king;
    private Vector3 _offset;
    private Rigidbody _rb;

    private bool _followForASecond = true;
    private float _followTimer = 0f;
    private const float FRICTION = 0.5f;
    private const float TARGET_Z = -10;
    private const float FOLLOW_DURATION = 6f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void OnEnable()
    {
        _offset = new Vector3(0, 0.1f, -2.6f);

        SubjectObstacleHitKing.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    void OnDestroy()
    {
        SubjectObstacleHitKing.Instance.RemoveObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    public void OnNotifyTellObstacleHitKing()
    {
        _followForASecond = true;
        _followTimer = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (_followForASecond)
        {
            _followTimer += Time.deltaTime;

            if (_followTimer <= FOLLOW_DURATION / 1.5)
                transform.position = new Vector3(_king.transform.position.x / 1.7f, 0, -_king.transform.position.z / 1.4f) + _offset;
            else
                MoveBack();
            
            if (_followTimer >= FOLLOW_DURATION)
            {
                _followForASecond = false;
                gameObject.SetActive(false);
            }
            
        }
    }
    
    private void MoveBack()
    {
        float currentZ = _rb.position.z;
        float distance = TARGET_Z - currentZ;
        float acceleration = 100 * distance / (100 * 100) / FRICTION;
        float force = _rb.mass * acceleration;
        
        Vector3 forceVector = new Vector3(0f, 0f, force);

        _rb.AddForce(forceVector, ForceMode.Force);
    }

}
