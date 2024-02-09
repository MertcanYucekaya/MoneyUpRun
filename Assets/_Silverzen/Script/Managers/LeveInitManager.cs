using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeveInitManager : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Gate[] gates;
    [SerializeField] private Fanus[] inGameFanus;
    

    private void Start()
    {
        FindAnyObjectByType<GameManager>().setLevelInitManager(this);
    }
    public void setGatePrice(int level,int multiplierForLevel, float gateMultiplier)
    {
        multiplierForLevel--;
        if (level - multiplierForLevel > 0)
        {
            float multiplier = gateMultiplier * (level - multiplierForLevel);
            foreach (Gate g in gates)
            {
                g.setPrice(multiplier+1);
            }
        }
    }
    public void setEnemyHitpoint(int level, int multiplierForLevel, float hitpointltiplier)
    {
        multiplierForLevel--;
        if (level - multiplierForLevel > 0)
        {
            float multiplier = hitpointltiplier * (level - multiplierForLevel);
            foreach (Enemy e in enemies)
            {
                e.setHitpoint(multiplier+1);
            }
        }
    }
    public void setInGameFanusPrice(int level, int multiplierForLevel, float priceltiplier)
    {
        multiplierForLevel--;
        if (level - multiplierForLevel > 0)
        {
            float multiplier = priceltiplier * (level - multiplierForLevel);
            foreach (Fanus f in inGameFanus)
            {
                f.inGameFanusInit(multiplier+1);
            }
        }
    }
    public void stopAllEnemy()
    {
        foreach(Enemy e in enemies)
        {
            if (e != null)
            {
                e.stopEnemy();
            }
        }
    }
}
