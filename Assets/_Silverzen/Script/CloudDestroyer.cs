using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDestroyer : MonoBehaviour
{
    private float _destroyTime = 15f;

    private void OnEnable()
    {
        DestroyTheCloud();
    }

    private void DestroyTheCloud() => Destroy(gameObject, _destroyTime);


}
