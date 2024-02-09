using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] levels;
    [Header("GateElements")]
    [SerializeField] private int gateMultiplierForLevel;
    [SerializeField] private float gateValueMultiplier;
    [Header("EnemyElements")]
    [SerializeField] private int enemyHitpointMultiplierForLevel;
    [SerializeField] private float enemyHitpointValueMultiplier;
    [Header("InGameFanusElements")]
    [SerializeField] private int inGameFanusHitpointMultiplierForLevel;
    [SerializeField] private float inGameFanusHitpointValueMultiplier;
    [Header("ReadylevelElements")] 
    [SerializeField] private Transform playerReady;
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private MainCanvas mainCanvas;
    private LeveInitManager leveInitManager;

    void Start()
    {
        readyLevel();
        if (getLevel() >= levels.Length)
        {
            setLevel(1);
        }
        leveInitManager =  Instantiate(levels[getLevel()], Vector3.zero, Quaternion.identity).GetComponent<LeveInitManager>();
        leveInitManager.setGatePrice(getMultiplierForLevel(), gateMultiplierForLevel, gateValueMultiplier);
        leveInitManager.setEnemyHitpoint(getMultiplierForLevel(), enemyHitpointMultiplierForLevel, enemyHitpointValueMultiplier);
        leveInitManager.setInGameFanusPrice(getMultiplierForLevel(), inGameFanusHitpointMultiplierForLevel, inGameFanusHitpointValueMultiplier);
        mainCanvas.setCurrentLevelText(getMultiplierForLevel()+1);
    }
    public void nextLevel()
    {
        setLevel(getLevel() + 1);
        setMultiplierForLevel(getMultiplierForLevel() + 1);
        SceneManager.LoadSceneAsync("MergeScene");
    }
    public void retryLevel()
    {
        SceneManager.LoadScene("MergeScene");
    }
    public int getLevel()
    {
        return PlayerPrefs.GetInt("level");
    }
    void setLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
    }
    void readyLevel()
    {
        player.transform.position = playerReady.transform.position;
        player.transform.rotation = playerReady.transform.rotation;
    }
    int getMultiplierForLevel()
    {
        return PlayerPrefs.GetInt("MultiplierForLevel");
    }
    void setMultiplierForLevel(int multiplierForLevel)
    {
        PlayerPrefs.SetInt("MultiplierForLevel", multiplierForLevel);
    }
}
