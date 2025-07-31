using UnityEngine;

public class ThrowableObstacle : Obstacle
{
    private int _floorType;
    public int FloorType { get => _floorType; }

    private float _speed = 10f;

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.back;
    }

}