using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudMovement : MonoBehaviour
{
    private float _speed;

    private void Start()
    {
        _speed = Random.Range(2, 10);
    }

    void Update()
    {
        transform.position += Vector3.right * (Time.fixedDeltaTime * _speed);
    }
}
