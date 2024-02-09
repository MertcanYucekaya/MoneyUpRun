using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigBankPlatform : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * 14 * Time.deltaTime);
    }
}
