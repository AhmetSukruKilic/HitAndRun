using UnityEngine;

public class Floor : MonoBehaviour
{
    private int _floorType;
    public int FloorType { get => _floorType; }

    private float _speed = 5f;

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.back;
    }

}