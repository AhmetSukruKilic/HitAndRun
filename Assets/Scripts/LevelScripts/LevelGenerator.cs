using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private static LevelGenerator _instance;
    public static LevelGenerator Instance { get => _instance; private set => _instance = value; }

    [SerializeField]
    private Transform _chunkParent;

    [SerializeField]
    private RandomFloor randomFloor;

    [SerializeField]
    private Floor _defaultFloor;

    private Floor _currentFloor;
    private Queue<Floor> pastFloors = new();

    private Vector3 _offsetVector;
    private float _lengthOfFloor;
    private float _destroyIndex;
    private const float CAMERA_DISTANCE = 2.5f;
    private readonly Vector3 INITIAL_POSITION = new Vector3(0, 0, 6);
    public const int INITIAL_NUMBER_FLOORS = 9;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        if (_defaultFloor == null)
        {
            Debug.Log("initialize default floor");
            return;
        }

        InitializeConsts();
        InitialFloors();
    }

    private void InitializeConsts()
    {
        Transform floorBlock = _defaultFloor.transform.Find("FloorBlock");
        if (floorBlock != null)
        {
            _lengthOfFloor = floorBlock.localScale.z;
            _destroyIndex = -3 * _lengthOfFloor / 2 - CAMERA_DISTANCE;
        }
        else
        {
            Debug.LogError("FloorBlock not found in chunkPrefab");
            return;
        }
        _offsetVector = new Vector3(0, 0, _lengthOfFloor * INITIAL_NUMBER_FLOORS + _destroyIndex);
    }
    private void InitialFloors()
    {
        for (int i = 0; i < INITIAL_NUMBER_FLOORS - 1; i++)
        {
            pastFloors.Enqueue(randomFloor.GenerateRandomFloor(INITIAL_POSITION + new Vector3(0, 0, i * _lengthOfFloor))); // inital step
        }

        _currentFloor = randomFloor.GenerateRandomFloor(INITIAL_POSITION + new Vector3(0, 0, (INITIAL_NUMBER_FLOORS - 1) * _lengthOfFloor));
    }

    void Update()
    {
        if (pastFloors?.Peek().transform.position.z < _destroyIndex)
        {
            randomFloor.DeactivateFloor(pastFloors.Dequeue());
            pastFloors.Enqueue(_currentFloor);
            _currentFloor = randomFloor.GenerateRandomFloor(_offsetVector);
        }
    }    
    

}