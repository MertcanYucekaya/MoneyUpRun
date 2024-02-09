using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Unity.VisualScripting;

public class MoneyGun : MonoBehaviour
{
    [SerializeField] private float fireRate;
    private int[] moneyCounts;
    private List<int> savedMoneys = new();
    [Header("ThrowMoney")]
    [SerializeField] private float throwMoneySpeed;
    [SerializeField] private float maxXDistance;
    [SerializeField] private float maxYDistance;
    [SerializeField] private float distanceDuration;
    [Header("Slop")]
    [SerializeField] private float rotateMultiplier;
    [SerializeField] private Transform moneyGun;
    [SerializeField] private Transform moneyGunForSlop;
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private ZMove playerZMove;
    [SerializeField] private GameObject throwMoney;
    #region Throw money manager
    [System.Serializable]
    public class ThrowMoneyManager
    {
        public Transform changeMoneyParent;
        public Transform[] throwMoneyInstantPosition;
        public MeshRenderer[] throwMoneyInstantPositionMeshRenderer;
        public int throwMoneyParentRotate;
    }
    [SerializeField] public List<ThrowMoneyManager> throwMoneyManager = new();
    private ThrowMoneyManager currentThrowMoneyManager;
    private Transform changeMoneyParent;
    private Transform[] throwMoneyInstantPosition;
    [SerializeField] private Material[] moneyMaterials;
    private int throwMoneyParentRotate;
    #endregion
    [SerializeField] private Transform unUsedThrowMoney;
    [SerializeField] private Transform usedThrowMoney;
    [SerializeField] private Transform throwMoneyDestroyer;
    [SerializeField] private GameObject droppedMoney;
    [SerializeField] private Transform dropMoneyTargetPos;
    private Coroutine fireThread;


