using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoubleGate : MonoBehaviour
{
    [SerializeField] private Gate rightGate;
    [SerializeField] private Gate leftGate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.transform.position.x >= 0)
            {
                leftGate.gateDisable();
                rightGate.upgrade();
                Destroy(rightGate.gameObject);
            }
            else
            {
                rightGate.gateDisable();
                leftGate.upgrade();
                Destroy(leftGate.gameObject);
            }
        }
    }
}
