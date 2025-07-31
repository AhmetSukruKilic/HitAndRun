using System.Collections.Generic;
using UnityEngine;

public class RandomThrowableObstacle : MonoBehaviour
{
    [SerializeField]
    private Transform _throwableParent;

    [SerializeField]
    private List<ThrowableObstacle> _throwableObstacleList;
    public List<ThrowableObstacle> ThrowableObstacleList { get => _throwableObstacleList; }
    public int TotalThrowableObstacleTypes { get => ThrowableObstacleList.Count; }

    private Dictionary<int, List<ThrowableObstacle>> _deactivatedObstacles = new();

    void Awake()
    {
        for (int i=0; i<TotalThrowableObstacleTypes; i++)
            _deactivatedObstacles[i] = new();
    }

    internal ThrowableObstacle GenerateRandomFloor(Vector3 position) // made this so it creates randomly
    {
        int randomNum = Random.Range(0, TotalThrowableObstacleTypes);

        if (_deactivatedObstacles[randomNum].Count > 0)
        {
            ThrowableObstacle reusedObstacle = _deactivatedObstacles[randomNum][0];
            _deactivatedObstacles[randomNum].RemoveAt(0);
            reusedObstacle.transform.position = LevelGenerator.Instance.transform.position + position;
            reusedObstacle.transform.rotation = Quaternion.identity;
            reusedObstacle.transform.SetParent(_throwableParent);
            reusedObstacle.gameObject.SetActive(true);
            return reusedObstacle;
        }
        
        return Instantiate(ThrowableObstacleList[randomNum], LevelGenerator.Instance.transform.position + position, Quaternion.identity, _throwableParent);
    }

    internal void DeactivateFloor(ThrowableObstacle throwableObstacle)
    {
        int type = throwableObstacle.FloorType;

        if (!_deactivatedObstacles.ContainsKey(type))
        {
            Debug.Log("No such floor.");
            return;
        }

         _deactivatedObstacles[type].Add(throwableObstacle);

        throwableObstacle.gameObject.SetActive(false);
    }
}