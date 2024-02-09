using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider col;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<MoneyGun>().loseMoney(true);
            col.enabled = false;
        }
    }
}
