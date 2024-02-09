using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private bool hit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hit)
        {
            foreach (GameObject g in enemy.particles)
            {
                g.transform.SetParent(enemy.unUsedThrowMoneyParticle);
                g.SetActive(false);
            }
            other.GetComponent<MoneyGun>().loseMoney(true);
            hit = false;
        }
        if (other.CompareTag("ThrowMoney"))
        {
            //moneyParticle(other);
            enemy.getDamage(int.Parse(other.name));
            Destroy(other.gameObject);
        }
        if (other.CompareTag("EnemyDestroyer"))
        {
            Destroy(enemy.gameObject);
        }
    }
    void moneyParticle(Collider other)
    {
        GameObject g = null;
        if (enemy.unUsedThrowMoneyParticle.childCount > 0)
        {
            g = enemy.unUsedThrowMoneyParticle.GetChild(0).gameObject;
            g.SetActive(true);
        }
        else
        {
            g = Instantiate(enemy.throwMoneyParticle);
        }
        enemy.particles.Add(g);
        g.transform.position = other.transform.position;
        g.transform.SetParent(enemy.usedThrowMoneyParticle);
    }
}
