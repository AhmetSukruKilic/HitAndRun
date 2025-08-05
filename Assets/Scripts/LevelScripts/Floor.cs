using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public int FloorType { get => tagToId[this.gameObject.tag]; }

    private static Dictionary<string, int> tagToId = new();
    private float _speed = 10f;

    public static void InitializeFloorTypes(List<Floor> floors)
    {
        foreach (Floor floor in floors)
        {
            if (!tagToId.ContainsKey(floor.gameObject.tag))
            {
                tagToId.Add(floor.gameObject.tag, tagToId.Count);
            }
        }
    }

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.back;
    }

}