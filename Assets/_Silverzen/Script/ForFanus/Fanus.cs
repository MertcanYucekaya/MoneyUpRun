using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class Fanus : MonoBehaviour
{

    [SerializeField] private int percentInstantMoney;
    [SerializeField] private int maxInstantMoney;
    private int currentPercentInstantMoney;
    private float currentInstantMoney = 0;
    [SerializeField] private float levelEndMoneyForce;
    [SerializeField] private float levelEndMoneyStartForce;
    private List<Rigidbody> moneyRigidbodies = new();
    private int moneyInstantQue = 0;
    private int currentHitpoint;
    [Header("InGameFanus")]
    [SerializeField] public bool inGameFanus;
    [Range(1,3)][SerializeField] private int instantDiamondAmount;
    [System.Serializable]
    public class DiamondManager
    {
        public Transform diamondParent;
        public FanusDiamond[] fanusDiamonds;
    }
    [SerializeField] public List<DiamondManager> diamondManager = new();
    [Header("Explosive")]
    [SerializeField] private LayerMask exloidLayer;
    [Header("Diamond")]
    [SerializeField] public int diamondAmount;
    [Header("References")]
    [SerializeField] private FanusDiamond fanusDiamond;
    [SerializeField] private Transform fanusDiamondTarget;
    [SerializeField] private GameObject levelEndMoney;
    [SerializeField] private Transform[] moneyInstantPos;
    [SerializeField] private Transform fanus;
    [SerializeField] private GameObject fragileFanus;
    [SerializeField] private Rigidbody[] fragileFanusRigidbody;
    [SerializeField] private GameObject[] disabledObjectsForExploid;
    [SerializeField] private Transform usedLevelEndMoney;
    [SerializeField] private Transform unUsedLevelEndMoney;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI hitpointText;

    private void Start()
    {
        currentHitpoint = percentInstantMoney;
        setHitpointText();
        if (inGameFanus)
        {
            diamondManager[instantDiamondAmount-1].diamondParent.gameObject.SetActive(true);
        }
    }
    private object _lock = new object();
    public void instantMoney(Transform t)
    {
        lock (_lock)
        {
            currentPercentInstantMoney += int.Parse(t.name.ToString());
            currentHitpoint -= int.Parse(t.name.ToString());
            setHitpointText();
            float percent = (float)currentPercentInstantMoney / percentInstantMoney;
            fanus.DOKill();
            fanus.DOShakePosition(.1f, new Vector3(.05f, 0, .05f), 0, 0);
            if (percent < 1)
            {
                currentInstantMoney = maxInstantMoney * percent;
                int c = (int)currentInstantMoney - moneyRigidbodies.Count;
                instantMoneyLoop(c);
            }
            else
            {
                Destroy(canvas);
                diamondAnimation();
                instantMoneyLoop(maxInstantMoney - moneyRigidbodies.Count);
                explosive();
            }
            Destroy(t.gameObject);
        }
    }
    void instantMoneyLoop(int c)
    {
        for (int i = 0; i < c; i++)
        {
            Rigidbody rigi;
            if (unUsedLevelEndMoney.childCount < 1)
            {
                rigi  = Instantiate(levelEndMoney).GetComponent<Rigidbody>();
            }
            else
            {
                rigi = unUsedLevelEndMoney.GetChild(0).GetComponent<Rigidbody>();
                rigi.gameObject.SetActive(true);
            }
            rigi.rotation = Quaternion.identity;
            rigi.transform.position = moneyInstantPos[moneyInstantQue].position;
            rigi.transform.SetParent(usedLevelEndMoney);
            rigi.AddForce(Vector3.up * levelEndMoneyStartForce);
            moneyRigidbodies.Add(rigi);
            moneyInstantQue++;
            if (moneyInstantQue >= moneyInstantPos.Length)
            {
                moneyInstantQue = 0;
            }
        }
        foreach (Rigidbody r in moneyRigidbodies)
        {
            r.AddForce(Vector3.up * levelEndMoneyForce);
        }
    }
    void explosive()
    {
        foreach(GameObject g in disabledObjectsForExploid)
        {
            g.SetActive(false);
        }
        fragileFanus.SetActive(true);
        foreach(Rigidbody rb in moneyRigidbodies)
        {
            //rb.AddExplosionForce(exploidForce, exlpoidPos.position, exploidRadius, 3.0F);
            rb.transform.tag = "LevelEnd/Money";
        }
        /*
        foreach (Rigidbody rb in fragileFanusRigidbody)
        {
            rb.AddExplosionForce(exploidForce, exlpoidPos.position, exploidRadius, 3.0F);
        }
        */
        StartCoroutine(setParentLevelEndMoney());
    }
    IEnumerator setParentLevelEndMoney()
    {
        yield return new WaitForSeconds(1);
        foreach (Rigidbody r in fragileFanusRigidbody)
        {
            r.transform.DOScale(Vector3.zero, 2);
        }
        foreach (Rigidbody r in moneyRigidbodies)
        {
            r.transform.DOScale(Vector3.zero, 2).OnComplete(() => 
            {
                r.transform.localScale = new Vector3(.14f, .14f, .14f);
                r.transform.tag = "Untagged";
                r.gameObject.SetActive(false);
                r.transform.SetParent(unUsedLevelEndMoney);
                Destroy(gameObject);
            });
        }
        
    }
    void diamondAnimation()
    {
        if (inGameFanus)
        {
            DiamondManager d = diamondManager[instantDiamondAmount - 1];
            for (int i = 0; i < d.fanusDiamonds.Length; i++)
            {
                d.fanusDiamonds[i].active(new Vector3(d.fanusDiamonds[i].transform.position.x,fanusDiamondTarget.position.y, d.fanusDiamonds[i].transform.position.z));
            }
        }
        else
        {
            fanusDiamond.active(fanusDiamondTarget.position);
        }
    }
    void setHitpointText()
    {
        if (currentHitpoint >= 0)
        {
            hitpointText.text = currentHitpoint.ToString();
        }
        else
        {
            hitpointText.text = 0 + "";
        }
    }
    public void fanusInit(int i)
    {
        percentInstantMoney = i;
        hitpointText.text = i.ToString();
    }
    public void inGameFanusInit(float multiplier)
    {
        percentInstantMoney = (int)Mathf.Round(percentInstantMoney * multiplier);
        hitpointText.text = hitpointText + "$";
    }
}
