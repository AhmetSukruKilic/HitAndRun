using UnityEngine;

public class CameraFollow : MonoBehaviour, IObserveTellObstacleHitKing
{
    public KingControler king;
    public Vector3 offset = new Vector3(0, 3.5f, -5f);
    public Vector3 fixedEulerAngles = new Vector3(17.5f, 0f, 0f);

    private bool _followForASecond = true;
    private float _followTimer = 0f;
    private const float FOLLOW_DURATION = 5f;


    public void OnNotifyTellObstacleHitKing()
    {
        _followForASecond = true;
        _followTimer = 0f;
    }

    void LateUpdate()
    {
        if (_followForASecond || _followTimer <= 0.5f)
        {
            return;
        }

        _followTimer += Time.deltaTime;
        transform.position = new Vector3(0, 0, king.transform.position.z) + offset;
        transform.eulerAngles = fixedEulerAngles; // Optional: set a fixed rotation

        if (_followTimer >= FOLLOW_DURATION)
        {
            _followForASecond = false;
        }
    }

    void Start()
    {
        if (king == null)
        {
            Debug.Log("Assign the king.");
            return;
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
}
