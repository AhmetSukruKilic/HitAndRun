using System.Collections.Generic;
using UnityEngine;

public class ThrowableObstacle : Obstacle
{
    public int ThrowableObstacleType { get => tagToId[this.gameObject.tag]; }
    private static Dictionary<string, int> tagToId = new();
    private float _speed = 10f;

    public static void InitializeThrowableObstacleTypes(List<ThrowableObstacle> throwableobstacles)
    {
        foreach (ThrowableObstacle throwableobstacle in throwableobstacles)
        {
            return;
            if (!tagToId.ContainsKey(throwableobstacle.gameObject.tag))
            {
                tagToId.Add(throwableobstacle.gameObject.tag, tagToId.Count);
            }
        }
    }

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.back;
    }

}