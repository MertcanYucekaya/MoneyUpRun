using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGridCanvas : MonoBehaviour
{
    [SerializeField] private MergeManager mergeManager;
    [System.Serializable]
    public class MoneyGridManager
    {
        public int cost;
        public Transform moneyGrid;
        public TextMeshProUGUI costText;
        public Button costButton;
        public Image costButtonImage;
    }
    [SerializeField] private List<MoneyGridManager> moneyGridManagers = new();
    [SerializeField] private MergeCanvas mergeCanvas;
    void Start()
    {
        int currentDiamond = mergeManager.getDiamond();
        for(int i = 0; i < moneyGridManagers.Count; i++)
        {
            MoneyGridManager m = moneyGridManagers[i];
            if (PlayerPrefs.GetInt("CostButton" + i) == 1)
            {
                m.costButton.gameObject.SetActive(false);
                resetMoneyGrid(m.moneyGrid);
            }
            else
            {
                m.costText.text = m.cost.ToString();
                if(currentDiamond < m.cost)
                {
                    m.costButton.enabled = false;
                    setColor(m.costButtonImage, true);
                }
            }
        }
    }
    public void costButton(int que)
    {
        MoneyGridManager m = moneyGridManagers[que];
        if (mergeManager.getDiamond() >= m.cost)
        {
            resetMoneyGrid(m.moneyGrid);
            m.costButton.gameObject.SetActive(false);
            mergeManager.setDiamond(-m.cost,true);
            PlayerPrefs.SetInt("CostButton" + que, 1);
            costButtonCheck();
            mergeCanvas.addMoneyButtonUpdate(false);
        }
    }
    void resetMoneyGrid(Transform t)
    {
        t.gameObject.SetActive(true);
    }
    void setColor(Image image,bool deActive)
    {
        if (deActive)
        {
            image.color = Color.gray;
        }
        else
        {
            image.color = Color.white;
        }
        
    }
    public void costButtonCheck()
    {
        int currentDiamond = mergeManager.getDiamond();
        for (int i = 0; i < moneyGridManagers.Count; i++)
        {
            MoneyGridManager m = moneyGridManagers[i];
            if (m.costButton.IsActive())
            {
                if (currentDiamond >= m.cost)
                {
                    setColor(m.costButtonImage, false);
                }
                else
                {
                    setColor(m.costButtonImage, true);
                    m.costButton.enabled = false;
                }
            }
        }
    }
}