    private void Start()
    {
        savedMoneys.Add(5);
        savedMoneys.Add(2);
        savedMoneys.Add(7);
        savedMoneys.Add(1);
        DOTween.Init();
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("SavedMoney" + i) > 0)
            {
                savedMoneys.Add(PlayerPrefs.GetInt("SavedMoney" + i));
            }
        }
        currentThrowMoneyManager = throwMoneyManager[savedMoneys.Count - 1];
        changeMoneyParent = currentThrowMoneyManager.changeMoneyParent;
        changeMoneyParent.gameObject.SetActive(true);
        moneyCounts = new int[savedMoneys.Count];
        for (int i = 0; i < savedMoneys.Count; i++)
        {
            int savedMoney = savedMoneys[i];
            currentThrowMoneyManager.throwMoneyInstantPositionMeshRenderer[i].material = moneyMaterials[savedMoney - 1];
            moneyCounts[i] = savedMoney;
        }
        throwMoneyInstantPosition = currentThrowMoneyManager.throwMoneyInstantPosition;
        throwMoneyParentRotate = currentThrowMoneyManager.throwMoneyParentRotate;

        setThrowMoneySpeed(throwMoneySpeed);
        fireThread = StartCoroutine(fire());
    }
    /*
    IEnumerator fire()
    {
        while (true)
        {
            //instantThrowMoney();
            //changeMoneyParentRotate();
            yield return new WaitForSeconds(fireRate);
        }
    }
    IEnumerator fireRotateWithout()
    {
        while (true)
        {
            instantThrowMoney();
            yield return new WaitForSeconds(fireRate);
        }
    }
    void changeMoneyParentRotate()
    {
        changeMoneyParent.DOLocalRotate(new Vector3(0, 0, changeMoneyParent.eulerAngles.z + throwMoneyParentRotate), fireRate)
                .OnUpdate(() =>
                {
                    for (int i = 0; i < changeMoneyParent.childCount; i++)
                    {
                        changeMoneyParent.GetChild(i).transform.eulerAngles = new Vector3(0, 90, 0);
                    }
                });
    }
    */
    IEnumerator fire()
    {
        float time = 0.0f;
        float currentRotate = changeMoneyParent.eulerAngles.z;
        while (time < fireRate)
        {
            changeMoneyParent.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentRotate, currentRotate + throwMoneyParentRotate, Mathf.InverseLerp(0, 1, time / fireRate)));
            for (int i = 0; i < changeMoneyParent.childCount; i++)
            {
                changeMoneyParent.GetChild(i).transform.eulerAngles = new Vector3(0, 90, 0);
            }
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        changeMoneyParent.eulerAngles = new Vector3(0, 0, currentRotate + throwMoneyParentRotate);
        for (int i = 0; i < changeMoneyParent.childCount; i++)
        {
            changeMoneyParent.GetChild(i).transform.DOPunchScale(Vector3.one * .2f, 0.08f);
        }
        instantThrowMoney();
        fireThread = StartCoroutine(fire());
    }
    void instantThrowMoney()
    {
        for (int i = 0; i < throwMoneyInstantPosition.Length; i++)
        {
            Transform t;
            Transform instantPos = throwMoneyInstantPosition[i];
            if (unUsedThrowMoney.childCount > 0)
            {
                t = unUsedThrowMoney.GetChild(0);
                t.gameObject.SetActive(true);
            }
            else
            {
                t = Instantiate(throwMoney).transform;
            }
            t.name = moneyCounts[i].ToString();
            t.position = instantPos.position;
            t.rotation = Quaternion.Euler(-moneyGun.eulerAngles.x * 3, instantPos.eulerAngles.y, instantPos.eulerAngles.z);
            t.SetParent(usedThrowMoney);
            t.DOMoveX(Random.Range(t.position.x, t.position.x + maxXDistance), distanceDuration);
            t.DOMoveY(Random.Range(t.position.y, t.position.y + maxYDistance), distanceDuration);
        }
    }
    public void loseMoney(bool rebound)
    {
        /*
        int highMoney = 0;
        for (int i = 0; i < moneyCounts.Length; i++)
        {
            if (moneyCounts[i] > moneyCounts[highMoney])
            {
                highMoney = i;
            }
        }
        */
        player.loseMoney();
        player.glowEffect();
        if (rebound)
        {
            player.cameraShake();
            playerZMove.setMove(false);
            transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z - 3), 1f, 1, 0.4f).OnComplete(() =>
            {
                playerZMove.setMove(true);
            });
        }
        /*
        if (currentThrowMoneyManager != throwMoneyManager[0] || moneyCounts[highMoney] > 1)
        {
            Vector3 dropPos;
            if (transform.position.x >= 0)
            {
                dropPos = new Vector3(dropMoneyTargetPos.position.x - 1.5f, dropMoneyTargetPos.position.y, dropMoneyTargetPos.position.z);
            }
            else
            {
                dropPos = new Vector3(dropMoneyTargetPos.position.x + 1.5f, dropMoneyTargetPos.position.y, dropMoneyTargetPos.position.z);
            }
            GameObject g = Instantiate(droppedMoney, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), throwMoney.transform.rotation);
            g.transform.name = moneyCounts[highMoney].ToString();
            g.transform.DOMove(dropPos, .6f).OnComplete(() =>
            {
                Destroy(g, 3);
            });
            g.transform.DORotate(new Vector3(0, Random.Range(0, 360), 0), .6f);
        }
        if (moneyCounts[highMoney] == 1 && currentThrowMoneyManager != throwMoneyManager[0])
        {
            changeMoneyParent.gameObject.SetActive(false);
            currentThrowMoneyManager = throwMoneyManager[currentThrowMoneyManager.changeMoneyParent.childCount - 2];
            changeMoneyParent = currentThrowMoneyManager.changeMoneyParent;
            changeMoneyParent.gameObject.SetActive(true);
            moneyCounts = new int[savedMoneys.Count];
            for (int i = 0; i < currentThrowMoneyManager.changeMoneyParent.childCount; i++)
            {
                int savedMoney = 1;
                currentThrowMoneyManager.throwMoneyInstantPositionMeshRenderer[i].material = moneyMaterials[savedMoney - 1];
                moneyCounts[i] = savedMoney;
            }
            throwMoneyInstantPosition = currentThrowMoneyManager.throwMoneyInstantPosition;
            throwMoneyParentRotate = currentThrowMoneyManager.throwMoneyParentRotate;
            StopCoroutine(fireThread);
            fireThread = StartCoroutine(fire());
        }
        if (moneyCounts[highMoney] > 1)
        {
            moneyCounts[highMoney]--;
            for (int i = 0; i < currentThrowMoneyManager.changeMoneyParent.childCount; i++)
            {
                if (moneyCounts[highMoney] + 1 == int.Parse(string.Concat(currentThrowMoneyManager.throwMoneyInstantPositionMeshRenderer[i].material.name.Where(char.IsDigit))))
                {
                    currentThrowMoneyManager.throwMoneyInstantPositionMeshRenderer[i].material = moneyMaterials[moneyCounts[highMoney] - 1];
                    return;
                }
            }
        }
        */
    }
    public void rangeUpdate(float range)
    {
        throwMoneyDestroyer.position = new Vector3(throwMoneyDestroyer.position.x, throwMoneyDestroyer.position.y,
            Mathf.Clamp(throwMoneyDestroyer.position.z + range, transform.position.z + 8, transform.position.z + 15));
    }
    public void fireRateUpdate(float rate)
    {
        fireRate = Mathf.Clamp(fireRate - rate, .15f, 2);
        /*
        StopCoroutine(fireThread);
        changeMoneyParent.DOKill();
        if (moneyCounts.Length > 1)
        {
            fireThread = StartCoroutine(fire());
        }
        else
        {
            fireThread = StartCoroutine(fireRotateWithout());
        }
        */
        throwMoneySpeed = throwMoneySpeed + (fireRate * 4);
        setThrowMoneySpeed(throwMoneySpeed);
    }
    void setThrowMoneySpeed(float speed)
    {
        PlayerPrefs.SetFloat("ThrowMoneySpeed", speed);
    }

    public void rotate(Transform playerLerp)
    {
        moneyGunForSlop.transform.LookAt(playerLerp);
        moneyGun.rotation = Quaternion.Lerp(moneyGun.rotation, Quaternion.Euler(-(moneyGunForSlop.eulerAngles.y * rotateMultiplier), -90, 0), .125f);
    }
    public void gameOver()
    {
        changeMoneyParent.gameObject.SetActive(false);
        StopCoroutine(fireThread);
    }
    void gameFailed()
    {
        player.gameFailed();
    }
}
