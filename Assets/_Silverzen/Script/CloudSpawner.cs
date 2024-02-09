using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Prefab Object")] public GameObject CloudPrefab;
    [Header("Player Object")] public Transform PlayerTransform;
    [Header("Spawn Time")] public float SpawnTime;
    [Header("Cloud Offset")] public Vector3 CloudOffset;

    private float _timer;

    private void Awake()
    {
        SpawnTime += Random.Range(0f, 15f);
        _timer = SpawnTime;

        Application.targetFrameRate = 60;

    }

    private void Update()
    {
        if (!IsTimerZero())
        {
            GetTimerValue();
        }
        else
        {
            CreateACloud();
            SpawnTime = _timer;
        }
    }
/// <summary>
/// Set the timer value.
/// </summary>
/// <returns></returns>
    private float GetTimerValue() => SpawnTime -= Time.fixedDeltaTime;

/// <summary>
/// Check timer value if it is zero.
/// </summary>
/// <returns></returns>
    private bool IsTimerZero()
    {
        if (SpawnTime <= 0) return true;

        return false;
    }
/// <summary>
/// Create cloud prefab.
/// </summary>
    private void CreateACloud()
{
    CloudOffset.y = Random.Range(-10, -100);
        Instantiate(CloudPrefab, PlayerTransform.position + CloudOffset, transform.rotation);
    }
    
    
}
