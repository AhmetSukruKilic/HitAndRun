using System.Collections.Generic;
using UnityEngine;

public class RandomThrowableObstacle : MonoBehaviour
{
    [SerializeField]
    private Transform _randomThrowableObstacleParent;

    [SerializeField]
    private List<ThrowableObstacle> _throwableObstacleList;
    public List<ThrowableObstacle> ThrowableObstacleList { get => _throwableObstacleList; }
    public int TotalThrowableObstacleTypes { get => ThrowableObstacleList.Count; }

    private Dictionary<int, List<ThrowableObstacle>> _deactivatedThrowableObstacles = new();
    

    void Awake()
    {
        ThrowableObstacle.InitializeThrowableObstacleTypes(ThrowableObstacleList);

        for (int i = 0; i < TotalThrowableObstacleTypes; i++)
            _deactivatedThrowableObstacles[i] = new();
    }

    internal ThrowableObstacle GenerateRandomThrowableObstacle(Vector3 position)
    {
        int randomNumThrowableObstacle = Random.Range(0, TotalThrowableObstacleTypes);
        int randomAngleNumber = Random.Range(0, 2);

        Quaternion rotation = randomAngleNumber == 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        int count = _deactivatedThrowableObstacles[randomNumThrowableObstacle].Count;

        if (count > 0)
        {
            return GetRecycledThrowableObstacle(position, randomNumThrowableObstacle, rotation, count);
        }

        return Instantiate(ThrowableObstacleList[randomNumThrowableObstacle], LevelGenerator.Instance.transform.position + position, rotation, _randomThrowableObstacleParent);
    }

    private ThrowableObstacle GetRecycledThrowableObstacle(Vector3 position, int randomNumThrowableObstacle, Quaternion rotation, int count)
    {
        ThrowableObstacle reusedThrowableObstacle = _deactivatedThrowableObstacles[randomNumThrowableObstacle][count - 1];
        _deactivatedThrowableObstacles[randomNumThrowableObstacle].RemoveAt(count - 1);
        reusedThrowableObstacle.transform.position = LevelGenerator.Instance.transform.position + position;
        reusedThrowableObstacle.transform.rotation = rotation;
        reusedThrowableObstacle.transform.SetParent(_randomThrowableObstacleParent);
        reusedThrowableObstacle.gameObject.SetActive(true);
        return reusedThrowableObstacle;
    }

    internal void DeactivateThrowableObstacle(ThrowableObstacle ThrowableObstacle)
    {
        int type = ThrowableObstacle.ThrowableObstacleType;

        if (!_deactivatedThrowableObstacles.ContainsKey(type))
        {
            Debug.Log("No such ThrowableObstacle.");
            return;
        }

        _deactivatedThrowableObstacles[type].Add(ThrowableObstacle);

        ThrowableObstacle.gameObject.SetActive(false);
    }
}








