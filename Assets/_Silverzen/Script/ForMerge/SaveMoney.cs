using UnityEngine;

public class SaveMoney : MonoBehaviour
{
    [SerializeField] private Transform[] saveMoneyMiddle;
    [SerializeField] private GridSaveMoney[] gridSaveMoney; 

    public void allMoneySave()
    {
        for(int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetInt("SavedMoney" + i, 0);
            if (saveMoneyMiddle[i].childCount > 0)
            {
                PlayerPrefs.SetInt("SavedMoney" + i, int.Parse(saveMoneyMiddle[i].GetChild(0).name));
            }
        }
        foreach (GridSaveMoney g in gridSaveMoney)
        {
            g.saveMoney();
        }
    }
    void OnApplicationPause(bool PauseStatus)
    {
        foreach (GridSaveMoney g in gridSaveMoney)
        {
            g.saveMoney();
        }
    }
}
