using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MainCanvas mainCanvas;
    int currentDiamond;
    private LeveInitManager leveInitManager;
    private void Start()
    {
        currentDiamond = 0;
    }
    public void setDiamond(int diamond, bool updateDiamondText)
    {
        if (getDiamond() + diamond < 0)
        {
            currentDiamond = 0;
            PlayerPrefs.SetInt("Diamond", 0);
        }
        else
        {
            currentDiamond = currentDiamond + diamond;
            PlayerPrefs.SetInt("Diamond", getDiamond() + diamond);
        }
        if (updateDiamondText)
        {
            mainCanvas.setDiamondText();
        }
        Debug.Log(currentDiamond);
    }
    public int getDiamond()
    {
        return PlayerPrefs.GetInt("Diamond");
    }
    public void gameOver()
    {
        leveInitManager.stopAllEnemy();
        mainCanvas.activateGameOverPanel(currentDiamond);
    }
    public void gameFailed()
    {
        leveInitManager.stopAllEnemy();
        mainCanvas.activateGameFailedPanel();
    }
    public void clearData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
    private object _lock = new object();
    public void instantDiamondImage(Transform instantDiamondTransform, int diamondAmount)
    {
        lock (_lock)
        {
            mainCanvas.instantDiamondImage(instantDiamondTransform, diamondAmount);
        }
    }
    public void setLevelInitManager(LeveInitManager leveInitManager)
    {
        this.leveInitManager = leveInitManager;
    }
}
