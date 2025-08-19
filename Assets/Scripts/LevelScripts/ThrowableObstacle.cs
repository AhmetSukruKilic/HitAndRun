using System.Collections.Generic;
using UnityEngine;

public class ThrowableObstacle : Obstacle, ITriggerable
{
    public int ThrowableObstacleType { get => tagToId[this.gameObject.tag]; }
    private static Dictionary<string, int> tagToId = new();
    private float _speed = 10f;
    private Rigidbody _rb;
    private float JUMP_FORCE = 5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public static void InitializeThrowableObstacleTypes(List<ThrowableObstacle> throwableobstacles)
    {
        foreach (ThrowableObstacle throwableobstacle in throwableobstacles)
        {
            if (!tagToId.ContainsKey(throwableobstacle.gameObject.tag))
            {
                tagToId.Add(throwableobstacle.gameObject.tag, tagToId.Count);
            }
        }
    }
    private bool CanJump()
    {
        return Physics.Raycast(new Ray(transform.position, Vector3.down), 0.2f, LayerMask.GetMask("Ground"));
    }
    private bool ShouldVelocityBeReversed()
    {
        return Physics.Raycast(new Ray(transform.position, Vector3.left), 0.2f, LayerMask.GetMask("Wall")) ||
                Physics.Raycast(new Ray(transform.position, Vector3.right), 0.2f, LayerMask.GetMask("Wall"));
    }
    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.back;

        if (CanJump())
        {
            _rb.linearVelocity = transform.forward * JUMP_FORCE;
        }
        if (ShouldVelocityBeReversed())
        {
            _rb.linearVelocity = new Vector3(-_rb.linearVelocity.x, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }
    }

    public void Triggered()
    {
        SubjectObstacleHitKing.Instance.NotifyObserversTellObstacleHitKing();
    }

}