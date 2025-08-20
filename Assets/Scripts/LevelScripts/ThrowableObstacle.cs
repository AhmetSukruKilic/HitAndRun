using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ThrowableObstacle : Obstacle, ITriggerable
{
    public int ThrowableObstacleType { get => tagToId[this.gameObject.tag]; }
    private static Dictionary<string, int> tagToId = new();

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float bounceForce = 2f;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = false;
        _rb.linearVelocity = Vector3.back * moveSpeed;
        _rb.AddTorque(Random.onUnitSphere * Random.Range(5f, 10f), ForceMode.VelocityChange);

        // Make sure it has a bouncy physics material
        Collider col = GetComponent<Collider>();
        if (col != null && col.material == null)
        {
            PhysicsMaterial bouncy = new PhysicsMaterial("Bouncy")
            {
                bounciness = 0.8f,
                bounceCombine = PhysicsMaterialCombine.Multiply,
                frictionCombine = PhysicsMaterialCombine.Minimum
            };
            col.material = bouncy;
        }
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
        return Physics.Raycast(transform.position, Vector3.down, 0.5f, LayerMask.GetMask("Ground"));
    }

    private bool ShouldVelocityBeReversed()
    {
        return Physics.Raycast(transform.position, Vector3.left, 0.5f, LayerMask.GetMask("Wall")) ||
               Physics.Raycast(transform.position, Vector3.right, 0.5f, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        // Constant forward push
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y, -moveSpeed - Random.Range(0f, 4f));

        // Jump if grounded
        if (CanJump())
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, jumpForce, _rb.linearVelocity.z);
        }

        // Bounce off walls
        if (ShouldVelocityBeReversed())
        {
            _rb.linearVelocity = new Vector3(-_rb.linearVelocity.x * bounceForce, _rb.linearVelocity.y, _rb.linearVelocity.z);
        }
    }

    public void Triggered()
    {
        SubjectObstacleHitKing.Instance.NotifyObserversTellObstacleHitKing();
    }
}
