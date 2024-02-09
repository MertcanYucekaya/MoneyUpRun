using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int loseMoneyAmount;
    [Header("ForwardMove")]
    [SerializeField] private ZMove zMove;
    [Header("Horizontal")]
    [SerializeField] private float horizontalSpeed = 1;
    [SerializeField] private float clampValue = 3;
    [SerializeField] private float xMoveLerp;
    private bool isHorizontalMove;
    private float horizontal;
    [Header("Diamond Animation")]
    [SerializeField] private GameObject diamond;
    [SerializeField] private Transform usedDiamond;
    [SerializeField] private Transform unUsedDiamond;
    [SerializeField] private Transform[] diamondInstantPoses;
    [Header("References")]
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform finishLineCameraTarget;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MainCanvas canvas;
    [SerializeField] private MoneyGun moneyGun;
    [SerializeField] private Transform playerLerp;
    [SerializeField] private Transform moneyGunTransform;
    private bool mouseXCheck;
    private void Start()
    {
        mouseXCheck = true;
        DOTween.Init();
        isHorizontalMove = true;
    }
    private void Update()
    {
        playerLerp.position = new Vector3(playerLerp.position.x, playerLerp.position.y, transform.position.z+10);
        if (Input.GetMouseButton(0))
        {
            horizontalMove();
        }
        if (!Mathf.Approximately(transform.position.x, playerLerp.position.x) && isHorizontalMove)
        {
            slop();
        }
    }
    void horizontalMove()
    {
        if (isHorizontalMove)
        {
            horizontal += horizontalSpeed * Input.GetAxis("Mouse X");
            if (mouseXCheck)
            {
                mouseXCheck = false;
                horizontal = 0;
            }
            horizontal = Mathf.Clamp(horizontal, -clampValue, clampValue);
            
            playerLerp.position = new Vector3(horizontal, playerLerp.position.y, playerLerp.position.z);
        } 
    }
    void slop()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerLerp.position.x, transform.position.y, transform.position.z), xMoveLerp*Time.deltaTime);
        moneyGun.rotate(playerLerp);
    }
    public void gameOver()
    {
        playerOver();
        gameManager.gameOver();
    }
    public void gameFailed()
    {
        playerOver();
        gameManager.gameFailed();
    }
    public void playerOver()
    {
        isHorizontalMove = false;
        moneyGun.gameOver();
        moneyGun.enabled = false;
        zMove.init(0);
        moneyGunTransform.DORotate(new Vector3(90, moneyGunTransform.eulerAngles.y, moneyGunTransform.eulerAngles.z), .7f);
    }
    public void glowEffect()
    {
        canvas.glowEffect();
    }
    public void cameraShake()
    {
        cameraFollower.StartShake();
    }
    public void finishLine()
    {
        cameraTarget.position = finishLineCameraTarget.position;
        cameraFollower.changeRotation(finishLineCameraTarget.eulerAngles);
    }
    public void loseMoney()
    {
        gameManager.setDiamond(-loseMoneyAmount, true);
        canvas.diamondPanelGlow();
        if (gameManager.getDiamond() >= 5)
        {
            diamondAnimation();
        }
    }
    void diamondAnimation()
    {
        List<Rigidbody> diamondRigi = new();
        for (int i = 0; i < 5; i++)
        {
            Transform t = null;
            if (unUsedDiamond.childCount > 0)
            {
                t = unUsedDiamond.GetChild(0);
                t.localScale = new Vector3(.5f, .5f, .5f);
                t.gameObject.SetActive(true);
                diamondRigi.Add(t.GetComponent<Rigidbody>());
            }
            else
            {
                t = Instantiate(diamond).transform;
                diamondRigi.Add(t.GetComponent<Rigidbody>());
            }
            t.transform.position = diamondInstantPoses[i].position;
            t.SetParent(usedDiamond);
        }
        foreach(Rigidbody r in diamondRigi)
        {
            r.AddForce(new Vector3(0, Random.Range(.5f, 1), Random.Range(.1f, .5f)) *400);
        }
        foreach (Rigidbody r in diamondRigi)
        {
            r.transform.DOScale(Vector3.zero, 5).OnComplete(() => 
            {
                r.transform.SetParent(unUsedDiamond);
                r.gameObject.SetActive(false);
            });
            
        }
    }
}