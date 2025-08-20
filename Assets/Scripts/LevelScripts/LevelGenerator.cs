using System;
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
    private RandomThrowableObstacle randomThObstacle;

    [SerializeField]
    private readonly Queue<ThrowableObstacle> pastThObstacles = new();

    [SerializeField]
    private Floor _defaultFloor;
    private Floor _currentFloor;
    private readonly Queue<Floor> pastFloors = new();

    private float _timeCountForObstacleThrow = 0f;
    private const float THROW_TIME = 3f;

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

        pastThObstacles.Enqueue(randomThObstacle.GenerateRandomThrowableObstacle(randomThObstacle.transform.position));  // change the throw position
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
            BuildAndDestroyFloor();
        }

        _timeCountForObstacleThrow += Time.deltaTime;

        if (_timeCountForObstacleThrow >= THROW_TIME)
        {
            BuildThObstacle();
            _timeCountForObstacleThrow = 0;
        }

        if (pastThObstacles.Count >= 10)
        {
            DestroyThObstacle();
        }

    }

    private void BuildAndDestroyFloor()
    {
        randomFloor.DeactivateFloor(pastFloors.Dequeue());
        pastFloors.Enqueue(_currentFloor);
        _currentFloor = randomFloor.GenerateRandomFloor(_offsetVector);
    }

    private void BuildThObstacle()
    {
        pastThObstacles.Enqueue(randomThObstacle.GenerateRandomThrowableObstacle(randomThObstacle.transform.position));  // change the throw position
    }

    private void DestroyThObstacle()
    {
        randomThObstacle.DeactivateThrowableObstacle(pastThObstacles.Dequeue());
    }

}