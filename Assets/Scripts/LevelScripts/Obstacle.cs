using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
            Debug.Log("rb for Obstacle {this.name} missing");
    }
}
