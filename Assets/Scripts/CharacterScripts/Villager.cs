using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField]
    private King king;
    private Vector3 _offset;
    private Rigidbody _rb;

    private bool _followForASecond = true;
    private float _followTimer = 0f;
    private const float FOLLOW_DURATION = 6f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    void OnEnable()
    {
        _offset = new Vector3(0, 0.1f, -3.2f);
        
        Subject.Instance.AddObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
    }

    void OnDestroy()
    {
        Subject.Instance.RemoveObserverTellObstacleHitKing(OnNotifyTellObstacleHitKing);
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
            transform.position = new Vector3(king.transform.position.x / 1.7f, 0, -king.transform.position.z / 1f) + _offset;

            if (_followTimer >= FOLLOW_DURATION)
            {
                _followForASecond = false;
                gameObject.SetActive(false);
            }
        }
    }

}
