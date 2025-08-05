using System.Collections.Generic;
using UnityEngine;

public class RandomFloor : MonoBehaviour
{
    [SerializeField]
    private Transform _randomFloorParent;

    [SerializeField]
    private List<Floor> _floorList;
    public List<Floor> FloorList { get => _floorList; }
    public int TotalFloorTypes { get => FloorList.Count; }

    private Dictionary<int, List<Floor>> _deactivatedFloors = new();
    

    void Awake()
    {
        Floor.InitializeFloorTypes(FloorList);

        for (int i = 0; i < TotalFloorTypes; i++)
            _deactivatedFloors[i] = new();
    }

    internal Floor GenerateRandomFloor(Vector3 position)
    {
        int randomNumFloor = Random.Range(0, TotalFloorTypes);
        int randomAngleNumber = Random.Range(0, 2);

        Quaternion rotation = randomAngleNumber == 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        int count = _deactivatedFloors[randomNumFloor].Count;

        if (count > 0)
        {
            return GetRecycledFloor(position, randomNumFloor, rotation, count);
        }

        return Instantiate(FloorList[randomNumFloor], LevelGenerator.Instance.transform.position + position, rotation, _randomFloorParent);
    }

    private Floor GetRecycledFloor(Vector3 position, int randomNumFloor, Quaternion rotation, int count)
    {
        Floor reusedFloor = _deactivatedFloors[randomNumFloor][count - 1];
        _deactivatedFloors[randomNumFloor].RemoveAt(count - 1);
        reusedFloor.transform.position = LevelGenerator.Instance.transform.position + position;
        reusedFloor.transform.rotation = rotation;
        reusedFloor.transform.SetParent(_randomFloorParent);
        reusedFloor.gameObject.SetActive(true);
        return reusedFloor;
    }

    internal void DeactivateFloor(Floor floor)
    {
        int type = floor.FloorType;

        if (!_deactivatedFloors.ContainsKey(type))
        {
            Debug.Log("No such floor.");
            return;
        }

        _deactivatedFloors[type].Add(floor);

        floor.gameObject.SetActive(false);
    }
}








