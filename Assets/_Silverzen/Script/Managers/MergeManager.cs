using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private int diamondPrice;
    [Header("Clases")]
    [SerializeField] private MergeCanvas mergeCanvas;
    [SerializeField] private AddMoney addMoney;

    private void Awake()
    {
        setDiamond(824, true);
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Login"))
        {
            PlayerPrefs.SetInt("Login", 1);
            addMoney.addMoney(false);
            mergeCanvas.enableDropTutorial();
        }
        mergeCanvas.setDiamondPriceText(diamondPrice);
    }
    public void setDiamond(int diamond, bool updateDiamondText)
    {
        PlayerPrefs.SetInt("Diamond", getDiamond() + diamond);
        if (updateDiamondText)
        {
            mergeCanvas.setDiamondText();
        }
    }
    public int getDiamond()
    {
        return PlayerPrefs.GetInt("Diamond");
    }
    public int getDiamondPrice()
    {
        return diamondPrice;
    }
}
