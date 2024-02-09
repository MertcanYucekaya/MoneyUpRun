using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoney : MonoBehaviour
{
    [SerializeField] private Transform[] mergeMiddle;
    [SerializeField] private GameObject mergeMoney;
    [SerializeField] private MergeManager mergeManager;
    [SerializeField] private MergeCanvas mergeCanvas;
    [SerializeField] private MoneyGridCanvas moneyGridCanvas;

    public void addMoney(bool reduce)
    {
        foreach(Transform t in mergeMiddle)
        {
            if (t.childCount <= 0)
            {
                Transform moneyTransform = Instantiate(mergeMoney, new Vector3(t.position.x, mergeMoney.transform.position.y, t.position.z), mergeMoney.transform.rotation).transform;
                moneyTransform.name = "1";
                moneyTransform.SetParent(t);
                if (reduce)
                {
                    mergeManager.setDiamond(-mergeManager.getDiamondPrice(), true);
                }
                moneyGridCanvas.costButtonCheck();
                return;
            }
        }
    }
    public void checkGrid()
    {
        foreach (Transform t in mergeMiddle)
        {
            if (t.childCount <= 0)
            {
                mergeCanvas.addMoneyButtonUpdate(false);
                return;
            }
        }
        mergeCanvas.addMoneyButtonUpdate(true);
    }
}
