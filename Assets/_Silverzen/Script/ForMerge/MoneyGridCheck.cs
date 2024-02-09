using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGridCheck : MonoBehaviour
{
    [SerializeField] private Transform[] moneyGrids;
    [SerializeField] private MergeCanvas mergeCanvas;
    private void Start()
    {
        Invoke("startCheck", Time.deltaTime);
    }
    public void check()
    {
        foreach(Transform t in moneyGrids)
        {
            if (t.childCount > 0)
            {
                mergeCanvas.startButtonUpdate(true);
                return;
            }
        }
        mergeCanvas.startButtonUpdate(false);
    }
    void startCheck()
    {
        check();
    }
}
