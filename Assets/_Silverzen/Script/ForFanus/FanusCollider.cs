using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanusCollider : MonoBehaviour
{
    [SerializeField] private Fanus levelEndFanus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowMoney"))
        {
            levelEndFanus.instantMoney(other.transform);
        }
        if (other.CompareTag("Player"))
        {
            if (levelEndFanus.inGameFanus)
            {
                other.GetComponent<MoneyGun>().loseMoney(true);
                gameObject.SetActive(false);
            }
            else if(levelEndFanus.inGameFanus == false)
            {
                other.GetComponent<Player>().gameOver();
            } 
        }
    }
}
