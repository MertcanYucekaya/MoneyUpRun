using UnityEngine;
using TMPro;
using Sirenix.Utilities;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Gate : MonoBehaviour
{
    public enum GateType {FireRange, FireRate}
    [SerializeField] private GateType gateType;
    [SerializeField] private bool doubleDoor;
    [SerializeField] private float currentAmount;
    [SerializeField] private float increaseAmount;
    [SerializeField] private int price;
    private int currentPrice = 0;
    private bool updateFilledImage;
    [Header("Punch")]
    [SerializeField] private float punchDuration;
    [SerializeField] private float minPunchScale;
    [SerializeField] private float maxPunchScale;
    private bool punchRunning = false;
    [Header("ThrowMoneyParticle")]
    [SerializeField] private GameObject throwMoneyParticle;
    [SerializeField] private float particleDeActiveTime;
    [SerializeField] private Transform throwMoneyParticleZ;
    [SerializeField] private Transform usedThrowMoneyParticle;
    [SerializeField] private Transform unUsedThrowMoneyParticle;
    private List<GameObject> particles = new();
    [Header("References")]
    [SerializeField] private Collider col;
    [SerializeField] private Image filledImage;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI currentPriceText;
    [SerializeField] private Canvas canvas;
    ThrowMoneyDestroyer throwMoneyDestroyer;
    MoneyGun moneyGun;
    [Header("GateColorTransition")]
    [SerializeField] private MeshRenderer gateBorderMeshRenderer;
    [SerializeField] private MeshRenderer gateMiddleMeshRenderer;
    [SerializeField] private MeshRenderer gateNotchMeshRenderer;
    [SerializeField] private Material gateBorderMaterialGreen;
    [SerializeField] private Material gateMiddleMaterialGreen;
    [SerializeField] private Material gateBorderMaterialRed;
    [SerializeField] private Material gateMiddleMaterialRed;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        moneyGun = FindObjectOfType<MoneyGun>();
        throwMoneyDestroyer = FindObjectOfType<ThrowMoneyDestroyer>();
        typeText.text = gateType.ToString().SplitPascalCase();
        priceText.text = price + "$";
        setAmountText();
        currentPriceText.text = increaseAmount.ToString();
        updateFilledImage = true;
        colorTransition();
    }
    public void setPrice(float multiplier)
    {
        price = (int)Mathf.Round(price * multiplier);
        priceText.text = price + "$";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowMoney"))
        {
            updateAmount(other);
        }
        if (other.CompareTag("Player"))
        {
            foreach(GameObject g in particles)
            {
                g.transform.SetParent(unUsedThrowMoneyParticle);
                g.SetActive(false);
            }
            if(doubleDoor == false)
            {
                upgrade();
                Destroy(gameObject);
            }
        }
    }
    public void upgrade()
    {
        if (gateType == GateType.FireRange)
        {
            moneyGun.rangeUpdate((currentAmount / 1000) * 7);
        }
        else if (gateType == GateType.FireRate)
        {
            moneyGun.fireRateUpdate((currentAmount / 1000) * 7);
        }
    }
    public void gateDisable()
    {
        col.enabled = false;
        foreach (GameObject g in particles)
        {
            g.transform.SetParent(unUsedThrowMoneyParticle);
            g.SetActive(false);
        }
    }
    private object _lock = new object();
    void updateAmount(Collider other)
    {
        lock (_lock)
        {
            currentPrice += int.Parse(other.name);
            moneyParticle(other);
            setImageFilled();
            punch(minPunchScale);
            if (currentPrice >= price)
            {
                currentPriceCheck();
            }
            throwMoneyDestroyer.throwMoneyDestroy(other.transform);
        }
    }

    void punch(float punchScale)
    {
        if (punchRunning == false)
        {
            punchRunning = true;
            transform.DOPunchScale(Vector3.one * punchScale, punchDuration)
                .OnComplete(() =>
                {
                    punchRunning = false;
                });
        }
        
    }
    void currentPriceCheck()
    {
        currentPrice -= price;
        currentAmount += increaseAmount;
        StartCoroutine(fullFilledImage());
        colorTransition();
        if (currentPrice>= price)
        {
            currentPriceCheck();
        }
        else
        {
            setAmountText();
        }
    }
    void moneyParticle(Collider other)
    {
        GameObject g = null;
        if(unUsedThrowMoneyParticle.childCount > 0)
        {
            g = unUsedThrowMoneyParticle.GetChild(0).gameObject;
            g.SetActive(true);
        }
        else
        {
            g = Instantiate(throwMoneyParticle);
        }
        particles.Add(g);
        g.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, throwMoneyParticleZ.transform.position.z);
        g.transform.SetParent(usedThrowMoneyParticle);
    }
    void setAmountText()
    {
        amountText.text = currentAmount.ToString();
    }
    void setImageFilled()
    {
        if (updateFilledImage)
        {
            filledImage.fillAmount = Mathf.InverseLerp(0, price, currentPrice);
        }
    }
    IEnumerator fullFilledImage()
    {
        updateFilledImage = false;
        filledImage.fillAmount = 1;
        yield return new WaitForSeconds(.1f);
        updateFilledImage = true;
        setImageFilled();
    }
    void colorTransition()
    {
        Material a = null;
        Material b = null;
        if (currentAmount >= 0)
        {
            a = gateBorderMaterialGreen;
            b = gateMiddleMaterialGreen;
        }
        else
        {
            a = gateBorderMaterialRed;
            b = gateMiddleMaterialRed;
        }
        gateBorderMeshRenderer.material = a;
        gateNotchMeshRenderer.material = a;
        gateMiddleMeshRenderer.material = b;
    }
}
